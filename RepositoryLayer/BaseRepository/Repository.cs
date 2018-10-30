using System.Collections.Generic;
using System.Linq;
using DbLayer;

namespace RepositoryLayer
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {

        private readonly DataContext context;

        public Repository(DataContext contextParam)
        {
            context = contextParam;
        }

        public virtual bool BulkInsert(List<T> modelList)
        {
            var Table = context.Set<T>();
            Table.AddRange(modelList);
            return Save();
        }

        public virtual bool Delete(T model)
        {
            var Table = context.Set<T>();
            Table.Remove(model);
            return Save();
        }

        public virtual bool DeleteById(int id)
        {
            var Table = context.Set<T>();
            var result = Table.Find(id);
            Table.Remove(result);
            return Save();
        }

        public virtual List<T> GetAll()
        {
            var Table = context.Set<T>();
            return Table.ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T Insert(T model)
        {
            var Table = context.Set<T>();
            var result = Table.Add(model);
            Save();
            return result.Entity;
        }

        public virtual T Update(T model)
        {
            var Table = context.Set<T>();
            var result = Table.Update(model);
            Save();
            return result.Entity;
        }

        private bool Save()
        {
            try
            {
                context.SaveChanges();
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
