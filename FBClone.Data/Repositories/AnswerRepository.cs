using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
    }

    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}