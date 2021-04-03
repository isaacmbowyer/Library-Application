using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Librarian
{
    public class LibrarianIndexModel : PageModel
    {

        public int Id;
        public const string SessionKeyName1 = "userID";

        public string UserName;
        public const string SessionKeyName2 = "username";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";


        public IActionResult OnGet()
        {
            Id = (int)HttpContext.Session.GetInt32(SessionKeyName1);
            UserName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
            //checks to see if session exists. Librarian Default page
           
            if (string.IsNullOrEmpty(UserName))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Browser/Login");
            }
            return Page();

        }
    }
}
