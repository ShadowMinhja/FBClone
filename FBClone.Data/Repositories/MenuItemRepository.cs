using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
    }

    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}