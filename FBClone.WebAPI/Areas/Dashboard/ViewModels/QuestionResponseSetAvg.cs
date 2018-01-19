using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Dashboard
{
    public class QuestionResponseSetAvg
    {
        public DateTimeOffset CreatedAt { get; set; }
        public double? AverageScore { get; set; }
        public double? AverageDuration { get; set; }
        public int? TotalSubscriptions { get; set; }
    }
}