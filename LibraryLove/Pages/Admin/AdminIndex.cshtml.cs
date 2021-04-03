using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLove.Pages.Admin
{
    public class AdminIndexModel : PageModel
    {
        public int Id;
        public string SessionKeyName1 = "userID";

        public string UserName;
        public const string SessionKeyName2 = "username";


        public string SessionID;
        public const string SessionKeyName3 = "sessionID";


        public IActionResult OnGet()
        {
            //Default page for admin
            Id = (int)HttpContext.Session.GetInt32(SessionKeyName1);
            UserName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(UserName))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Login/Login");
            }
            return Page();


        }
    }
}
