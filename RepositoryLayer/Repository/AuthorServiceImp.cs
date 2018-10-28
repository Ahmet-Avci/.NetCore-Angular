using DbLayer;
using RepositoryLayer;

namespace ServiceLayer
{
    public class AuthorServiceImp : Repository<AuthorEntity, int>
    {
        public AuthorServiceImp(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
