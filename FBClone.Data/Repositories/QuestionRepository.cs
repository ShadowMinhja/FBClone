using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
    }

    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}