using Ads.DataAccess.Domain;
using Ads.DataAccess.EfDataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ads.Infrastructure.EntityFramework
{
    public class SeedAdminUser
    {
        public static async Task SeedAsync(AdsContext context, UserManager<ApplicationUser> userManager)
        {
            if (!await context.Users.AnyAsync(x => x.Email == "admin@domain.com"))
            {
                var user = new ApplicationUser()
                {
                    Email = "admin@domain.com",
                    UserName = "admin@domain.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };
                await userManager.CreateAsync(user, "password");

                await userManager.AddToRoleAsync(user, "Admin");
            }

        }
    }
}