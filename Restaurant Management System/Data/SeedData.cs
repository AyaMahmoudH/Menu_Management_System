using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            string[] roleNames = { "Admin", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

           string adminEmail = "admin@restaurant.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Appetizers" },
                    new Category { Name = "Main Courses" },
                    new Category { Name = "Desserts" },
                    new Category { Name = "Beverages" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.MenuItems.Any())
            {
                var springRolls = new MenuItem
                {
                    Name = "Spring Rolls",
                    Description = "Crispy rolls stuffed with fresh vegetables.",
                    Price = 5.99m,
                    ImageUrl = "~/Images/SpringRolls.jpg",
                    IsAvailable = true,
                    CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Appetizers").Id
                };

                var grilledChicken = new MenuItem
                {
                    Name = "Grilled Chicken",
                    Description = "Juicy grilled chicken breast with herbs",
                    Price = 18.99m,
                    ImageUrl = "~/Images/GrilledChicken.jpg",
                    IsAvailable = true,
                    CategoryId = context.Categories.FirstOrDefault(c => c.Name == "Main Courses").Id
                };

                await context.MenuItems.AddRangeAsync(springRolls, grilledChicken);
                await context.SaveChangesAsync();
            }
        }
    }
}
