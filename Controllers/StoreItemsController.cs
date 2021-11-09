using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ElectronicsStore.Controllers
{
    public class StoreItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StoreItemsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: StoreItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoreItem.ToListAsync());
        }

        // GET: StoreItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // GET: StoreItems/Create
        public IActionResult Create()
        {
            var categories = _context.Category.Where(c => c.ParentId != 0).ToList();
            StoreItemViewModel viewModel = new StoreItemViewModel();
            List<SelectListItem> availableCategories = new List<SelectListItem>();
            availableCategories.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            List<SelectListItem> aCategories = categories.ConvertAll(a =>
           {
               return new SelectListItem()
               {
                   Text = a.Name.ToString(),
                   Value = a.Id.ToString()
                  // Selected = false
               };
           });
            availableCategories.AddRange(aCategories);
            viewModel.Categories = availableCategories;
            return View(viewModel);
        }

        // POST: StoreItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreItemViewModel storeItemVM)
        {
            if (ModelState.IsValid)
            {
                if (storeItemVM.FormFile.Length > 0)
                {
                    var fileName = GetUniqueFileName(storeItemVM.FormFile.FileName);
                    var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images", "itemImages");
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await storeItemVM.FormFile.CopyToAsync(stream);
                    }
                    storeItemVM.StoreItem.ImagePath = Path.Combine("images", "itemImages", fileName);
                }
                storeItemVM.StoreItem.Category = _context.Category.Where(c => c.Id == storeItemVM.SelectedCategory).Single();
                _context.Add(storeItemVM.StoreItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storeItemVM.StoreItem);
        }

        // GET: StoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItem.FindAsync(id);
            if (storeItem == null)
            {
                return NotFound();
            }
            StoreItemViewModel storeItemVM = new()
            {
                StoreItem = storeItem
            };
            return View(storeItemVM);
        }

        // POST: StoreItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  StoreItemViewModel storeItemVM)
        {
            if (id != storeItemVM.StoreItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (storeItemVM.FormFile.Length > 0)
                    {
                        var fileName = GetUniqueFileName(storeItemVM.FormFile.FileName);
                        var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images", "itemImages");
                        System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath,storeItemVM.StoreItem.ImagePath));
                        var filePath = Path.Combine(uploads, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await storeItemVM.FormFile.CopyToAsync(stream);
                        }
                        storeItemVM.StoreItem.ImagePath = Path.Combine("images", "itemImages", fileName);
                    }
                    _context.Update(storeItemVM.StoreItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreItemExists(storeItemVM.StoreItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(storeItemVM.StoreItem);
        }

        // GET: StoreItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.StoreItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // POST: StoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeItem = await _context.StoreItem.FindAsync(id);
            System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath,storeItem.ImagePath));
            _context.StoreItem.Remove(storeItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreItemExists(int id)
        {
            return _context.StoreItem.Any(e => e.Id == id);
        }

        static string GetUniqueFileName(string fileName)
        {
        return Guid.NewGuid().ToString().Substring(0,4) + Path.GetExtension(fileName);
        }
    }

}
