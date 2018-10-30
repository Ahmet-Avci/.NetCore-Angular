using DbLayer.Entity;
using Repository;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IAuthorService 
    {
        List<AuthorEntity> GetAll();
    }
}
