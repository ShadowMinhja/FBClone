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
    public interface IAnswerDraftService
    {
        AnswerDraft GetById(string Id);
        IEnumerable<AnswerDraft> GetAll();
        IEnumerable<AnswerDraft> GetMany(string userName);
        IQueryable<AnswerDraft> Query(Expression<Func<AnswerDraft, bool>> where);
        AnswerDraft Add(AnswerDraft answerDraft);
        void CreateBulk(List<AnswerDraft> answerDrafts);
        AnswerDraft Update(AnswerDraft answerDraft);
        void UpdateBulk(List<AnswerDraft> answerDrafts);

        void Delete(string id);
    }
    public class AnswerDraftService : IAnswerDraftService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAnswerDraftRepository answerDraftRepository;
        public AnswerDraftService(IUnitOfWork unitOfWork, IAnswerDraftRepository answerDraftRepository)
        {
            this.unitOfWork = unitOfWork;
            this.answerDraftRepository = answerDraftRepository;
        }

        public AnswerDraft GetById(string id)
        {
            return answerDraftRepository.GetById(id);
        }

        public IEnumerable<AnswerDraft> GetAll()
        {
            return answerDraftRepository.GetAll();
        }

        public IEnumerable<AnswerDraft> GetMany(string answerDraftText)
        {
            return answerDraftRepository.GetMany(x => x.AnswerText == answerDraftText);
        }

        public IQueryable<AnswerDraft> Query(Expression<Func<AnswerDraft, bool>> where)
        {
            return answerDraftRepository.Query(where);
        }

        public AnswerDraft Add(AnswerDraft answerDraft)
        {
            answerDraftRepository.Add(answerDraft);
            unitOfWork.SaveChanges();
            return answerDraft;
        }

        public void CreateBulk(List<AnswerDraft> answerDrafts)
        {
            foreach (var answerDraft in answerDrafts)
            {
                answerDraft.QuestionDraft = null; //Override to null to avoid circular dependency
                answerDraftRepository.Add(answerDraft);
            }
            unitOfWork.SaveChanges();
        }

        public AnswerDraft Update(AnswerDraft answerDraft)
        {
            answerDraftRepository.Update(answerDraft);
            unitOfWork.SaveChanges();
            return answerDraft;
        }

        public void UpdateBulk(List<AnswerDraft> answerDrafts)
        {
            foreach (var answerDraft in answerDrafts)
            {
                if(answerDraft.Id != null)
                    answerDraftRepository.Update(answerDraft);
                else
                    answerDraftRepository.Add(answerDraft);
            }
            unitOfWork.SaveChanges();
        }

        public void Delete(string id)
        {
            var answerDraft = answerDraftRepository.GetById(id);
            if (answerDraft != null)
            {
                answerDraftRepository.Delete(answerDraft);
                unitOfWork.SaveChanges();
            }
        }

    }
}
