using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;
using System.Linq;

namespace StudentPlatform.Controllers
{
    public class LectureController : Controller
    {
        private readonly PlatFormDBContext context;

        public LectureController()
        {
            this.context = new();
        }

        public IActionResult Index()
        {
            var lectures = context.Lectures
                          .Include(l => l.Course)  
                          .ToList();
            return View(lectures);
        }
        public IActionResult Details(int id)
        {
            var lecture = context.Lectures
                .Include(l => l.Course) 
                .FirstOrDefault(m => m.LectureId == id);

            if (lecture == null)
            {
                return NotFound(); 
            }

            return View(lecture); 
        }
        
        public IActionResult Create()
        {
            ViewBag.Courses = context.Courses.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Lecture Added Successfully.";
                context.Lectures.Add(lecture);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Courses = context.Courses.ToList(); 
            return View(lecture);
        }

        public IActionResult Edit(int id)
        {
            var lecture = context.Lectures.Find(id);
            if (lecture == null)
            {
                return NotFound();
            }
            ViewBag.Courses = context.Courses.ToList(); 
            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lecture lecture)
        {
            if (id != lecture.LectureId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                context.Lectures.Update(lecture);
                context.SaveChanges();
                TempData["Message"] = "Lecture Updated Successfully.";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Courses = context.Courses.ToList(); 
            return View(lecture);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lec = context.Lectures.SingleOrDefault(u => u.LectureId == id);
            if (lec == null)
            {
                return Content("Not Valid Id !");
            }
            return View(lec);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var lec = context.Lectures.SingleOrDefault(u => u.LectureId == id);
            if (lec == null)
            {
                return Content("Not Valid Id !");
            }

            context.Lectures.Remove(lec);
            context.SaveChanges(); 
            TempData["Message"] = "Lecture Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ViewLectures()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Index", "Home"); 
            }

            var lectures = context.Lectures
                .Include(l => l.Course)
                .Where(l => l.Course.Students.Any(s => s.StudentId == studentId))
                .ToList();

            return View(lectures);
        }
    }
}
