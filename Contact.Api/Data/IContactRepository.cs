using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.Api.Dtos;

namespace Contact.Api.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfoAsync(BaseUserInfo userInfo, CancellationToken cancellationToken);
        Task<bool> AddContactAsync(int userId,BaseUserInfo contact, CancellationToken cancellationToken);

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactListAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// 更新好友标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagContanctsAsync(int userId, int contactId, List<string> tags, CancellationToken cancellationToken);
    }
}
