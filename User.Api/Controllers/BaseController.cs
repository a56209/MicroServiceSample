using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Dtos;

namespace User.Api.Controllers
{
    public class BaseController:Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity { UserId = 1, Name = "a56209" };

        //protected UserIdentity UserIdentity
        //{
        //    get
        //    {
        //        var user = new UserIdentity();
        //        int s = User.Claims.Count();
        //        user.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        //        user.Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
        //        user.Company = User.Claims.FirstOrDefault(c => c.Type == "company").Value;
        //        user.Title = User.Claims.FirstOrDefault(c => c.Type == "title").Value;
        //        user.Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar").Value;

        //        return user;
        //    }
        //}
    }
}
