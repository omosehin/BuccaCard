using Buccacard.Domain;
using Buccacard.Infrastructure;
using Buccacard.Repository.DbContext;
using HermesApp.Infrastructure.Dictionary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Buccacard.Repository
{
    public class Seed
    {
        public static async Task SeedData(UserDbContext context,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    Email = "superadmin@gmail.com",
                    UserName = "superadmin",
                    EmailConfirmed = true
                };
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user, "Lsc100ab@");
                user.PasswordHash = hashed;
                var result = await userManager.CreateAsync(user, "Lsc100ab@");

                if (result.Succeeded)
                {
                    var newRole = new IdentityRole(UserRole.SuperAdmin.ToString());

                    var role = await roleManager.CreateAsync(newRole); 
                    if (!await userManager.IsInRoleAsync(user, UserRole.SuperAdmin.ToString()))
                    {
                        await userManager.AddToRoleAsync(user, UserRole.SuperAdmin.ToString());
                    }
                }
                await context.SaveChangesAsync();

            }
        }

    }
}
