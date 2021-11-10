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
            var item = await _context.StoreItem.Include(s=> s.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();

            var relatedItems = await _context.StoreItem.Where(s => s.Category.Id == item.Category.Id).Where(s => s.Id != item.Id).Take(4).ToListAsync();

            StoreItemInfoViewModel itemViewModel = new();

            itemViewModel.StoreItem = item;
            if (relatedItems != null)
                itemViewModel.RelatedStoreItems = relatedItems;
            return View(itemViewModel);
        }

        public async Task<IActionResult> ItemByCategoryList(int id)
        {
            var viewModel = new StoreItemsByCategoryViewModel();
            var itemList = await _context.StoreItem.Where(s => s.Category.Id == id).Include(c => c.Category).ToListAsync();
            viewModel.StoreItems = itemList;
            var category = await _context.Category.Where(c => c.Id == id).FirstOrDefaultAsync();
            viewModel.CategoryName = category.Name;
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(int itemId, int quantity)
        {
            var user = await _userManager.Users.Include(u => u.Cart).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(quantity == 0)
                throw(new Exception());
            var cartItem = new CartItem();
            var item = await _context.StoreItem.FindAsync(itemId);
            cartItem.StoreItem = item;
            cartItem.Quantity = quantity;
            cartItem.DateCreated = DateTime.UtcNow;
            if (user.Cart == null)
                user.Cart = new Cart();
            user.Cart.CartItems.Add(cartItem);

            int totalSum = 0;
            foreach(var cItem in user.Cart.CartItems)
            {
                totalSum += cItem.StoreItem.Price * cItem.Quantity;
            }
            user.Cart.Total = totalSum;
           // cartItem.ShoppingUserId = userId;
          //  if (user.CartItems == null)
    //            user.CartItems = new List<CartItem>();
       //     user.CartItems.Add(cartItem);
            //_context.Update(cartItem);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart");
        }

        [Authorize]
        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s=>s.CartItems).ThenInclude(i=>i.StoreItem)
                .SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user.Cart == null)
                user.Cart = new Cart();
            var cart = user.Cart;
            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s => s.CartItems).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItem = user.Cart.CartItems.Where(m => m.Id == id).Single();
            _context.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart");
        }

        public async Task<IActionResult> ChangeOrderQuantity(int amount, int itemId)
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(k => k.StoreItem).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItem = user.Cart.CartItems.Where(m => m.Id == itemId).Single();
            
            cartItem.Quantity += amount;

            user.Cart.Total += amount * cartItem.StoreItem.Price;

            _context.Update(cartItem);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart");
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(i => i.StoreItem)
    .SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user.Cart == null)
                user.Cart = new Cart();
            var cart = user.Cart;
            CheckoutViewModel viewModel = new();
            viewModel.Cart = cart;
            return View(viewModel);
        }
        public async Task<IActionResult> ConfirmCheckout(CheckoutViewModel checkoutViewModel)
        {
             var user = await _userManager.Users.Include(u => u.Cart).ThenInclude(s => s.CartItems).ThenInclude(cartItem => cartItem.StoreItem).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            // var cart = user.Cart;
          //  var user = await _userManager.Users.Include(u => u.Cart).SingleOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = user.Cart;
            List<OrderItem> orderItems = new();
            foreach(var cItem in cart.CartItems)
            {
                orderItems.Add(new OrderItem
                {
                    DateCreated = DateTime.Now,
                    Quantity = cItem.Quantity,
                    StoreItem = cItem.StoreItem,
                    Price = cItem.StoreItem.Price
                });
            }

            Order order = new()
            {
                OrderItems = orderItems,
                ShippingAddress = checkoutViewModel.ShippingAddress,
                //ShoppingUserId = cart.ShoppingUserId,
                ShoppingUser = cart.ShoppingUser,
                Total = cart.Total,
                OrderStatus = OrderStatusEnum.Accepted
            };

            _context.Remove(user.Cart);
            user.Cart = new Cart();
            user.Orders.Add(order);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart");
        }

    }
}
