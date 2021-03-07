using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Notes__Marketplace.Models
{
   public class ForgotPasswordModel
    {

        [Required]
        [EmailAddress]
        public string EmailID { get; set; }


    }
}
