using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Interface;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("[action]")]
        public List<ArticleDto> GetAllArticle()
        {
            List<ArticleDto> result = _articleService.GetAllArticles();

            return result != null && result.Count > 0
                ? result
                : new List<ArticleDto>();
        }
    }
}