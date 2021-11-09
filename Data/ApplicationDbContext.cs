using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ElectronicsStore.Models;

namespace ElectronicsStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ShoppingUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, ParentId = 0, Name = "Computers and Components" },
                new Category { Id = 2, ParentId = 0, Name = "Office Equipment" },
                new Category { Id = 3, ParentId = 0, Name = "Home Electronics" },
                new Category { Id = 4, ParentId = 0, Name = "Communication Equipment" },
                new Category { Id = 5, ParentId = 0, Name = "Video Games and Consoles" },
                new Category { Id = 6, ParentId = 0, Name = "Household Appliances" },
                new Category { Id = 7, ParentId = 0, Name = "Other Items" },
                new Category { Id = 8, ParentId = 1, Name = "Laptops" },
                new Category { Id = 9, ParentId = 1, Name = "Tablets" },
                new Category { Id = 10, ParentId = 1, Name = "Computer Components" },
                new Category { Id = 11, ParentId = 2, Name = "Monitors" },
                new Category { Id = 12, ParentId = 2, Name = "Mice" },
                new Category { Id = 13, ParentId = 2, Name = "Keyboards" },
                new Category { Id = 14, ParentId = 3, Name = "TVs" },
                new Category { Id = 15, ParentId = 3, Name = "Speakers" },
                new Category { Id = 16, ParentId = 4, Name = "Phones" },
                new Category { Id = 17, ParentId = 5, Name = "Consoles" },
                new Category { Id = 18, ParentId = 5, Name = "Video Games" },
                new Category { Id = 19, ParentId = 6, Name = "Coffee Machines" },
                new Category { Id = 20, ParentId = 7, Name = "Car Equipment" }
            );
        }
        public DbSet<ElectronicsStore.Models.StoreItem> StoreItem { get; set; }
        public DbSet<ElectronicsStore.Models.Category> Category { get; set; }
        public DbSet<ElectronicsStore.Models.Order> Order { get; set; }
    }
}
