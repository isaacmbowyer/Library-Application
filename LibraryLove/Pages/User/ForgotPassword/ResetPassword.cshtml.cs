using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using LibraryLove.Model;
using System.Security.Cryptography;
using System.Text;

namespace LibraryLove.Pages.User.ForgotPassword
{
    public class ResetPasswordModel : PageModel
    {
        public int Id;
        public string SessionKeyName1 = "userID";

        public string UserName;
        public const string SessionKeyName2 = "username";


        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        [BindProperty]
        public CreateMember NewUser { get; set; }

        public IActionResult OnGet()
        {
            // retirve the infomation from the session
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(SessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            return Page();

        }
        public IActionResult OnPost(CreateMember NewUser)
        {

            // Connect to Database 
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            using (SqlCommand command = new SqlCommand())

            {
                //Updates the password of the user with the new password
                command.Connection = conn;
                command.CommandText = @"UPDATE Member SET Password = @UPass WHERE Id = @UID";

                // Encypt the Password

                // Chand, M. (2020, April 16). Compute SHA256 Hash In C#. Retrieved from c-sharpcorner: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
                string HashedPassword = "";

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Get a Byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(NewUser.Password));

                    // Convert Byte array to string 
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    HashedPassword = builder.ToString();

                }
                command.Parameters.AddWithValue("@UPass", HashedPassword);
                command.Parameters.AddWithValue("@UID", HttpContext.Session.GetInt32(SessionKeyName1));


                command.ExecuteNonQuery();

                // clear session
                HttpContext.Session.Clear();

                // let user login
                return RedirectToPage("../../Browser/Login");

            }
        }

    }
}
