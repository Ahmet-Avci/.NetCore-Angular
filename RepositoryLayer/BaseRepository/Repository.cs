using System.Collections.Generic;
using System.Linq;
using DbLayer;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer
{
    public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        public DbSet<TEntity> Table { get; set; }
        private readonly DataContext dbContext;

        public Repository(DataContext dbContext)
        {
            this.dbContext = dbContext;
            this.Table = dbContext.Set<TEntity>();
        }

        public virtual bool BulkInsert(List<TEntity> modelList)
        {
            Table.AddRange(modelList);
            return Save();
        }

        public virtual bool Delete(TEntity model)
        {
            Table.Remove(model);
            return Save();
        }

        public virtual bool DeleteById(TId id)
        {
            var result = Table.Find(id);
            Table.Remove(result);
            return Save();
        }

        public virtual List<TEntity> GetAll()
        {
            return Table.ToList();
        }

        public virtual TEntity GetById(TId id)
        {
            return Table.Find(id);
        }

        public virtual TEntity Insert(TEntity model)
        {
            var result = Table.Add(model);
            Save();
            return result.Entity;
        }

        public virtual TEntity Update(TEntity model)
        {
            var result = Table.Update(model);
            Save();
            return result.Entity;
        }

        private bool Save()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                // TODO: Hatalar log'lanmalı...
                return false;
            }
        }
    }
}
