using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Guestcards
{
    public class MenuItemReview
    {
        public long? CountOrders{ get; set; }
        public double? AverageScore { get; set; }
    }
}