using DbLayer;
using RepositoryLayer;

namespace ServiceLayer
{
    public class AuthorService : Repository<AuthorEntity, int>
    {
        public AuthorService(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
