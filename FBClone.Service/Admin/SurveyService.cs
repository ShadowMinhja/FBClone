using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface ISurveyService
    {
        Survey GetById(string Id);
        IEnumerable<Survey> GetAll();
        IEnumerable<Survey> GetMany(string userName);
        IQueryable<Survey> Query(Expression<Func<Survey, bool>> where);
        IQueryable<Survey> AllIncluding(Expression<Func<Survey, bool>> where);
        Survey Add(Survey survey);
        Survey Update(Survey survey);
        void Delete(string id);
        void Publish(Survey survey, bool active);
    }
    public class SurveyService : ISurveyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurveyRepository surveyRepository;
        private readonly IQuestionService questionService;
        private readonly IQuestionDraftService questionDraftService;
        private FBCloneContext dbContext;
        public SurveyService(IUnitOfWork unitOfWork, ISurveyRepository surveyRepository, IQuestionService questionService, IQuestionDraftService questionDraftService)
        {
            this.unitOfWork = unitOfWork;
            this.surveyRepository = surveyRepository;
            this.questionService = questionService;
            this.questionDraftService = questionDraftService;
            this.dbContext = new FBCloneContext();
        }

        public Survey GetById(string id)
        {
            return surveyRepository.GetById(id);
        }

        public IEnumerable<Survey> GetAll()
        {
            return surveyRepository.GetAll();
        }

        public IEnumerable<Survey> GetMany(string name)
        {
            return surveyRepository.GetMany(x => x.Name == name);
        }

        public IQueryable<Survey> Query(Expression<Func<Survey, bool>> where)
        {
            return surveyRepository.Query(where);
        }


        public IQueryable<Survey> AllIncluding(Expression<Func<Survey, bool>> where)
        {
            return surveyRepository.AllIncluding(
                    s => s.QuestionDrafts, 
                    s => s.QuestionDrafts.Select(a => a.AnswerDrafts), 
                    s => s.QuestionDrafts.Select(c => c.Category)
                ).Where(where);
        }

        public Survey Add(Survey survey)
        {
            surveyRepository.Add(survey);
            unitOfWork.SaveChanges();
            return survey;
        }

        public Survey Update(Survey survey)
        {
            surveyRepository.Update(survey);
            unitOfWork.SaveChanges();
            return survey;
        }

        public void Delete(string id)
        {
            var survey = surveyRepository.GetById(id);
            var surveys = surveyRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in surveys)
            {
                var questions = questionService.GetAll().Where(q => q.SurveyId == item.Id).ToList();
                var questionDrafts = questionDraftService.GetAll().Where(qd => qd.SurveyId == item.Id).ToList();
                foreach(var q in questions)
                {
                    questionService.Delete(q.Id);
                }
                foreach(var qd in questionDrafts)
                {
                    questionDraftService.Delete(qd.Id);
                }
                surveyRepository.Delete(item);
            }

            if (survey != null)
            {
                surveyRepository.Delete(survey);
                try {
                    unitOfWork.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }
        }

        public void Publish(Survey survey, bool active)
        {
            if (active) //Publish
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter{
                        ParameterName = "@surveyId",
                        Value = survey.Id
                    }
                };
                this.dbContext.Database.ExecuteSqlCommand("EXEC spPublishSurvey @surveyId", parameters);
            }
        }

    }
}
