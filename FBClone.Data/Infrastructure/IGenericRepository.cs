using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FBClone.Data.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(string id);
        T GetById(long id);
        IEnumerable<T> GetAll();
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        void Refresh(T entity);

        T Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }

}
