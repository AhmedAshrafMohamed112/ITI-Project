using System;
using System.Collections.Generic;

namespace StudentPlatform.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string AcademicNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string IdOrPassportNumber { get; set; }

    public string Nationality { get; set; }

    public string PlaceOfBirth { get; set; }

    public string Religion { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public override string ToString()
    {
        return $"Student: {FirstName} {LastName}, " +
               $"Academic Number: {AcademicNumber}, " +
               $"Email: {Email}, " +
               $"Password: {Password}, " +
               $"Date of Birth: {DateOfBirth?.ToString("yyyy-MM-dd") ?? "N/A"}, " +
               $"Gender: {Gender}, " +
               $"ID/Passport Number: {IdOrPassportNumber}, " +
               $"Nationality: {Nationality}, " +
               $"Place of Birth: {PlaceOfBirth}, " +
               $"Religion: {Religion}";
    }

}