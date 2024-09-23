
using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; }

    public string Instructor { get; set; }

    public DateTime? Schedule { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin Admin { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}