using DbLayer;
using DbLayer.Entity;
using RepositoryLayer;

namespace ServiceLayer
{
    public class AuthorService : Repository<AuthorEntity>
    {
        public AuthorService(DataContext contextParam) : base(contextParam)
        {
        }
    }
}
