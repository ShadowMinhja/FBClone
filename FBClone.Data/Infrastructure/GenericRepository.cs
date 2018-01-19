using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using FBClone.Data;
using FBClone.Model;

namespace FBClone.Data.Infrastructure
{
    public abstract class GenericRepository<T> where T : class, new()
    {
        private FBCloneContext dataContext;
        private readonly IDbSet<T> dbset;
        protected GenericRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected FBCloneContext DataContext
        {
            //get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
            get
            {
                if (dataContext != null)
                    return dataContext;
                else
                {
                    var dbContext = DatabaseFactory.Get();
                    //Optimization
                    //dbContext.Configuration.AutoDetectChangesEnabled = false; //Can't disable this until enable change tracking proxies
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    //dbContext.Configuration.ProxyCreationEnabled = false;
                    return dataContext = dbContext;
                }
            }
        }

        public virtual void Refresh(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).Reload();
        }

        public virtual T Add(T entity)
        {
            dbset.Add(entity);
            return entity;
        }
        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }
        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            //return dbset.ToList();
            return dbset.AsNoTracking().ToList(); //Optimization
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbset;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where);
        }
    }
}
