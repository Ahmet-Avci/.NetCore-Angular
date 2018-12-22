using DbLayer;
using DbLayer.Entity;
using Repository.Interface;

namespace Repository.Implementation
{
    public class CategoryRepository : GenericRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
