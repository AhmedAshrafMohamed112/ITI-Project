using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;
using System.Linq;

public class AssignmentController : Controller
{
    private readonly PlatFormDBContext context;

    public AssignmentController()
    {
        context = new();
    }

    public IActionResult Index()
    {
        var assignments = context.Assignments
            .Include(a => a.Course)
            .Include(b => b.Admin)
            .ToList();
        return View(assignments);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return Content("NO Found ID !");
        }

        var assignment = context.Assignments
       .Include(a => a.Course)
       .Include(a => a.Admin)
       .Include(a => a.Grades) 
           .ThenInclude(g => g.Student) 
       .Include(a => a.Submissions) 
           .ThenInclude(s => s.Student) 
       .FirstOrDefault(m => m.AssignmentId == id);

        if (assignment == null)
        {
            return Content("NO Found !");
        }

        return View(assignment);

        if (assignment == null)
        {
            return Content("NO Found !");
        }

        return View(assignment);
    }

    public IActionResult Create()
    {
        ViewBag.Courses = context.Courses.ToList();
        ViewBag.Admins = context.Admins.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Assignment assignment)
    {
        if (ModelState.IsValid)
        {
            TempData["Message"] = "Assignment Added Successfully.";

            context.Assignments.Add(assignment);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(assignment);
    }

    public IActionResult Edit(int id)
    {
        var assignment = context.Assignments.Find(id);
        if (assignment == null)
        {
            return NotFound();
        }

        ViewBag.CourseId = new SelectList(context.Courses, "CourseId", "CourseName");

        ViewBag.AdminId = new SelectList(context.Admins, "AdminId", "AdminName");
        return View(assignment);
    }

    [HttpPost]
    public IActionResult Edit(Assignment assignment)
    {
        if (ModelState.IsValid)
        {
            context.Update(assignment);
            context.SaveChanges();
            TempData["Message"] = "Assignment Updated Successfully.";

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CourseId = new SelectList(context.Courses, "CourseId", "CourseName", assignment.CourseId);
        ViewBag.AdminId = new SelectList(context.Admins, "AdminId", "AdminName", assignment.AdminId);

        return View(assignment);
    }


    [HttpGet]
    public IActionResult Delete(int id)
    {
        var assignment = context.Assignments.SingleOrDefault(u => u.AssignmentId == id);
        if (assignment == null)
        {
            return Content("Not Valid Id !");
        }
        return View(assignment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var assignment = context.Assignments.Find(id);
        if (assignment == null)
        {
            return Content("Not Valid Id !");
        }

        context.Assignments.Remove(assignment);
        context.SaveChanges(); 
        TempData["Message"] = "Assignment Deleted Successfully.";

        return RedirectToAction(nameof(Index));
    }

    private bool AssignmentExists(int id)
    {
        return context.Assignments.Any(e => e.AssignmentId == id);
    }
    public IActionResult ViewAssignments()
    {
        var studentId = HttpContext.Session.GetInt32("StudentId");

        if (studentId == null)
        {
            return RedirectToAction("Index", "Home"); 
        }

        var student = context.Students
            .Include(s => s.Courses) 
            .FirstOrDefault(s => s.StudentId == studentId);

        if (student == null)
        {
            return RedirectToAction("Index", "Home"); 
        }

        var enrolledCourseIds = student.Courses.Select(c => c.CourseId).ToList();

        var assignments = context.Assignments
            .Where(a => a.CourseId.HasValue && enrolledCourseIds.Contains(a.CourseId.Value)) 
            .Include(a => a.Course) 
            .Select(a => new
            {
                a.AssignmentId,
                a.Title,
                a.DueDate,
                a.Description,
                CourseName = a.Course.CourseName,
                IsSubmitted = a.Submissions.Any(s => s.StudentId == studentId) 
            })
            .ToList(); 

        return View(assignments);
    }

}
