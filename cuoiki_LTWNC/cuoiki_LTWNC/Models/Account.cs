using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace cuoiki_LTWNC.Models
{
    public class Account
    {
        [Required(ErrorMessage = "MSV cannot be blank")]
        public string MSV { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        public string Password { get; set; }
    }
}