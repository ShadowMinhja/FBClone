using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Guestcards
{
    public class LocationReview
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public double? AverageScore { get; set; }
        public int? TotalCount { get; set; }
        public double? TotalScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}