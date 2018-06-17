using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetlSityTest.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        [Required]
        [Compare("Pass", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PassConfirm { get; set; }
    }
}