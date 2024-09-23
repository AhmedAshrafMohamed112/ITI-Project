using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Submission
{
    public int SubmissionId { get; set; }

    public int? AssignmentId { get; set; }

    public int? StudentId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string FileUrl { get; set; }

    public virtual Assignment Assignment { get; set; }

    public virtual Student Student { get; set; }
}