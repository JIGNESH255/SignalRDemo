using Microsoft.AspNetCore.Mvc;

namespace SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }
    }
}