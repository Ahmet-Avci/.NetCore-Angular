using DbLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly DataContext _dbContext;
        private readonly DbSet<T> _dbSet;
        

        protected GenericRepository(DataContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = _dbContext.Set<T>();
        }

        public bool BulkDelete(List<T> entityList)
        {
            _dbSet.RemoveRange(entityList);
            return Save();
        }

        public bool BulkInsert(List<T> entityList)
        {
            _dbSet.AddRange(entityList);
            return Save();
        }

        public bool DeleteById(long id)
        {
            var entity = GetById(id);
            _dbSet.Remove(entity);
            return Save();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public T Save(T entity)
        {
            var value = _dbSet.Add(entity).Entity;
            Save();
            return value;
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            Save();
            return entity;
        }

        public bool Save()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
