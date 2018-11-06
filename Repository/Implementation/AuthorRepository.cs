using AutoMapper;
using DbLayer;
using DbLayer.Entity;

namespace Repository.Implementation
{

    public class AuthorRepository : GenericRepository<AuthorEntity>, IAuthorRepository
    {
        public AuthorRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
