using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Notes__Marketplace.Models
{
   public class RejectNotesModel
    {
        public String noteID { get; set; }

        public String NotesTitle { get; set; }

        [Required]
        public String Remark { get; set; }

        public String SellerID { get; set; }
    }
}
