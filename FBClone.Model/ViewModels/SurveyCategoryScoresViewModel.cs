using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model.ViewModels
{
    public class SurveyCategoryScoresViewModel
    {
        public List<CategoryScore> CategoryScores { get; set; }
    }

    public class CategoryScore
    {
        public string Category { get; set; }
        public double TotalScore { get; set; }
        public string Positivity { get; set; }
    }
}
