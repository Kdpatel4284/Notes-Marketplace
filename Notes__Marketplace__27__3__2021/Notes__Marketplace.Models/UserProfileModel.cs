using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes__Marketplace.Models
{
  public  class UserProfileModel
    {
     
        public int ID { get; set; }

        public int UserID { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public string SecondaryEmailAddress { get; set; }
        [Required]
        public string PhonenumberCountryCode { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }
        public string University { get; set; }
        public string College { get; set; }

    }
}
