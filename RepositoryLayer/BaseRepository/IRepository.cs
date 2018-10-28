using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        DbSet<TEntity> Table { get; }
        TEntity Insert(TEntity model);
        TEntity Update(TEntity model);
        bool Delete(TEntity model);
        bool DeleteById(TId id);
        List<TEntity> GetAll();
        TEntity GetById(TId id);
        bool BulkInsert(List<TEntity> modelList);
    }
}