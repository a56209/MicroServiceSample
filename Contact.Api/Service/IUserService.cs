using Contact.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<UserIdentity> GetBaseUserInfoAsync(int UserId);
    }
}
