using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using User.Api.Controllers;
using User.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using User.Api.Models;

namespace User.Api.Test
{
    public class UserControllerUnitests
    {
        private Data.UserContext GetUserContext()
        {
            var option = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var userContext = new UserContext(option);

            userContext.Users.Add(new Models.AppUser
            {
                Id = 1,
                Name = "a56209"
            });

            userContext.SaveChanges();
            return userContext;
        }

        //private UserController GetUserController()
        //{
        //    var context = GetUserContext();
        //    var loggerMoq = new Mock<ILogger<UserController>>();
        //    var logger = loggerMoq.Object;

        //    var controller = new UserController(context, logger);
        //    return controller;
        //}
        private (UserController controller,UserContext userContext) GetUserController()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            return (controller: new UserController(context, logger), userContext: context);            
        }

        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParamentersAsync()
        {
            //UserController controller = GetUserController();
            (UserController controller, UserContext userContext) = GetUserController();
            //var response = await controller.Get();
            var response = await controller.Get();

            //Assert.IsType<JsonResult>(response);
            //response.Should().BeOfType<JsonResult>();
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("a56209");
        }


        [Fact]
        public async Task Path_ReturnNewName_WithExpctedNewNameParament()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext userContext) = GetUserController();
            var document = new JsonPatchDocument<Models.AppUser>();
            document.Replace(u => u.Name, "amy");
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert response
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;            
            appUser.Name.Should().Be("amy");

            //assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("amy");
        }

        [Fact]
        public async Task Path_ReturnNewProperties_WithAddNewProperty()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty> {
                new UserProperty{
                    Key ="fin_industry",Value="»¥ÁªÍø",Text="»¥ÁªÍø"
                }
            });
            var response = await controller.Patch(document);
            
            var result = response.Should().BeOfType<JsonResult>().Subject;
            //assert response
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.First().Value.Should().Be("»¥ÁªÍø");
            appUser.Properties.First().Key.Should().Be("fin_industry");

            //assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.First().Value.Should().Be("»¥ÁªÍø");
            userModel.Properties.First().Key.Should().Be("fin_industry");
        }

        [Fact]
        public async Task Path_ReturnNewProperties_WithRemoveProperty()
        {
            //var controller = GetUserController();
            (UserController controller, UserContext userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty> {
               
            });
            var response = await controller.Patch(document);

            var result = response.Should().BeOfType<JsonResult>().Subject;
            //assert response
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();

            //assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Should().BeEmpty();            
        }
    }
}
