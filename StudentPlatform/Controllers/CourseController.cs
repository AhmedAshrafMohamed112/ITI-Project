using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;

namespace StudentPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly PlatFormDBContext context;

        public CourseController()
        {
            this.context = new();
        }
        public IActionResult Details(int id)
        {
            var course = context.Courses
                .Include(c => c.Assignments)
                .Include(c => c.Attendances)
                .Include(c => c.Lectures)
                .Include(c => c.Students)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Index()
        {
            var courses = context.Courses.ToList(); 
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Course Added Successfully.";

                context.Courses.Add(course);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                context.Courses.Update(course);
                context.SaveChanges();
                TempData["Message"] = "Course Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View(course);
        }

       
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = context.Courses.SingleOrDefault(u => u.CourseId == id);
            if (course == null)
            {
                return Content("Not Valid Id !");
            }
            return View(course);
        }


      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = context.Courses.SingleOrDefault(u => u.CourseId == id);
            if (course == null)
            {
                return Content("Not Valid Id !");
            }

            context.Courses.Remove(course);
            context.SaveChanges(); 
            TempData["Message"] = "Course Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ViewCourses()
        {
           
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return Unauthorized(); 
            }

            var courses = context.Students
                .Where(s => s.StudentId == studentId)
                .SelectMany(s => s.Courses) 
                .ToList();

            return View(courses);
        }
    }
}
