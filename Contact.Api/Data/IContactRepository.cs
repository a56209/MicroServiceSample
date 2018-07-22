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
        Task<bool> AddContactAsync(int contactId,BaseUserInfo contact, CancellationToken cancellationToken);


    }
}
