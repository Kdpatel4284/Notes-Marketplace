using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Notes__Marketplace.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EmailID { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
