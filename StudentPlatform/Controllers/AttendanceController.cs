using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;
using System.Linq;

public class AttendanceController : Controller
{
    private readonly PlatFormDBContext context;

    public AttendanceController()
    {
        context = new();
    }

    public IActionResult Index()
    {
        var attendances = context.Attendances
            .Include(a => a.Admin)
            .Include(a => a.Course)
            .Include(a => a.Student)
            .ToList(); 

        return View(attendances);
    }

    public IActionResult Details(int id)
    {
        var attendance = context.Attendances
            .Include(a => a.Admin)
            .Include(a => a.Course)
            .Include(a => a.Student)
            .FirstOrDefault(m => m.AttendanceId == id);

        if (attendance == null)
        {
            return NotFound();
        }

        return View(attendance);
    }

    public IActionResult Create()
    {
        ViewBag.StudentId = new SelectList(context.Students, "StudentId", "FirstName");
        ViewBag.CourseId = new SelectList(context.Courses, "CourseId", "CourseName");
        ViewBag.AdminId = new SelectList(context.Admins, "AdminId", "AdminName");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            TempData["Message"] = "Attendance Added Successfully.";
            context.Attendances.Add(attendance);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.StudentId = new SelectList(context.Students, "StudentId", "FirstName");
        ViewBag.CourseId = new SelectList(context.Courses, "CourseId", "CourseName");
        ViewBag.AdminId = new SelectList(context.Admins, "AdminId", "AdminName");

        return View(attendance);
    }

    public IActionResult Edit(int id)
    {
        var attendance = context.Attendances.Find(id);
        if (attendance == null)
        {
            return NotFound();
        }

        ViewBag.Courses = new SelectList(context.Courses, "CourseId", "CourseName", attendance.CourseId);
        ViewBag.Students = new SelectList(context.Students, "StudentId", "FirstName", attendance.StudentId);
        ViewBag.Admins = new SelectList(context.Admins, "AdminId", "AdminName", attendance.AdminId);

        return View(attendance);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            context.Update(attendance);
            context.SaveChanges();
            TempData["Message"] = "Attendance Updated Successfully.";

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Courses = new SelectList(context.Courses, "CourseId", "CourseName", attendance.CourseId);
        ViewBag.Students = new SelectList(context.Students, "StudentId", "FirstName", attendance.StudentId);
        ViewBag.Admins = new SelectList(context.Admins, "AdminId", "Name", attendance.AdminId);

        return View(attendance);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var attendance = context.Attendances.SingleOrDefault(u => u.AttendanceId == id);
        if (attendance == null)
        {
            return Content("Not Valid Id !");
        }
        return View(attendance);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var attendance = context.Attendances.Find(id);
        if (attendance == null)
        {
            return Content("Not Valid Id !");
        }

        context.Attendances.Remove(attendance);
        context.SaveChanges(); 
        TempData["Message"] = "Attendance Deleted Successfully.";

        return RedirectToAction(nameof(Index));
    }
    private bool AttendanceExists(int id)
    {
        return context.Attendances.Any(e => e.AttendanceId == id);
    }
    public IActionResult ViewAttendance()
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

        var attendance = context.Attendances
            .Where(a => a.CourseId.HasValue && enrolledCourseIds.Contains(a.CourseId.Value)) 
            .Include(a => a.Course) 
            .Select(a => new
            {
                a.AttendanceId,
                a.CourseId,
                a.Student,
                a.IsPresent,
                CourseName = a.Course.CourseName,Date=a.Date

            })
            .ToList(); 

        return View(attendance);
    }

}
