using DbLayer;
using DbLayer.Entity;
using Repository.Interface;

namespace Repository.Implementation
{
    public class ArticleRepository : GenericRepository<ArticleEntity>, IArticleRepository
    {
        public ArticleRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
