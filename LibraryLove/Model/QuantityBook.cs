using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public class QuantityBook
    {
        public int BookId { get; set; }
        public int? CurrentBooks { get; set; }

        [Display(Name = "Number of Books to add")]
        public int AddedBooks{ get; set; }

     
    }
}
