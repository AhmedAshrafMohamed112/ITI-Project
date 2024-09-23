using Microsoft.AspNetCore.Mvc;
using StudentPlatform.Models;

namespace StudentPlatform.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly PlatFormDBContext context;

        public WelcomeController()
        {
            this.context = new();
        }
        public IActionResult Index()
        {
        
            return View();
        }
        public IActionResult display()
        {
            var std=context.Students.ToList();
            return View(std);
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {

            if (ModelState.IsValid)
            {
                if (role == "student")
                {
                    return RedirectToAction("Index", "Student");
                }
                else if (role == "admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            ViewBag.ErrorMessage = "Invalid login attempt. Please try again.";
            return View("Index");
        }

        

    }
}
