using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Dashboard
{
    public class StaffMemberAvgScore
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string Name { get; set; }
        public double? AverageScore { get; set; }
        public double? AverageDuration { get; set; }
        public int? TotalSubscriptions { get; set; }
        public int? CountOccurrences { get; set; }
    }
}