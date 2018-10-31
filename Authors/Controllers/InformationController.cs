using System.Collections.Generic;
using System.Linq;
using DbLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class InformationController : Controller
    {
        private readonly IAuthorService _authorService;

        public InformationController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("[action]")]
        public List<AuthorEntity> GetAllAuthors()
        {
            return _authorService.GetAll().ToList();
        }
    }
}