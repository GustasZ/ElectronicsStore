using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ShoppingUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                dbContext.Database.Migrate();

                var adminRole = new IdentityRole("Admin");
                var modRole = new IdentityRole("Moderator");

                if (!dbContext.Roles.Any())
                {
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(modRole).GetAwaiter().GetResult();
                }

                if (!dbContext.Users.Any(u => u.UserName == "admin"))
                {
                    var adminUser = new ShoppingUser // can I use just identityuser here?
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com"
                    };
                    var result = userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
