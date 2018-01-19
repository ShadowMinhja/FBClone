using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Dashboard
{
    public class QuestionResponseSetPositivity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string Positivity { get; set; }
        public double? AverageScore { get; set; }
        public int? CountOccurrences { get; set; }
    }
}