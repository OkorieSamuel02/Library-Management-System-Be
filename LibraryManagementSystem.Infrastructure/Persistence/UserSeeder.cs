using LibraryManagementSystem.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Persistence
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {

            var adminExist = await userManager.FindByEmailAsync("admin@library.com");

            if (adminExist != null)
            {
                return;
            }

            var admin = new User
            {
                Email = "admin@library.com",
                UserName = "admin@library.com",
                EmailConfirmed = true,
                PhoneNumber = "08089506729",
                Roles = Domain.Enums.Roles.Admin,
            };

            var result = await userManager.CreateAsync(admin, "Password@@1");

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
