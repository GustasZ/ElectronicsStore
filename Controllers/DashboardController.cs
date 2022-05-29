using ElectronicsStore.Data;
using ElectronicsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> OrderStats()
        {
            var allOrders = await _context.Order.ToListAsync();
            var chartLabel = "Orders";
            int lines = 10;

            int[] data = new int[lines];
            string[] labels = new string[lines];

            DateTime todayStart = DateTime.Today.AddDays(-lines + 1);
            for (int i = 0; i < lines; i++)
            {
                var dayOrders = allOrders.Where(x => x.CreatedDate.Date == todayStart.AddDays(i).Date).ToList();
                data[i] = dayOrders.Count();
                labels[i] = todayStart.AddDays(i).Date.ToString("M");
            }

          return new JsonResult(new { myData = data, myLabels = labels, myChartLabel = chartLabel });
        }

        public ActionResult Users()
        {
            var users = _context.Users;
            return View();
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
