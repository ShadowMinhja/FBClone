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
    public interface IAnswerService
    {
        Answer GetById(string Id);
        IEnumerable<Answer> GetAll();
        IEnumerable<Answer> GetMany(string userName);
        IQueryable<Answer> Query(Expression<Func<Answer, bool>> where);

        Answer Add(Answer answer);
        Answer Update(Answer answer);
        void Delete(string id);
    }
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAnswerRepository answerRepository;
        public AnswerService(IUnitOfWork unitOfWork, IAnswerRepository answerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.answerRepository = answerRepository;
        }

        public Answer GetById(string id)
        {
            return answerRepository.GetById(id);
        }

        public IEnumerable<Answer> GetAll()
        {
            return answerRepository.GetAll();
        }

        public IEnumerable<Answer> GetMany(string answerText)
        {
            return answerRepository.GetMany(x => x.AnswerText == answerText);
        }

        public IQueryable<Answer> Query(Expression<Func<Answer, bool>> where)
        {
            return answerRepository.Query(where);
        }

        public Answer Add(Answer answer)
        {
            answerRepository.Add(answer);
            unitOfWork.SaveChanges();
            return answer;
        }

        public Answer Update(Answer answer)
        {
            answerRepository.Update(answer);
            unitOfWork.SaveChanges();
            return answer;
        }

        public void Delete(string id)
        {
            var answer = answerRepository.GetById(id);
            var answers = answerRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in answers)
            {
                answerRepository.Delete(item);
            }

            if (answer != null)
            {
                answerRepository.Delete(answer);
                unitOfWork.SaveChanges();
            }
        }

    }
}
