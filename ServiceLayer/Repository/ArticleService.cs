using DbLayer;
using DbLayer.Entity;
using RepositoryLayer;

namespace ServiceLayer.Repository
{
    public class ArticleService : Repository<ArticleEntity, int>
    {
        public ArticleService(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
