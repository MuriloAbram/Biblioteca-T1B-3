using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace T1B_3Library.Infrastructure.Identity
{
    public static class SeedData
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            const string adminRole = "Admin";
            const string adminEmail = "admin@localhost";
            const string adminPassword = "Admin@123";

            // Cria role Admin se não existir
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            // Cria usuário admin se não existir
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
                else
                {
                    // opcional: log de erros
                }
            }
            else
            {
                // garante que o usuário tenha a role Admin
                if (!await userManager.IsInRoleAsync(admin, adminRole))
                    await userManager.AddToRoleAsync(admin, adminRole);
            }
        }
    }
}