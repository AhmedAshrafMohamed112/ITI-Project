using Microsoft.AspNetCore.Mvc;
using StudentPlatform.Models;
using System.Linq;

namespace StudentPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly PlatFormDBContext context;

        public AccountController()
        {
            this.context = new ();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RejStudentVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RejStudentVM student)
        {
            if (ModelState.IsValid)
            {
                var Found_AcademicNumber = context.Students.FirstOrDefault(s => s.AcademicNumber == student.AcademicNumber);

                if (Found_AcademicNumber != null)
                {                    
                    ModelState.AddModelError("AcademicNumber", "This academic number is already registered !");
                    return View(student); 
                }
                var Found_Email = context.Students.FirstOrDefault(s => s.Email == student.Email);

                if (Found_Email != null)
                {
                    ModelState.AddModelError("Email", "This Email is already registered !");
                    return View(student);
                }
                Student newStudent = new Student()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    AcademicNumber = student.AcademicNumber,
                    Email = student.Email,
                    Password = student.Password,
                };

                context.Students.Add(newStudent);
                newStudent.StudentId = context.SaveChanges(); 

                return RedirectToAction("display","Welcome");
            }

            return View(student); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public IActionResult Login(LoginVM login)
        {
            if (login.Role == "student")
            {
                var student = context.Students
                    .Where(s => s.Email == login.Email && s.Password == login.Password)
                    .SingleOrDefault();

                if (student == null)
                {
                    ViewBag.error = "Invalid Student Email or Password!";
                    return View("Login");
                }
                string fullname=student.FirstName+" "+student.LastName;
                HttpContext.Session.SetString("StudentName", fullname);
                HttpContext.Session.SetInt32("StudentId", student.StudentId);
                return RedirectToAction("Dashboard", "student");
            }
            else if (login.Role == "admin")
            {
                var admin = context.Admins
                    .Where(a => a.Email == login.Email && a.Password == login.Password)
                    .SingleOrDefault();

                if (admin == null)
                {
                    ViewBag.error = "Invalid Admin Email or Password!";
                    return View("Login");
                }

                HttpContext.Session.SetString("AdminName", admin.AdminName);
                HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                return RedirectToAction("Dashboard", "Admin"); 
            }

            ViewBag.error = "Please select a role.";
            return View("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



    }
}
