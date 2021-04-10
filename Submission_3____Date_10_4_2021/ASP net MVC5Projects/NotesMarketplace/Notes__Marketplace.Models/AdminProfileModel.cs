using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes__Marketplace.Models
{
    public class AdminProfileModel
    {

        public int ID { get; set; }

        public int UserID { get; set; }

        public string EmailAddress { get; set; }

        public string SecondaryEmailAddress { get; set; }
        [Required]
        public string PhonenumberCountryCode { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        public string ProfilePicture { get; set; }
       

    }
}
