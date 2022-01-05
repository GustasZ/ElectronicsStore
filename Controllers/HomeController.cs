using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ShoppingUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ShoppingUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var topItems = _context.StoreItem.ToList().Take(8);
            return View(topItems);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(cart => cart.CartItems).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return PartialView("_CartPartial", new Cart());
            if (user.Cart == null)
                user.Cart = new Cart();

            return PartialView("_CartPartial", user.Cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
                List<CategoryViewModel> viewModels = new();
                var categories = await _context.Category.Where(c => c.ParentId == 0).ToListAsync();
                foreach (var category in categories)
                    viewModels.Add(new CategoryViewModel {TopCategory = category });
                foreach (var topCategory in viewModels)
                {
                    topCategory.AddSubCategories(_context.Category.Where(c => c.ParentId == topCategory.TopCategory.Id).ToList());
                foreach (var subCategory in topCategory.SubCategories)
                {
                    topCategory.AddSubSubCategories(_context.Category.Where(c => c.ParentId == subCategory.Id).ToList());
                }



                /*           foreach (var sCategory in topCategory.Categories)
                           {
                               topCategory.AddSubCategories(_context.Category.Where(c => c.ParentId == sCategory.Id).ToList());
                           }*/
            }

            //    var categories = await _context.Category.ToListAsync();

            return PartialView("_CategoryPartial", viewModels);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
