using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
    }

    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}