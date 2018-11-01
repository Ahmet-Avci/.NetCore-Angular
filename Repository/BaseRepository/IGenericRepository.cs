using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Save(T entity);
        T GetById(long id);
        T Update(T entity);
        bool DeleteById(long id);
        bool BulkInsert(List<T> entityList);
        bool BulkDelete(List<T> entityList);
        IQueryable<T> GetAll();
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
    }
}