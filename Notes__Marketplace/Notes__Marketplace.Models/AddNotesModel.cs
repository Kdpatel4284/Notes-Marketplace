using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Notes__Marketplace.Models
{
 public class AddNotesModel
    {

        public int ID { get; set; }
        public int SellerID { get; set; }
        public int Status { get; set; }
     
        [Required]
        public string Title { get; set; }

        [Required]
        public int Category { get; set; }

        public string displayPicture;
        public int? Note_Type { get; set; }
        public int? NumberofPages { get; set; }

        [Required]
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public int? Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        public String IsPaid { get; set; }

        [Required]
        public decimal? SellingPrice { get; set; }

        private string notesPreview;

        public string GetNotesPreview()
        {
            return notesPreview;
        }

        public void SetNotesPreview(string value)
        {
            notesPreview = value;
        }

        public bool IsActive { get; set; }




    }
}
