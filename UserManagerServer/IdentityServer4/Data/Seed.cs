using IdentityData;
using IdentityData.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public static class Seed
    {
        public static IHost MigrateAndSeed(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();

                        CreateRolesAsync(scope);

                        CreateDefaultAdmin(scope);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return webHost;
        }

        private static void CreateRolesAsync(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            AddRole("user", roleManager);
            AddRole("admin", roleManager);
        }

        private static void AddRole(string roleName, RoleManager<AppRole> roleManager)
        {
            var role = new AppRole { Id = Guid.NewGuid(), Name = roleName };
            var isRoleExist = roleManager.FindByNameAsync(role.Name).Result;

            if (isRoleExist == null)
            {
                var result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        public static void CreateDefaultAdmin(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var admin = userManager.FindByNameAsync("admin").Result;

            if (admin != null)
            {
                Console.WriteLine("admin already exists");
                return;
            }

            admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@email.com",
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(admin, "Admin123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userManager.AddClaimsAsync(admin,
                new Claim[] { 
                    new Claim(JwtClaimTypes.Role, "admin"),
                    new Claim(JwtClaimTypes.Email, "admin@email.com")
                }).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }


            Console.WriteLine("admin created");
        }
    }
}
