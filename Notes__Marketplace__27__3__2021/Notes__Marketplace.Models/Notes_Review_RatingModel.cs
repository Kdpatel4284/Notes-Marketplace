using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes__Marketplace.Models
{
   public  class Notes_Review_RatingModel
    {

        public int ID { get; set; }
        public int NoteID { get; set; }
        public int? AverageRating { get; set; }
        public int? TotalReview { get; set; }
        public int? TotalReport { get; set; }
       
    }
}
