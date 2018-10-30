using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public interface IGenericRepository<T>
    {
        T Save(T entity);
        T GetById(int id);
        T Update(T entity);
        bool DeleteById(int id);
        bool BulkInsert(List<T> entityList);
        bool BulkDelete(List<T> entityList);
        IQueryable<T> GetAll();
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
    }
}