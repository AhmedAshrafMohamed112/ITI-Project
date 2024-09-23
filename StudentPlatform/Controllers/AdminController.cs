using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPlatform.Models;

namespace StudentPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly PlatFormDBContext context;

        public AdminController()
        {
            this.context = new();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Students()
        {
            var students =context.Students.ToList();
            return View(students);
        }

        public IActionResult Courses()
        {
            var courses = context.Courses.ToList();
            return View(courses);
        }

        public IActionResult Lectures()
        {
            var lectures = context.Lectures.ToList();
            return View(lectures);
        }

        public IActionResult Assignments()
        {
            var assignments = context.Assignments.ToList();
            return View(assignments);
        }

        public IActionResult Attendance()
        {
            var attendanceRecords = context.Attendances.ToList();
            return View(attendanceRecords);
        }
        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(student);
                context.SaveChanges();
                return RedirectToAction(nameof(Students));
            }
            return View(student);
        }

        public IActionResult EnrollStudents()
        {
            var students = context.Students.ToList();
            var courses = context.Courses.ToList();

            ViewBag.Students = students;
            ViewBag.Courses = courses;

            return View();
        }
        public IActionResult EnrollStudent(int studentId)
        {
            var student = context.Students.FirstOrDefault(s => s.StudentId == studentId);
            var courses = context.Courses.ToList();

            if (student == null)
            {
                TempData["Message"] = "Student not found.";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Courses = courses;
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnrollStudent(Student student, int[] courseIds)
        {
            var studentFromDb = context.Students.Include(s => s.Courses).FirstOrDefault(s => s.StudentId == student.StudentId);

            if (studentFromDb == null)
            {
                TempData["Message"] = "Student not found.";
                return RedirectToAction("Dashboard");
            }

            var courses = context.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();

            foreach (var course in courses)
            {
                if (!studentFromDb.Courses.Contains(course))
                {
                    studentFromDb.Courses.Add(course);
                }
            }

            context.SaveChanges();
            TempData["Message"] = "Student enrolled successfully.";
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        public IActionResult EnrollStudentCourses(int studentId, string firstName, string academicNumber, string nationalId, int[] courseIds)
        {            var student = context.Students.Include(s => s.Courses).FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                TempData["Message"] = "Student not found.";
                return RedirectToAction("EnrollStudents");
            }

            student.FirstName = firstName;
            student.AcademicNumber = academicNumber;
            student.IdOrPassportNumber = nationalId;

            var courses = context.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            foreach (var course in courses)
            {
                if (!student.Courses.Contains(course))
                {
                    student.Courses.Add(course);
                }
            }

            context.SaveChanges();
            TempData["Message"] = "Student enrolled successfully.";
            return RedirectToAction("EnrollStudents");
        }

    }
}
