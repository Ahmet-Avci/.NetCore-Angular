using System;
using System.Collections.Generic;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Interface;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private IMemoryCache _cache;
        private readonly IArticleService _articleService;
        private readonly IAuthorService _authorService;
        public HomeController(IArticleService articleService, IAuthorService authorService, IMemoryCache cache)
        {
            _articleService = articleService;
            _authorService = authorService;
            _cache = cache;
        }

        /// <summary>
        /// Tüm eserleri getirir
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public List<ArticleDto> GetAllArticle()
        {
            List<ArticleDto> result = _articleService.GetAllArticles();

            return result != null && result.Count > 0
                ? result
                : new List<ArticleDto>();
        }

        /// <summary>
        /// En popüler yazarları ve eserlerini cacheleyerek getirir
        /// </summary>
        /// <param name="authorCount"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public List<AuthorDto> GetTopAuthorArticle(int authorCount)
        {
            if (authorCount <= 0)
                return new List<AuthorDto>();


            if (_cache.TryGetValue("TopAuthor", out List<AuthorDto> authors))
            {
                return authors;
            }
            else
            {
                var cacheEntry = _authorService.GetPopularAuthor(authorCount);
                var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set("TopAuthor", cacheEntry, cacheEntryOption);

                return cacheEntry;
            }
        }

        /// <summary>
        /// Admin'in eklemiş olduğu aktif eserleri cacheleyerek getirir
        /// </summary>
        /// <param name="articleCount"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public List<ArticleDto> GetArticleByAdmin(int articleCount)
        {
            if (articleCount <= 0)
                new ArticleDto();

            if (_cache.TryGetValue("AdminArticles", out List<ArticleDto> articles))
            {
                return articles;
            }
            else
            {
                var cacheEntry = _articleService.GetArticleByAdmin(articleCount);
                var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.Set("AdminArticles", cacheEntry, cacheEntryOption);

                return cacheEntry;
            }

            //List<ArticleDto> result = _articleService.GetArticleByAdmin(articleCount);

            //return result != null && result.Count > 0
            //    ? Json(result)
            //    : Json(new { isError = true, message = "Editörün seçtiği bir eser bulunamadı..." });

        }
    }
}