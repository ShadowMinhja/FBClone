using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FBClone.Model;
using FBClone.Model.ViewModels;

namespace FBClone.Service.Common
{
    public static class Utils
    {
        public static string GetMD5Hash(string input)
        {
            //Calculate MD5 hash. This requires that the string is splitted into a byte[].
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(input);
            byte[] result = md5.ComputeHash(textToHash);

            //Convert result back to string.
            return System.BitConverter.ToString(result);
        }

        public static double ComputeTotalScore(List<QuestionResponse> questionResponses)
        {
            double totalScore = 0.0;
            List<double> questionScores = new List<double>();
            List<double> questionFactors = new List<double>();
            double totalQuestionFactors = 0.0;
            //Collect all score and factors
            for (int i = 0; i < questionResponses.Count(); i++)
            {
                var q = questionResponses[i].Question;
                if (q.QuestionType.ToUpper() != "COMMENT" && q.QuestionType.ToUpper() != "CHECKBOX" && questionResponses[i].AnswerId != null)
                {
                    var questionResponseScore = questionResponses[i].QuestionScore;
                    var questionFactor = q.QuestionFactor;
                    questionScores.Add(questionResponseScore);
                    questionFactors.Add(questionFactor);
                    totalQuestionFactors += questionFactor;                    
                }
            }
            //Compute total weighted score
            for (int j = 0; j < questionScores.Count(); j++)
            {
                var weightedScore = questionScores.ElementAt(j) * (questionFactors.ElementAt(j) / totalQuestionFactors);
                totalScore += weightedScore;
            }
            return totalScore;
        }

        public static SurveyCategoryScoresViewModel GetSurveyScores(QuestionResponseSet questionResponseSet)
        {
            SurveyCategoryScoresViewModel surveyCategoryScoresViewModel = new SurveyCategoryScoresViewModel();
            List<string> categories = questionResponseSet.QuestionResponses
                                                            .Select(x => x.Question.Category.Name)
                                                            .Distinct()
                                                            .ToList();
            List<CategoryScore> categoryScores = new List<CategoryScore>();
            foreach(var category in categories)
            {
                var categoryList = questionResponseSet.QuestionResponses.Where(x => x.Question.Category.Name == category).ToList();
                double totalScore = ComputeTotalScore(categoryList);
                categoryScores.Add(new CategoryScore {
                    Category = category,
                    TotalScore = totalScore * 100,
                    Positivity = GetProgressBarColor(GetPositivity(totalScore))
                });
            }
            surveyCategoryScoresViewModel.CategoryScores = categoryScores;
            return surveyCategoryScoresViewModel;
        }

        public static string GetPositivity(double? totalScore)
        {
            if (totalScore != null)
            {
                if (totalScore >= 0.80)
                    return "Good";
                else if (totalScore > 0.65 && totalScore < 0.79)
                {
                    return "Average";
                }
                else
                    return "Bad";
            }
            else
            {
                return "N/A";
            }
        }

        public static string GetProgressBarColor(string positivity)
        {
            switch (positivity)
            {
                case "Average":
                    return "progress progress-warning";
                case "Bad":
                    return "progress progress-danger";
                case "Good":
                default:
                    return "progress progress-info";
            }
        }


    }
}
