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

        public string SessionID;
        public const string SessionKeyName1 = "sessionID";


        public IActionResult OnGet()
        {
            SessionID = HttpContext.Session.GetString(SessionKeyName1);
            //checks to see if session exists. Librarian Default page
           
            if (string.IsNullOrEmpty(SessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("../Browser/Login");
            }
            return Page();

        }
    }
}
