using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Browser
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LogIn UserRecord { get; set; }

        public string Message { get; set; }
        public string SessionID;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Member WHERE Username = @UN AND Password = @UP";

                command.Parameters.AddWithValue("@UN", UserRecord.Username);

                // Encypt the Password

                // Chand, M. (2020, April 16). Compute SHA256 Hash In C#. Retrieved from c-sharpcorner: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
                string HashedPassword = "";

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Get a Byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UserRecord.Password));

                    // Convert Byte array to string 
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    HashedPassword = builder.ToString();

                }


                command.Parameters.AddWithValue("@UP", HashedPassword);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserRecord.Id = reader.GetInt32(0);
                    UserRecord.Username = reader.GetString(1);
                    UserRecord.Role = reader.GetString(6);
  
                }

                if (!string.IsNullOrEmpty(UserRecord.Role))
                {
                    SessionID = HttpContext.Session.Id;
                    HttpContext.Session.SetString("sessionID", SessionID);
                    HttpContext.Session.SetInt32("userID", UserRecord.Id);
                    HttpContext.Session.SetString("username", UserRecord.Username);
                    HttpContext.Session.SetString("role", UserRecord.Role);


                    if (UserRecord.Role == "Customer")
                    {
                        return RedirectToPage("/Customer/CustomerIndex");
                    }
                    else if (UserRecord.Role == "Librarian")
                    {
                        return RedirectToPage("/Librarian/LibrarianIndex"); 
                    }
                    else 
                    {
                        return RedirectToPage("/Admin/AdminIndex");
                    }


                }
                else
                {
                    Message = "Invalid Username or Password!";
                    return Page();
                }
            }

        }
    }
}
