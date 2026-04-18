using E_Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Data
{
    public static class AppSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<E_LibraryDbContext>();
            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure the database is migrated. For relational databases, do not use EnsureCreated() if using migrations.

            await context.Database.EnsureCreatedAsync();

            // Seed Roles (check if they exist first to ensure idempotency)
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }
    }
}
