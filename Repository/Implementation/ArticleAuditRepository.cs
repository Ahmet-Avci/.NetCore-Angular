using DbLayer;
using DbLayer.Entity;
using Repository.Interface;

namespace Repository.Implementation
{
    public class ArticleAuditRepository : GenericRepository<ArticleAuditEntity>, IArticleAuditRepository
    {
        public ArticleAuditRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
