using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IQuestionResponseRepository : IGenericRepository<QuestionResponse>
    {
    }

    public class QuestionResponseRepository : GenericRepository<QuestionResponse>, IQuestionResponseRepository
    {
        public QuestionResponseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}