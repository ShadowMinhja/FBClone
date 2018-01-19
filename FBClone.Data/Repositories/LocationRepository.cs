using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
    }

    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}