using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using LibraryLove.Model;
using LibraryLove;
using System.Security.Cryptography;
using System.Text;

namespace LibraryLove.Pages.Browser
{
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        public CreateMember record { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost(CreateMember record)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Member (Username, FirstName, LastName, Email, Password, Role, SecurityQuestion, SecurityAnswer) VALUES (@User, @FName, @LName, @UEmail, @Pass, @URole, @SQ, @SA)";


                // Add book to database
                command.Parameters.AddWithValue("@User", record.Username);
                command.Parameters.AddWithValue("@FName", record.FirstName);
                command.Parameters.AddWithValue("@LName", record.LastName);
                command.Parameters.AddWithValue("@UEmail", record.Email);


                // Encypt the Password

                // Chand, M. (2020, April 16). Compute SHA256 Hash In C#. Retrieved from c-sharpcorner: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
                string HashedPassword = "";

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Get a Byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(record.Password));

                    // Convert Byte array to string 
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    HashedPassword = builder.ToString();

                }
                command.Parameters.AddWithValue("@Pass", HashedPassword);
                command.Parameters.AddWithValue("@URole", "Customer");
                command.Parameters.AddWithValue("@S1", record.SecurityQuestion);
                command.Parameters.AddWithValue("@S1", record.SecurityAnswer);



                command.ExecuteNonQuery();
            }

            return RedirectToPage("Login");
        }

        
    }
}
