using Microsoft.AspNetCore.Mvc;

namespace HomeWorkMVC.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        { }

        public IActionResult Index()
        {
            string OSVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ViewData["OSVersion"] = OSVersion;

            return View();
        }
    }
}
