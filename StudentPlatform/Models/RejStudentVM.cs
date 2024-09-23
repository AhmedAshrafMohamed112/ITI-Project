using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;
namespace StudentPlatform.Models
{
    public class RejStudentVM
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        [Required(ErrorMessage = "Name is required.")]
        public string FirstName { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        [Required(ErrorMessage = "Name is required.")]
        public string LastName { get; set; }
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Name must be between 8 and 30 characters.")]
        [Required(ErrorMessage = "Name is required.")]
        public string AcademicNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 60 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$",
            ErrorMessage = "Password must have at least one (uppercase and lowercase) letter, one number, and one special character !")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }



    }
}
