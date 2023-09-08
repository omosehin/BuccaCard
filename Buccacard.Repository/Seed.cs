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
            UserManager<AppUser> userManager, RoleManager<AppUser> roleManager)
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

                if (!result.Succeeded)
                {

                    if (!await roleManager.RoleExistsAsync(UserRole.SuperAdmin.ToString()))
                    {
                        var roleStore = new RoleStore<IdentityRole>(context);

                       // await roleManager.CreateAsync(new IdentityRole(roleName: UserRole.SuperAdmin.DisplayName));
                        if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
                        {
                          await  roleStore.CreateAsync(new IdentityRole("SuperAdmin"));
                        }
                    }

                    if (await roleManager.RoleExistsAsync(UserRole.User.ToString()))
                    {
                        await userManager.AddToRoleAsync(user, UserRole.User.ToString());
                    }
                    
                }
                await context.SaveChangesAsync();

            }
        }

    }
}
