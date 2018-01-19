using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IQuestionDraftRepository : IGenericRepository<QuestionDraft>
    {
    }

    public class QuestionDraftRepository : GenericRepository<QuestionDraft>, IQuestionDraftRepository
    {
        public QuestionDraftRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}