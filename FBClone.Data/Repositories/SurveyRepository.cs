using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface ISurveyRepository : IGenericRepository<Survey>
    {
    }

    public class SurveyRepository : GenericRepository<Survey>, ISurveyRepository
    {
        public SurveyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}