using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IMenuSectionRepository : IGenericRepository<MenuSection>
    {
    }

    public class MenuSectionRepository : GenericRepository<MenuSection>, IMenuSectionRepository
    {
        public MenuSectionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}