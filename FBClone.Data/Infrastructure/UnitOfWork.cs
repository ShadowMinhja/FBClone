using System;
using FBClone.Model;

namespace FBClone.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private FBCloneContext dbContext;
        private readonly IDatabaseFactory dbFactory;
        protected FBCloneContext DbContext
        {
            get
            {
                return dbContext ?? dbFactory.Get();
            }
        }

        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
