using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Dashboard
{
    public class QuestionTextAvgScore
    {
        public DateTimeOffset CreatedAt { get; set; }
        public int? Sequence { get; set; }
        public string QuestionText { get; set; }
        public double? AverageScore { get; set; }
    }
}