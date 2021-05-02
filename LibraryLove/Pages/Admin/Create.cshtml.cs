using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Admin
{
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public AdminCreate record { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Connect to Database
                DBConnection dbstring = new DBConnection();
                string DbConnection = dbstring.DbString();
                SqlConnection conn = new SqlConnection(DbConnection);
                conn.Open();


                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"INSERT INTO Member (Username, FirstName, LastName, Email, Password, Role) VALUES (@User, @FName, @LName, @UEmail, @Pass, @URole)";


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


                    string role = record.MemberRole.ToString();
                    command.Parameters.AddWithValue("@URole", role);


                    command.ExecuteNonQuery();
                }

                // Depending on the creation of an employee depends on the page they are shown
                return RedirectToPage("AdminIndex");
            }

            return Page();

        }
    }
}

