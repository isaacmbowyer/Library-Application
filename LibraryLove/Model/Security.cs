using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryLove.Model
{
    public enum Security
    {
        [Display(Name = "What is your favourite colour?")]
        SQ1,
        [Display(Name = "What school did you attend when you were 12?")]
        SQ2,
        [Display(Name = "What is the name of your favourite resturant?")]
        SQ3,
        [Display(Name = "What is your sibling's middle name?")]
        SQ4,
        [Display(Name = "What was your favourite subject at school?")]
        SQ5

    }
 
}
