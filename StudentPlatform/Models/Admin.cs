using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string AdminName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}