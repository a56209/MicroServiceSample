using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using User.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace User.Api.Test
{
    public class UserControllerUnitests
    {
        private Data.UserContext GetUserContext()
        {
            var option = new DbContextOptionsBuilder<Data.UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var userContext = new Data.UserContext(option);

            userContext.Users.Add(new Models.AppUser
            {
                Id = 1,
                Name = "a56209"
            });

            userContext.SaveChanges();
            return userContext;

        }

        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParamentersAsync()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;

            var controller = new UserController(context, logger);

            var response = await controller.Get();

            Assert.IsType<JsonResult>(response);
        }
    }
}
