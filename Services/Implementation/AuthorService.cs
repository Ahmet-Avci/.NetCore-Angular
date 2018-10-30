using System.Collections.Generic;
using System.Linq;
using DbLayer.Entity;
using Repository;
using Services.Interface;

namespace Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorService _authorService;

        public AuthorService(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public List<AuthorEntity> GetAll()
        {
            return _authorService.GetAll();
        }
    }
}
