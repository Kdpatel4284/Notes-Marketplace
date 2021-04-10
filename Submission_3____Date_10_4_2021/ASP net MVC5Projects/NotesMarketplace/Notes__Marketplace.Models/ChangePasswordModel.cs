using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Notes__Marketplace.Models
{
   public class ChangePasswordModel
    {


        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}
