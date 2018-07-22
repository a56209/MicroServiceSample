using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contact.Api.Data;
using Contact.Api.Models;
using Contact.Api.Service;
using System.Threading;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _ContactApplyRequestRepositroy;
        private IUserService _userService;
        protected ContactController(IContactApplyRequestRepository ContactApplyRequestRepositroy, IUserService userService)
        {
            _ContactApplyRequestRepositroy = ContactApplyRequestRepositroy;
            _userService = userService;
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
            return Ok(); ;            
        }

    }
}
