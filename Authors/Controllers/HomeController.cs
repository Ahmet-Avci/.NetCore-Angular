using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataTransferObject.Dto;
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
        public IActionResult GetAllArticle()
        {
            return Ok(_articleService.GetAllArticles());
        }

        /// <summary>
        /// En popüler yazarları ve eserlerini cacheleyerek getirir
        /// </summary>
        /// <param name="authorCount"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopAuthorArticle(int authorCount = 3)
        {
            if (authorCount <= 0)
                return Json(new { isNull = true, message = "Ana sayfa'da bir hata oluştu. Lütfen site yöneticisine başvurun. :(" });

            if (_cache.TryGetValue("TopAuthor", out Result<List<AuthorDto>> authors))
            {
                return Ok(authors);
            }
            else
            {
                var cacheEntry = await _authorService.GetPopularAuthor(authorCount);
                var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(6));
                _cache.Set("TopAuthor", cacheEntry, cacheEntryOption);

                return Ok(cacheEntry);
            }
        }

        /// <summary>
        /// Admin'in eklemiş olduğu aktif eserleri cacheleyerek getirir
        /// </summary>
        /// <param name="articleCount"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult GetArticleByAdmin(int articleCount = 4)
        {
            if (articleCount <= 0)
                return Json(new { isNull = true, message = "Ana sayfa'da bir hata oluştu. Lütfen site yöneticisine başvurun. :(" });

            if (_cache.TryGetValue("AdminArticles", out Result<List<ArticleDto>> articles))
            {
                return Ok(articles);
            }
            else
            {
                var cacheEntry = _articleService.GetArticleByAdmin(articleCount);
                var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(6));
                _cache.Set("AdminArticles", cacheEntry, cacheEntryOption);

                return Ok(cacheEntry);
            }
            
        }
    }
}