using System;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IRepository<T> where T : class
    {
        T Insert(T model);
        T Update(T model);
        bool Delete(T model);
        bool DeleteById(int id);
        List<T> GetAll();
        T GetById(int id);
        bool BulkInsert(List<T> modelList);
    }
}