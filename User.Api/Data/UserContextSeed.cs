using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                //userContext.Database.Migrate();
                if (! userContext.Users.Any())
                {
                    userContext.Users.Add(new Models.AppUser() {
                        Name = "a56209",
                        Company = "深圳市顺欣同创科技有限公司",
                        Title = "项目经理"
                    });
                    await userContext.SaveChangesAsync();
                }
            }
        }
    }
}
