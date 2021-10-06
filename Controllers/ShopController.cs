using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ShoppingUser> _userManager;

        public ShopController(ApplicationDbContext context, UserManager<ShoppingUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Item(int? id)
        {
            if (id == null)
                return NotFound();
            var item = await _context.StoreItem.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(int itemId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var cartItem = new CartItem();
            var item = await _context.StoreItem.FirstOrDefaultAsync(m => m.Id == itemId);
            cartItem.StoreItem = item;
            cartItem.Quantity = quantity;
            cartItem.DateCreated = DateTime.UtcNow;
            if (user.Cart == null)
                user.Cart = new Cart();
            if (user.Cart.CartItems == null)
                user.Cart.CartItems = new List<CartItem>();
            user.Cart.CartItems.Add(cartItem);
           // cartItem.ShoppingUserId = userId;
          //  if (user.CartItems == null)
    //            user.CartItems = new List<CartItem>();
       //     user.CartItems.Add(cartItem);
            //_context.Update(cartItem);
            _context.Update(user);
            _context.SaveChanges();
            return View(RedirectToAction("Item",itemId));
        }

        [Authorize]
        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s=>s.CartItems).ThenInclude(i=>i.StoreItem).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = user.Cart;
            var cartItems = cart.CartItems;
            if (cartItems == null)
                cartItems = new List<CartItem>();
            return View(cartItems);
        }

    }
}
