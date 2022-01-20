using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MyOrders()
        {
            var Orders = _context.Order.Include(o => o.ShoppingUser).Where(order => order.ShoppingUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await Orders.ToListAsync());
        }
    }
}
