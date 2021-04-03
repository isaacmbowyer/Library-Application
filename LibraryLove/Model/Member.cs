using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class Member : IValidatableObject
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
                command.Connection = conn;
                Int32 count;


                // Check if the user changed their Username
                command.CommandText = @"SELECT Username FROM Member WHERE Id = @UId";
                command.Parameters.AddWithValue("@UId", Id);

                SqlDataReader reader = command.ExecuteReader();

                // Get the Username from database
                string databaseUsername = "";
                while (reader.Read())
                {
                    databaseUsername = reader.GetString(0);
                }

                reader.Close();

                // Check if the Username is the same or not
                if (!databaseUsername.Equals(Username))
                {
                    // Check Username is Unique 
                    command.CommandText = @"SELECT COUNT(Username) FROM Member WHERE Username = @User";
                    command.Parameters.AddWithValue("@User", Username);

                    count = (Int32)command.ExecuteScalar();

                    if (count == 1)
                    {
                        // Username exists show error
                        yield return new ValidationResult("This Username already exists", new[] { "Username" });
                    }


                }

                // Check if User changed the Email
                command.CommandText = @"SELECT Email FROM Member WHERE Id = @UId";
                reader = command.ExecuteReader(); // read records 

                string databaseEmail = "";
                while (reader.Read())
                {
                    databaseEmail = reader.GetString(0);
                }

                reader.Close();


                if (!databaseEmail.Equals(Email))
                {
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
