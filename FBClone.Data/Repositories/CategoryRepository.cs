using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}