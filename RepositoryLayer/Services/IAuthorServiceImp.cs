using DbLayer;
using RepositoryLayer;

namespace ServiceLayer.Services
{
    public interface IAuthorServiceImp : IRepository<AuthorEntity,int>
    {
    }
}
