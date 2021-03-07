using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Notes__Marketplace.Models
{
   
    public class ContactUSModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailID { get; set; }


        [Required]
        public string Subject { get; set; }

        [Required]
        public string Comment {get; set; }

   
    }
}
