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
    public class SecurityQuestionModel : PageModel
    {
        //Calls the variables from the Session 
        public Security? SecurityQuestion;
        public const string SessionKeyName1 = "squestion";

        public string SecurityAnswer;
        public const string SessionKeyName2 = "sanswer";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        public string Message { get; set; }

        [BindProperty]
        public Member NewUser { get; set; }

        public IActionResult OnGet()
        {
            // retirve the infomation from the session
            SecurityQuestion = (Security?)HttpContext.Session.GetInt32(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(SessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            return Page();

        }

        public IActionResult OnPost()
        {

            // retirve the infomation from the session
            SecurityAnswer = HttpContext.Session.GetString(SessionKeyName2);

            // compare the Secuirty Answer with the one selected from user
            if (NewUser.SecurityAnswer == SecurityAnswer)
            {
                // User has entered all the correct details, they can now change their password
               return RedirectToPage("/ResetPassword");
            }

            Message = "Incorrect details. Try again";
            return Page();

        }
    }
}