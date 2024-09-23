using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;
using System.Linq;

namespace StudentPlatform.Controllers
{
    public class GradesController : Controller
    {
        private readonly PlatFormDBContext context;

        public GradesController()
        {
            context = new();
        }

        public IActionResult ViewGrades()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Index", "Home"); 
            }

            var grades = context.Grades
                .Include(g => g.Assignment) 
                .ThenInclude(a => a.Course) 
                .Where(g => g.StudentId == studentId) 
                .Select(g => new
                {
                    g.GradeId,
                    g.Assignment.Title,
                    CourseName = g.Assignment.Course.CourseName,
                    g.Score,
                    g.Assignment.Description 
                })
                .ToList(); 

            return View(grades);
        }
    }
}
