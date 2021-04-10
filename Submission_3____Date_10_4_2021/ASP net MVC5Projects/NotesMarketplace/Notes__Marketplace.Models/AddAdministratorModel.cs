using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes__Marketplace.Models
{
  public class AddAdministratorModel
    {


        public int UserID { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }
        [Required]
        public String EmailID { get; set; }

        [Required]
        public string PhonenumberCountryCode { get; set; }
        [Required]
        public string Phonenumber { get; set; }
    }
}
