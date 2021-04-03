using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class PreLoanedBook
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Username")]
        public string CustomerUsername { get; set; }

        [Display(Name = "First Name")]
        public string CustomerFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string CustomerLastName { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Date Pre-Loaned")]
        public DateTime PreLoanedDate { get; set; }

        public List<bool> isPreLoaned{ get; set; }
    }
}
