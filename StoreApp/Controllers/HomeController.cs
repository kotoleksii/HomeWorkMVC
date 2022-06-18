using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApp.Models;
using System;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            db.Items.Add(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Goods");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                db.ChangeTracker.Clear();
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            var entry = db.Items.First(e => e.Id == item.Id);

            db.Entry(entry).CurrentValues.SetValues(item);
            db.SaveChangesAsync();
            return RedirectToAction("Goods");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                {
                    db.Items.Remove(item);
                    await db.SaveChangesAsync();


                    return RedirectToAction("Goods");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult IncrementCount()
        {
            return View(db.Items.OrderBy(p => p.Price).ToList());
        }

        public async Task<IActionResult> Goods(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "Id" : "";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            int pageSize = 3;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var items = from i in db.Items
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Title.Contains(searchString)
                                       || i.Description.Contains(searchString)
                                       || i.Id.ToString().Contains(searchString)
                                       || i.Price.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Id":
                    items = items.OrderBy(s => s.Id);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                case "Title":
                    items = items.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    items = items.OrderByDescending(s => s.Title);
                    break;
                default:
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }

            return View(await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize));
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

            ViewData["ItemId"] = id;
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

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            db.Users.Add(user);

            db.SaveChanges();

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
