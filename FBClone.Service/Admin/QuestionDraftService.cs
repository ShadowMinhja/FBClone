using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface IQuestionDraftService
    {
        QuestionDraft GetById(string Id);
        IEnumerable<QuestionDraft> GetAll();
        IEnumerable<QuestionDraft> GetMany(string userName);
        IQueryable<QuestionDraft> Query(Expression<Func<QuestionDraft, bool>> where);
        QuestionDraft Add(QuestionDraft questionDraft);
        QuestionDraft Update(QuestionDraft questionDraft);
        void Delete(string id);
    }
    public class QuestionDraftService : IQuestionDraftService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IQuestionDraftRepository questionDraftRepository;
        private readonly IAnswerDraftRepository answerDraftRepository;
        public QuestionDraftService(IUnitOfWork unitOfWork, IQuestionDraftRepository questionDraftRepository, IAnswerDraftRepository answerDraftRepository)
        {
            this.unitOfWork = unitOfWork;
            this.questionDraftRepository = questionDraftRepository;
            this.answerDraftRepository = answerDraftRepository;
        }

        public QuestionDraft GetById(string id)
        {
            return questionDraftRepository.GetById(id);
        }

        public IEnumerable<QuestionDraft> GetAll()
        {
            return questionDraftRepository.GetAll();
        }

        public IEnumerable<QuestionDraft> GetMany(string questionDraftText)
        {
            return questionDraftRepository.GetMany(x => x.QuestionText == questionDraftText);
        }

        public IQueryable<QuestionDraft> Query(Expression<Func<QuestionDraft, bool>> where)
        {
            return questionDraftRepository.Query(where);
        }

        public QuestionDraft Add(QuestionDraft questionDraft)
        {
            questionDraftRepository.Add(questionDraft);
            unitOfWork.SaveChanges();
            return questionDraft;
        }

        public QuestionDraft Update(QuestionDraft questionDraft)
        {
            questionDraftRepository.Update(questionDraft);
            unitOfWork.SaveChanges();
            return questionDraft;
        }

        public void Delete(string id)
        {   
            var questionDraft = questionDraftRepository.GetById(id);
            var questionDrafts = questionDraftRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships


            foreach (var item in questionDrafts)
            {
                var answerDrafts = answerDraftRepository.GetAll().Where(a => a.QuestionId == item.Id).ToList();
                foreach(var ad in answerDrafts)
                {
                    answerDraftRepository.Delete(ad);
                }
                questionDraftRepository.Delete(item);
            }

            if (questionDraft != null)
            {
                questionDraftRepository.Delete(questionDraft);
                unitOfWork.SaveChanges();
            }
        }

    }
}
