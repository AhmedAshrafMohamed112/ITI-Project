using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public string Title { get; set; }

    public DateTime DueDate { get; set; }

    public string Description { get; set; }

    public int? CourseId { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin Admin { get; set; }

    public virtual Course Course { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}