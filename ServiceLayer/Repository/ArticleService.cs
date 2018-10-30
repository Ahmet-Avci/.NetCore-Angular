using DbLayer;
using DbLayer.Entity;
using RepositoryLayer;

namespace ServiceLayer.Repository
{
    public class ArticleService : Repository<ArticleEntity>
    {
        public ArticleService(DataContext contextParam) : base(contextParam)
        {
        }
    }
}
