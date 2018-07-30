using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 检查手机号是否已注册，如果没有注册的话就注册一个用户
        /// </summary>
        /// <param name="phone"></param>
        Task<Dtos.UserInfo> CheckOrCreatAsync(string phone);
    }
}
