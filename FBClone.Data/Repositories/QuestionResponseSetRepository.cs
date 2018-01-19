using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IQuestionResponseSetRepository : IGenericRepository<QuestionResponseSet>
    {
    }

    public class QuestionResponseSetRepository : GenericRepository<QuestionResponseSet>, IQuestionResponseSetRepository
    {
        public QuestionResponseSetRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}