using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.Api.Models;

namespace Contact.Api.Data
{
    public interface IContactApplyRequestRepository
    {
        /// <summary>
        /// 添加申请好友请求
        /// </summary>
        /// <param name="contactApplyRequest"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApplyRequest contactApplyRequest, CancellationToken cancellationToken);
        /// <summary>
        /// 处理（通过）好友请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId,int applierId, CancellationToken cancellationToken);
        /// <summary>
        /// 获取请求好友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken);
    }
}
