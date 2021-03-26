using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes__Marketplace.Models
{
  public  class NotesReviewModel
    {
        public int ID { get; set; }
        public int NoteID { get; set; }
        public int ReviewedByID { get; set; }
        public int AgainstDownloadsID { get; set; }
        public decimal Ratings { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModififedBy { get; set; }
        public bool IsActive { get; set; }

      
    }
}
