using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibraryLove.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.User
{
    public class EditProfileModel : PageModel
    {

        public string SessionID;
        public const string SessionKeyName1 = "sessionID";

        public int Id;
        public const string SessionKeyName2 = "userID";


        [BindProperty]
        public Member member { get; set; }

        public IActionResult OnGet()
        {
            //Takes context strings of all needed variables to display current values for each in the form box

            SessionID = HttpContext.Session.GetString(SessionKeyName1);
            Id = (int)HttpContext.Session.GetInt32(SessionKeyName2);

            //checks if session is correct
            if (string.IsNullOrEmpty(SessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("../Browser/Login");
            }


            // Connect to Database
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                // SQL Query
                command.CommandText = @"SELECT * FROM Member WHERE Id = @UId";
                command.Parameters.AddWithValue("@UID", Id);
                SqlDataReader reader = command.ExecuteReader(); // read records 

                member = new Member();

                while (reader.Read())
                {
                    member.Id = reader.GetInt32(0);
                    member.Username = reader.GetString(1);
                    member.FirstName = reader.GetString(2);
                    member.LastName = reader.GetString(3);
                    member.Email = reader.GetString(4);
                    member.MemberRole = (UserRole?)Enum.Parse(typeof(UserRole), reader.GetString(6)); // get the value of the Role

                    if (reader.IsDBNull(7))
                    {
                        member.SecurityQuestion = null;
                        member.SecurityAnswer = null;
                    }
                    else
                    {
                        member.SecurityQuestion = (Security?)reader.GetInt32(7);
                        member.SecurityAnswer = reader.GetString(8);

                    }

                }

                reader.Close();
            }
            return Page();
        }

        public IActionResult OnPost() //On press of the submit changes button 
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
                //Updates each variable accessiblee 
                command.Connection = conn;
                command.CommandText = "UPDATE Member SET Username = @User, FirstName = @UFN, LastName = @ULN, Email = @Em, SecuityQuestion = @SQ, SecuityAnswer = @SA WHERE Id = @UID";
                command.Parameters.AddWithValue("@UID", member.Id);
                command.Parameters.AddWithValue("@User", member.Username);
                command.Parameters.AddWithValue("@UFN", member.FirstName);
                command.Parameters.AddWithValue("@ULN", member.LastName);
                command.Parameters.AddWithValue("@Em", member.Email);
                command.Parameters.AddWithValue("@SQ", member.SecurityQuestion);
                command.Parameters.AddWithValue("@SA", member.SecurityAnswer);


                command.ExecuteNonQuery();

                return RedirectToPage("/User/ViewAccount");

            }

        }

    }
}



