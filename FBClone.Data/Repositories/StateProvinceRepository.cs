using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IStateProvinceRepository : IGenericRepository<StateProvince>
    {
    }

    public class StateProvinceRepository : GenericRepository<StateProvince>, IStateProvinceRepository
    {
        public StateProvinceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}