using System.Collections.Generic;
using System.Linq;
using DbLayer.Entity;
using Repository;
using Services.Interface;

namespace Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<AuthorEntity> GetAll()
        {
            return _authorRepository.GetAll().ToList();
        }
    }
}
