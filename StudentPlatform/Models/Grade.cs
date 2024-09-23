using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public decimal Score { get; set; }

    public int? AssignmentId { get; set; }

    public int? StudentId { get; set; }

    public virtual Assignment Assignment { get; set; }

    public virtual Student Student { get; set; }
}