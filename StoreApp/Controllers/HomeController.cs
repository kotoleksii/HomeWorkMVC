using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Controllers
{
    public class HomeController : Controller
    {
        StoreContext db;

        private readonly IConfiguration Configuration;
        public string AppName { get; set; }
        public string FromValueText { get; set; }
        public string ToValueText { get; set; }
        public HomeController(StoreContext context, IConfiguration _configuration)
        {
            db = context;
            Configuration = _configuration;

            if (db.Items.FirstOrDefault() != null)
            {
                FromValueText = db.Items.Min(p => p.Price).ToString();
                ToValueText = db.Items.Max(p => p.Price).ToString();
            }
        }

        public IActionResult Index()
        {
            ViewBag.AppName = Configuration.GetSection("AppSettings")["AppName"];

            return View();
        }

        [HttpPost]
        public IActionResult IncrementCount()
        {
            return View(db.Items.OrderBy(p => p.Price).ToList());
        }

        [HttpGet]
        public IActionResult Goods()
        {
            ViewBag.FromValueText = FromValueText;
            ViewBag.ToValueText = ToValueText;

            return View(db.Items.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) RedirectToAction("Index");

            Item item = null;

            try
            {
                item = await db.Items.FindAsync(id);
            }
            catch { }

            return View("Details", item);
        }

        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.ItemId = id;
            ViewBag.ItemTitle = GetItemTitleByID(id);

            return View();
        }

        [HttpPost]
        public IActionResult Buy(Order order)
        {
            db.Orders.Add(order);

            db.SaveChanges();

            var itemTitle = GetItemTitleByID(order.ItemId);

            TempData["message"] = $"Thanks {order.FullName} for purchasing {itemTitle}!";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private string GetItemTitleByID(int? id)
        {
            return db.Items.Find(id).Title;
        }
    }
}
