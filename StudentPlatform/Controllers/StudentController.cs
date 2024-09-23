using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;
using System.Linq;

namespace StudentPlatform.Controllers
{
    public class StudentController : Controller
    {
        private readonly PlatFormDBContext context;

        public StudentController()
        {
            this.context = new();
        }
        public IActionResult Details(int id)
        {
            var student = context.Students
                .Include(s => s.Attendances)
                .Include(s => s.Grades)
                .Include(s => s.Submissions)
                .Include(s => s.Courses)
                .FirstOrDefault(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Index()
        {
            var students = context.Students.Include(s => s.Attendances).ToList();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(student);
                context.SaveChanges();
                TempData["Message"] = "Student Added Successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }


        public IActionResult Edit(int id)
        {
            var student = context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                context.Students.Update(student);
                context.SaveChanges();
                TempData["Message"] = "Student Updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = context.Students.SingleOrDefault(u => u.StudentId == id);
            if (student == null)
            {
                return Content("Not Valid Id !");
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = context.Students.Find(id);
            if (student == null)
            {
                return Content("Not Valid Id !");
            }

            context.Students.Remove(student);
            context.SaveChanges(); 
            TempData["Message"] = "Student Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dashboard()
        {
            return View();  
        }
        public IActionResult EnrolledCourses(int studentId)
        {
            var enrolledCourses = context.Students
                .Where(s => s.StudentId == studentId)
                .SelectMany(s => s.Courses)  
                .ToList();

            return View(enrolledCourses);
        }





    }
}
