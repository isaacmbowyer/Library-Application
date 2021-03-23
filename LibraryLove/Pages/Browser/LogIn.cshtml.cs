using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Browser
{
    public class LogInModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Member MemberRecord { get; set; }

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
                // SQL Query

                command.CommandText = @"SELECT Username, Password FROM Member WHERE Username = @User AND Password = @Pass";



                // Encypt the Password to see if it matches the password in the database

                // Create SHA256 hash
                string HashedPassword = "";

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Get a Byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(MemberRecord.Password));

                    // Convert Byte array to string 
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    HashedPassword = builder.ToString();

                }
                // wipe out the password the user entered
                MemberRecord.Password = null;

                command.Parameters.AddWithValue("@User", MemberRecord.Username);
                command.Parameters.AddWithValue("@Pass", HashedPassword);

                SqlDataReader reader = command.ExecuteReader(); // read records 

                Member record = new Member();
                while (reader.Read())
                {

                    record.Username = reader.GetString(0);
                    record.Password = reader.GetString(1);
                }

                reader.Close();

                if (record.Username != null && record.Password != null)
                {
                    return RedirectToPage("/Librarian/View");
                }


            }

            return Page();
        }
    }
}
