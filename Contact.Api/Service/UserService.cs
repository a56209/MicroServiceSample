using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Dtos;

namespace Contact.Api.Service
{
    public class UserService : IUserService
    {
        public Task<BaseUserInfo> GetBaseUserInfoAsync(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
