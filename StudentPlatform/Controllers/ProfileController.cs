using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using StudentPlatform.Models;
using System.Linq;

public class ProfileController : Controller
{
    private readonly PlatFormDBContext context;

    public ProfileController()
    {
        context = new();
    }

    public IActionResult ViewProfile()
    {
        var studentId = HttpContext.Session.GetInt32("StudentId");

        if (studentId == null)
        {
            return Unauthorized();
        }

        var student = context.Students
                              .FirstOrDefault(s => s.StudentId == studentId);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        var studentId = HttpContext.Session.GetInt32("StudentId");

        if (studentId == null)
        {
            return Unauthorized(); 
        }

        var student = context.Students.FirstOrDefault(s => s.StudentId == studentId);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    public IActionResult EditProfile(Student model)
    {
        if (ModelState.IsValid)
        {
            var student = context.Students.Find(model.StudentId);
            if (student != null)
            {
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Email = model.Email;
                student.Password = model.Password;
                student.AcademicNumber = model.AcademicNumber;
                student.DateOfBirth = model.DateOfBirth;
                student.PlaceOfBirth = model.PlaceOfBirth;
                student.Nationality = model.Nationality;
                student.Gender = model.Gender;
                student.Religion = model.Religion;
                context.SaveChanges();
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("EditProfile");
            }
        }
        return View(model);
    }
}
