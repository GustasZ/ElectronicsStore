using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ShoppingUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ShoppingUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            //_context.Users;
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                //.Include(o => o.G) roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);

            UserViewModel model = new UserViewModel { ShoppingUser = user , CurrentRoles = (System.Collections.Generic.List<string>)roles };
            return View(model);
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = _context.Roles.Select(x => x.Name).ToList();
            var currRoles = await _userManager.GetRolesAsync(user);

            UserViewModel model = new UserViewModel { ShoppingUser = user, CurrentRoles = currRoles, AvailableRoles = allRoles};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(string id, UserViewModel userVW)
        {
            if (id != userVW.ShoppingUser.Id)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if(userVW.CurrentRoles != null)
            await _userManager.AddToRolesAsync(user,userVW.CurrentRoles);

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
