using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class CreateMember : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Select a Role")]
        [Display(Name = "Role")]
        public UserRole? MemberRole { get; set; }

        public string Role { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Select a Security Question")]
        [Display(Name = "Security Question")]
        public Security? SecurityQuestion { get; set; }

        [Required(ErrorMessage = "Answer to Security Question is required")]
        [Display(Name = "Secuirty Answer")]
        public string SecurityAnswer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                if(Id == 0)
                {
                    // Create Page
                    command.Connection = conn;
                    Int32 count;


                    // Check Username is Unique 
                    command.CommandText = @"SELECT COUNT(Username) FROM Member WHERE Username = @User";
                    command.Parameters.AddWithValue("@User", Username);

                    count = (Int32)command.ExecuteScalar();

                    if (count == 1)
                    {
                        // Username exists show error
                        yield return new ValidationResult("This Username already exists", new[] { "Username" });
                    }

                    // Check if the Email is unique 
                    command.CommandText = @"SELECT COUNT(Email) FROM Member WHERE Email = @UEmail";
                    command.Parameters.AddWithValue("@UEmail", Email);

                    count = (Int32)command.ExecuteScalar();

                    if (count == 1)
                    {
                        yield return new ValidationResult("This Email already exists", new[] { "Email" });
                    }

                }


            }

        }
    }
    
}
