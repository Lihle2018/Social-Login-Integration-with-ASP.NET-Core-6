using Microsoft.AspNetCore.Mvc;

namespace Online_Election_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
