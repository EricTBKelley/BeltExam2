using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions; // for regex

namespace BeltExam2.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Must include a name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must include an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must include a password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Password must contain at least one uppercase, one lowercase, and one number.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]

        public string PasswordConfirm { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        
        public string Description {get;set;}

    }
}