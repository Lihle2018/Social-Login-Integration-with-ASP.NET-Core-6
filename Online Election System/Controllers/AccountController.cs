using Microsoft.AspNetCore.Mvc;
using Online_Election_System.Models.ViewModels;

namespace Online_Election_System.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model) 
        {
            return View(model);
        }
        
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Register(RegisterViewModel model) 
        { 
            return View(model);
        }
    }
}
