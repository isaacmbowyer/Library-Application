using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using LibraryLove.Model;
using Microsoft.AspNetCore.Http;

namespace LibraryLove.Pages.User
{
    public class ViewAccountModel : PageModel
    {
        public int Id;
        public const string SessionKeyName1 = "userID";


        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        [BindProperty]
        public Member member { get; set; }


        public IActionResult OnGet()
        {
            //calls variables for the model for the webpage to view
            Id = (int)HttpContext.Session.GetInt32(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
  

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
       
    }
}


