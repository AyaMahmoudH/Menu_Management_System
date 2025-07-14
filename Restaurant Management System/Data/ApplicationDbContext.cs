using EnumsNET;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data.Enum;
using Restaurant_Management_System.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Category> Categories { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.MenuItems)
            .WithOne(m => m.Category)
            .HasForeignKey(m => m.CategoryId);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasColumnType("decimal(18,2)");


        modelBuilder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "Appetizers" },
    new Category { Id = 2, Name = "Main Courses" },
    new Category { Id = 3, Name = "Desserts" },
    new Category { Id = 4, Name = "Beverages" }
);
        modelBuilder.Entity<MenuItem>().HasData(
    new MenuItem
    {
        Id = 1,
        Name = "Spring Rolls",
        Description = "Crispy rolls stuffed with fresh vegetables.",
        Price = 5.99m,
        ImageUrl = "~/Images/SpringRolls.jpg",
        IsAvailable = true,
        CategoryId = 1
    },
    new MenuItem
    {
        Id = 2,
        Name = "Garlic Bread",
        Description = "Toasted bread with garlic and herbs.",
        Price = 4.99m,
        ImageUrl = "~/Images/GarlicBread.png",
        IsAvailable = true,
        CategoryId = 1
    },
    new MenuItem
    {
        Id = 3,
        Name = "Bruschetta",
        Description = "Grilled bread topped with tomato and basil.",
        Price = 6.49m,
        ImageUrl = "~/Images/Bruschetta.png",
        IsAvailable = true,
        CategoryId = 1
    },

   
    new MenuItem
    {
        Id = 5,
        Name = "Beef Steak",
        Description = "Tender beef steak cooked to perfection.",
        Price = 24.99m,
        ImageUrl = "~/Images/GrilledChickenBreast.jpg",
        IsAvailable = true,
        CategoryId = 2
    },
    new MenuItem
    {
        Id = 6,
        Name = "Salmon Fillet",
        Description = "Grilled salmon fillet with lemon butter sauce.",
        Price = 21.50m,
        ImageUrl = "~/Images/SalmonFillet.png",
        IsAvailable = true,
        CategoryId = 2
    },

    new MenuItem
    {
        Id = 7,
        Name = "Chocolate Cake",
        Description = "Rich chocolate cake with ganache.",
        Price = 7.99m,
        ImageUrl = "~/Images/69b1c95c-1c07-4f76-b889-a6bebceaf14c.png",
        IsAvailable = true,
        CategoryId = 3
    },
    new MenuItem
    {
        Id = 8,
        Name = "Cheesecake",
        Description = "Creamy cheesecake with berry topping.",
        Price = 8.50m,
        ImageUrl = "~/Images/Cheesecake.jpg",
        IsAvailable = true,
        CategoryId = 3
    },
    new MenuItem
    {
        Id = 9,
        Name = "Fruit Tart",
        Description = "Tart with fresh seasonal fruits.",
        Price = 6.75m,
        ImageUrl = "~/Images/FruitTart.png",
        IsAvailable = true,
        CategoryId = 3
    }
);




    }
}
