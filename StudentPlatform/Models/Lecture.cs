using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Lecture
{
    public int LectureId { get; set; }

    public string Title { get; set; }

    public string MaterialUrl { get; set; }

    public DateTime? Date { get; set; }

    public int? CourseId { get; set; }

    public virtual Course Course { get; set; }
}