using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using LibraryLove.Model;

namespace LibraryLove.Pages.User.ForgotPassword
{
    public class VerifyUserModel : PageModel
    {
        [BindProperty]
        public Member NewUser { get; set; }

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
                command.CommandText = @"SELECT Id, SecuityQuestion, SecuityAnswer FROM Member WHERE Email = @Em AND Username = @User";
               
                //Checks to see if an account exists with the given email address and username
                command.Parameters.AddWithValue("@Em", NewUser.Email);
                command.Parameters.AddWithValue("@User", NewUser.Username);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NewUser.Id = reader.GetInt32(0);
                    if (reader.IsDBNull(1))
                    {
                        // This user does not have an secuirty question, they are an employee so they must contact a leading admin
                        Message = "Invalid Password Reset. Please contact your leading admin";
                        return Page();
                    }
                    else
                    {
                        NewUser.SecurityQuestion = (Security?)reader.GetInt32(1);
                        NewUser.SecurityAnswer = reader.GetString(2);
                    }

                }

                if (!string.IsNullOrEmpty(NewUser.SecurityAnswer))
                {
                    // enter the session so we know the user's details for later
                    SessionID = HttpContext.Session.Id;
                    HttpContext.Session.SetString("sessionID", SessionID);
                    HttpContext.Session.SetInt32("userID", NewUser.Id);
                    HttpContext.Session.SetInt32("squestion", (int)NewUser.SecurityQuestion);
                    HttpContext.Session.SetString("sanswer", NewUser.SecurityAnswer);

                    //If an account exists with this email address and username then it creates a session and saves the above variables for future use
                    return RedirectToPage("SecuirtyQuestion");
                }
                else
                {
                    //If an account does not exist with the given email address then it informs the user
                    Message = "Invalid User Details";
                    return Page();
                }
            }

        }
    }
}
