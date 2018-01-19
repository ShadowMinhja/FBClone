using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IAnswerDraftRepository : IGenericRepository<AnswerDraft>
    {
    }

    public class AnswerDraftRepository : GenericRepository<AnswerDraft>, IAnswerDraftRepository
    {
        public AnswerDraftRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}