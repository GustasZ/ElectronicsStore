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
            var item = await _context.StoreItem.Include(s=> s.Category).Include(k => k.MoreItemInfo).FirstOrDefaultAsync(x => x.Id == id);
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

            var mainCategory = await _context.Category.Where(c => c.Id == id).FirstOrDefaultAsync();
            var itemCategory = mainCategory;
            List<Category> categoryPath = new();
            categoryPath.Add(itemCategory);
            while(itemCategory.ParentId != 0)
            {
                itemCategory = await _context.Category.Where(c => c.Id == itemCategory.ParentId).FirstOrDefaultAsync();
                categoryPath.Add(itemCategory);
            }
            categoryPath.Reverse();
            viewModel.CategoryPath = categoryPath; // Parent category -> child category -> current category


            // get items from current category and any childs the category has

            var currentCategory = mainCategory;
            List<Category> categoryAllChildren = new();
            List<Category> categoryChildren = new();
            List<Category> categoryTier = new();
            List<Category> categoryTierTemp = new();

            categoryChildren.AddRange(await _context.Category.Where(c => c.ParentId == mainCategory.Id).ToListAsync());

            categoryTier.Add(currentCategory);
            while (categoryTier.Count != 0)
            {
                categoryAllChildren.AddRange(categoryTier);
                foreach(var category in categoryTier)
                {
                    categoryTierTemp.AddRange(await _context.Category.Where(c => c.ParentId == category.Id).ToListAsync());
                }
                categoryTier = categoryTierTemp;
                categoryTierTemp = new();
            }

            List<StoreItem> storeItems = new();
            foreach (var childCategory in categoryAllChildren)
            {
                storeItems.AddRange(await _context.StoreItem.Where(s => s.Category.Id == childCategory.Id).ToListAsync());
            }
            viewModel.StoreItems = storeItems;

            viewModel.ChildCategories = categoryChildren;

            if (categoryChildren.Count == 0)
                return View("ItemsByCategoryList", viewModel);
            else
            {
                var previewItems = viewModel.StoreItems.OrderBy(x => Guid.NewGuid()).Take(4).ToList(); // randomly take 4 items out of all child categories, to show them as a preview
                viewModel.StoreItems = previewItems;
                return View("ItemsByCategoryListPreview", viewModel);
            }
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
                OrderStatus = OrderStatusEnum.Accepted,
                CreatedDate = DateTime.Now
            };

            _context.Remove(user.Cart);
            user.Cart = new Cart();
            user.Orders.Add(order);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart");
        }

        public async Task<IActionResult> ItemsSearch(string name)
        {
            ViewData["searchString"] = name;
            return View(await _context.StoreItem.Where(x => x.Name.Contains(name)).ToListAsync());
        }

    }
}
