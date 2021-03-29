using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class Member
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(16, ErrorMessage = "Username must be between 3 and 16 characters", MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)] // make sure we cant read the password
        public string Password { get; set; }

        [Required(ErrorMessage = "Select a Role from the drop down list")]
        public int Role { get; set; }
    }
}
