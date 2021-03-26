using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Notes_MarketPlace.Db;

namespace Notes__Marketplace.Models
{
    public class DashboardMergeModel
    {

        public List<SellerNotes> IPNotes { get; set; }
        public List<SellerNotes> PublishNotes { get; set; }

    }
}