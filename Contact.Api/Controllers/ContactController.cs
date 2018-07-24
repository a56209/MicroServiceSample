using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contact.Api.Data;
using Contact.Api.Models;
using Contact.Api.Service;
using System.Threading;
using Contact.Api.ViewModels;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _ContactApplyRequestRepositroy;
        private IContactRepository _contactRepository;
        private IUserService _userService;
        protected ContactController(IContactApplyRequestRepository ContactApplyRequestRepositroy, IContactRepository contactRepository, IUserService userService)
        {
            _ContactApplyRequestRepositroy = ContactApplyRequestRepositroy;
            _contactRepository = contactRepository;
            _userService = userService;
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactListAsync(UserIdentity.UserId, cancellationToken));
        }

        [HttpPut]
        [Route("tag")]
        public async Task<IActionResult> TagContact([FromBody]TagContactInputViewModel tagContactInput, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.TagContanctsAsync(UserIdentity.UserId, tagContactInput.ContactId, tagContactInput.Tags, cancellationToken);
            if(result)
            {
                return Ok();
            }

            //LOG TBD
            return BadRequest();
        }

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult>GetApplyRequest(CancellationToken cancellationToken)
        {
            var request = await _ContactApplyRequestRepositroy.GetRequestListAsync(UserIdentity.UserId,cancellationToken);
            return Ok(request);
        }

        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequest(int userId, CancellationToken cancellationToken)
        {
            var userBaseInfo = await _userService.GetBaseUserInfoAsync(userId);
            if (userBaseInfo == null)
            {
                throw new Exception("用户参数错误");
            }

            var result = await _ContactApplyRequestRepositroy.AddRequestAsync(new ContactApplyRequest
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = userBaseInfo.Name,
                Company = userBaseInfo.Company,
                Title = userBaseInfo.Title,
                ApplyTime = DateTime.Now,
                Avatar = userBaseInfo.Avatar
            },cancellationToken);

            if (! result)
            {
                return BadRequest();
            }
            return Ok(); ;
        }

        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-request")]
        public async Task<IActionResult> approvalApplyRequest(int applierId, CancellationToken cancellationToken)
        {
            var result = await _ContactApplyRequestRepositroy.ApprovalAsync(UserIdentity.UserId, applierId,cancellationToken);
            if (! result)
            {
                return BadRequest();
            }

            //申请者信息
            var applier = await _userService.GetBaseUserInfoAsync(applierId);
            //当前用户
            var userBaseInfo = await _userService.GetBaseUserInfoAsync(UserIdentity.UserId);

            //互相加好友
            await _contactRepository.AddContactAsync(UserIdentity.UserId, userBaseInfo, cancellationToken);
            await _contactRepository.AddContactAsync(applierId, applier, cancellationToken);

            return Ok(); ;            
        }

    }
}
