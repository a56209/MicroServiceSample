﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Api.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace User.Api.Controllers
{    
    [Route("api/Users")]
    public class UserController : BaseController
    {
        private UserContext _userContext;
        private ILogger<UserController> _logger;

        public UserController(UserContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("查询日志记录");
            var user = await _userContext.Users
                .AsNoTracking()
                .Include(u => u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            if (user == null)
            {
                //return NotFound();
                throw new UserOperationException($"错误的上下文Id：{UserIdentity.UserId}");
            }
            return Json(user);
        }

        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<Models.AppUser> patch)
        {
            var user =await _userContext.Users
                //.Include(u=>u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            patch.ApplyTo(user);

            foreach(var property in user?.Properties)
            {
                _userContext.Entry(property).State = EntityState.Detached;
            }

            var originProperties = await _userContext.Userproperties.AsNoTracking().Where(u=>u.AppUserId== UserIdentity.UserId).ToListAsync();
            var allProperties = originProperties.Union(user.Properties).Distinct();

            var removeProperties = originProperties.Except(user.Properties);
            var newProperties = allProperties.Except(originProperties);

            foreach(var property in removeProperties)
            {
                _userContext.Remove(property);
            }

            foreach(var property in newProperties)
            {
                _userContext.Add(property);
            }

            _userContext.Users.Update(user);
            _userContext.SaveChanges();
            return Json(user);
        }

        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            //TODO:手机格式验证
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Tel == phone);
            if(user == null)
            {
                user = new Models.AppUser { Tel = phone };
                _userContext.Users.Add(user);
                await _userContext.SaveChangesAsync();
            }

            return Ok(user.Id);
        }

        /// <summary>
        /// 获取用户标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetUserTag()
        {
            return Ok(await _userContext.UserTags.Where(u =>u.UserId==UserIdentity.UserId).ToListAsync());

        }

        /// <summary>
        /// 根据手机号码查询用户资料
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(string phone)
        {
            return Ok(await _userContext.Users.Include(u => u.Properties).Where(u => u.Id == UserIdentity.UserId).ToListAsync());
        }

        /// <summary>
        /// 更新用户标签数据
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> UpdateUserTag([FromBody]List<string> tages)
        {
            var originTags = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            var newTags = tages.Except(originTags.Select(t => t.Tag));

            await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new Models.UserTag {
                CreatedTime = DateTime.Now,
                UserId = UserIdentity.UserId,
                Tag = t
            }));

            await _userContext.SaveChangesAsync();
            return Ok();
        }
    }
}