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
        public DbSet<ElectronicsStore.Models.StoreItem> StoreItem { get; set; }
    }
}
