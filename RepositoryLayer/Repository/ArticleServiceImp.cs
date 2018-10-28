using DbLayer;
using DbLayer.Entity;
using RepositoryLayer;

namespace ServiceLayer.Repository
{
    public class ArticleServiceImp : Repository<ArticleEntity, int>
    {
        public ArticleServiceImp(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
