using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IAspNetUserRepository : IGenericRepository<AspNetUser>
    {
    }

    public class AspNetUserRepository : GenericRepository<AspNetUser>, IAspNetUserRepository
    {
        public AspNetUserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}