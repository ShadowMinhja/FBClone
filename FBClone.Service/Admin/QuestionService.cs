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
    public interface IQuestionService
    {
        Question GetById(string Id);
        IEnumerable<Question> GetAll();
        IEnumerable<Question> GetMany(string userName);
        IQueryable<Question> Query(Expression<Func<Question, bool>> where);
        Question Add(Question question);
        Question Update(Question question);
        void Delete(string id);
    }
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        public QuestionService(IUnitOfWork unitOfWork, IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        public Question GetById(string id)
        {
            return questionRepository.GetById(id);
        }

        public IEnumerable<Question> GetAll()
        {
            return questionRepository.GetAll();
        }

        public IEnumerable<Question> GetMany(string questionText)
        {
            return questionRepository.GetMany(x => x.QuestionText == questionText);
        }

        public IQueryable<Question> Query(Expression<Func<Question, bool>> where)
        {
            return questionRepository.Query(where);
        }

        public Question Add(Question question)
        {
            questionRepository.Add(question);
            unitOfWork.SaveChanges();
            return question;
        }

        public Question Update(Question question)
        {
            questionRepository.Update(question);
            unitOfWork.SaveChanges();
            return question;
        }

        public void Delete(string id)
        {
            var question = questionRepository.GetById(id);
            var questions = questionRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in questions)
            {
                var answers = answerRepository.GetAll().Where(a => a.QuestionId == item.Id).ToList();
                foreach(var a in answers)
                {
                    answerRepository.Delete(a);
                }
                questionRepository.Delete(item);
            }

            if (question != null)
            {
                questionRepository.Delete(question);
                unitOfWork.SaveChanges();
            }
        }

    }
}
