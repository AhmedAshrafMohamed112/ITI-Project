using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public DateTime Date { get; set; }

    public bool IsPresent { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin Admin { get; set; }

    public virtual Course Course { get; set; }

    public virtual Student Student { get; set; }
}