using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IHubConnectionRepository : IGenericRepository<HubConnection>
    {
    }

    public class HubConnectionRepository : GenericRepository<HubConnection>, IHubConnectionRepository
    {
        public HubConnectionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}