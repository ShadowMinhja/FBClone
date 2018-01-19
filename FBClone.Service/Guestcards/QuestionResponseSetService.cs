using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;
using FBClone.Service.Common;

namespace FBClone.Service
{
    public interface IQuestionResponseSetService
    {
        QuestionResponseSet GetById(string Id);
        IEnumerable<QuestionResponseSet> GetAll();
        IEnumerable<QuestionResponseSet> GetMany(string userName);
        IQueryable<QuestionResponseSet> Query(Expression<Func<QuestionResponseSet, bool>> where);
        QuestionResponseSet Add(QuestionResponseSet questionResponseSet);
        QuestionResponseSet Update(QuestionResponseSet questionResponseSet);
        void Delete(string id);
        Dictionary<string, double> GetCategoryScores(QuestionResponseSet newQuestionResponseSet);
    }
    public class QuestionResponseSetService : IQuestionResponseSetService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IQuestionResponseSetRepository questionResponseSetRepository;
        private readonly IQuestionResponseRepository questionResponseRepository;
        private readonly IQuestionService questionService;
        private readonly IAnswerService answerService;

        public QuestionResponseSetService(IUnitOfWork unitOfWork, IQuestionResponseSetRepository questionResponseSetRepository, IQuestionResponseRepository questionResponseRepository, IQuestionService questionService, IAnswerService answerService)
        {
            this.unitOfWork = unitOfWork;
            this.questionResponseSetRepository = questionResponseSetRepository;
            this.questionResponseRepository = questionResponseRepository;
            this.questionService = questionService;
            this.answerService = answerService;
        }

        public QuestionResponseSet GetById(string id)
        {
            return questionResponseSetRepository.GetById(id);
        }

        public IEnumerable<QuestionResponseSet> GetAll()
        {
            return questionResponseSetRepository.AllIncluding(x => x.Survey, x => x.QuestionResponses);
        }

        public IEnumerable<QuestionResponseSet> GetMany(string tableNumber)
        {
            return questionResponseSetRepository.GetMany(x => x.TableNumber == tableNumber);
        }

        public IQueryable<QuestionResponseSet> Query(Expression<Func<QuestionResponseSet, bool>> where)
        {
            return questionResponseSetRepository.Query(where);
        }

        public QuestionResponseSet Add(QuestionResponseSet questionResponseSet)
        {
            QuestionResponseSet newQuestionResponseSet = new QuestionResponseSet {
                SurveyId = questionResponseSet.SurveyId,
                LocationId = questionResponseSet.LocationId,
                CustomerName = questionResponseSet.CustomerName,
                CustomerEmail = questionResponseSet.CustomerEmail,
                IsSubscribe = questionResponseSet.IsSubscribe,
                Positivity = questionResponseSet.Positivity,
                TotalScore = questionResponseSet.TotalScore,
                SessionDuration = questionResponseSet.SessionDuration,
                UserId = questionResponseSet.UserId,
                CreatedBy = questionResponseSet.CreatedBy,
                UpdatedBy = questionResponseSet.UpdatedBy
            };
            
            questionResponseSetRepository.Add(newQuestionResponseSet);
            foreach (var questionResponse in questionResponseSet.QuestionResponses)
            {
                questionResponse.QuestionResponseSetId = newQuestionResponseSet.Id;
                questionResponse.UserId= newQuestionResponseSet.UserId;
                questionResponse.CreatedBy = newQuestionResponseSet.CreatedBy;
                questionResponse.UpdatedBy = newQuestionResponseSet.UpdatedBy;
                questionResponseRepository.Add(questionResponse);
            }
            unitOfWork.SaveChanges();
            return newQuestionResponseSet;
        }

        public QuestionResponseSet Update(QuestionResponseSet QuestionResponseSet)
        {
            questionResponseSetRepository.Update(QuestionResponseSet);
            unitOfWork.SaveChanges();
            return QuestionResponseSet;
        }

        public void Delete(string id)
        {   
            var QuestionResponseSet = questionResponseSetRepository.GetById(id);
            var QuestionResponseSets = questionResponseSetRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships


            foreach (var item in QuestionResponseSets)
            {
                questionResponseSetRepository.Delete(item);
            }

            if (QuestionResponseSet != null)
            {
                questionResponseSetRepository.Delete(QuestionResponseSet);
                unitOfWork.SaveChanges();
            }
        }

        public Dictionary<string, double> GetCategoryScores(QuestionResponseSet newQuestionResponseSet)
        {
            Dictionary<string, double> categoryScores = new Dictionary<string, double>();
            List<string> categories = newQuestionResponseSet.QuestionResponses.Select(x => x.Question.Category.Name).Distinct().ToList();
            foreach(string category in categories)
            {
                List<QuestionResponse> questionResponses = newQuestionResponseSet.QuestionResponses.Where(x => x.Question.Category.Name == category).ToList();
                double totalScore = Utils.ComputeTotalScore(questionResponses);
                categoryScores.Add(category, totalScore);
            }
            return categoryScores;
        }
    }
}
