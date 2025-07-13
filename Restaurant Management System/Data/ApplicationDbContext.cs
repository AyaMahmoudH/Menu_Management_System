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
        Name = "Grilled Chicken",
        Description = "Juicy grilled chicken breast with herbs.",
        Price = 18.99m,
        ImageUrl = "~/Images/GrilledChicken.jpg",
        IsAvailable = true,
        CategoryId = 2
    }
);




    }
}
