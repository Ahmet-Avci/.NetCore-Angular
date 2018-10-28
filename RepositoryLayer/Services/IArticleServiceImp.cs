using DbLayer.Entity;
using RepositoryLayer;

namespace ServiceLayer.Services
{
    public interface IArticleServiceImp : IRepository<ArticleEntity,int>
    {
    }
}