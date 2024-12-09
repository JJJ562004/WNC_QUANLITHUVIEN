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
        [Required(ErrorMessage = "Ten cannot be blank")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Ho cannot be blank")]
        public string Ho { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dia chi cannot be blank")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "EnrollmentDate cannot be blank")]
        public string EnrollmentDate { get; set; }
    }
}