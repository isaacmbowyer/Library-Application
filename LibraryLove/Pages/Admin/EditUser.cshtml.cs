using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Member record { get; set; }
        public void OnGet(int? id)
        {
            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            record = new Member();

            // SQL Query
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Member WHERE Id = @UId";
                command.Parameters.AddWithValue("@UId", id);


                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    record.Id = reader.GetInt32(0);
                    record.Username = reader.GetString(1);
                    record.FirstName = reader.GetString(2);
                    record.LastName = reader.GetString(3);
                    record.Email = reader.GetString(4);
                    record.MemberRole = (UserRole?)Enum.Parse(typeof(UserRole), reader.GetString(6)); // get the value of the Role
                }

                reader.Close(); // finished reading 
            }
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

                // SQL Query
                using (SqlCommand command = new SqlCommand())
                {

                    command.Connection = conn;
                    command.CommandText = @"UPDATE Member SET Username = @User, FirstName = @FName, LastName = @LName, Email = @UEmail, Role = @URole WHERE Id = @UId";
                    command.Parameters.AddWithValue("@UId", record.Id);

                    // Edit
                    command.Parameters.AddWithValue("@User", record.Username);
                    command.Parameters.AddWithValue("@FName", record.FirstName);
                    command.Parameters.AddWithValue("@LName", record.LastName);
                    command.Parameters.AddWithValue("@UEmail", record.Email);
                    command.Parameters.AddWithValue("@URole", record.MemberRole.ToString());  // get the name of the role as a string 


                }

                return RedirectToPage("AdminIndex");
            }

            return Page();


        }
    }
}
