using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Collections.Generic;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;


        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Yeni eser ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult AddArticle(ArticleDto model)
        {
            if (string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.Header))
                return Json(new { isError = true, message = "Lütfen gerekli alanlarý doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return Json(new { isError = true, message = "Giriþ yapmadan yazý ekleyemezsiniz :(" });

            model.CreatedBy = user.Id;
            var articleDto = _articleService.AddArticle(model);

            return articleDto != null && articleDto.Id > 0
                ? Json(articleDto)
                : Json(new ArticleDto());
        }

        /// <summary>
        /// Login olmuþ olan kiþiye ait eserleri getirir
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult GetArticlesByAuthorId()
        {
            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id <= 0)
                return Json(new { isError = true, message = "Lütfen giriþ yapýnýz.." });

            var result = _articleService.GetArticlesByAuthorId(user);

            return result == null || result.Id <= 0
                ? Json(new { isError = true, message = "Bir hata oluþtu." })
                : Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleCount"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult GetArticleByAdmin(int articleCount)
        {
            if (articleCount <= 0)
                return Json(new { isError = true, message = "Hata Oluþtu" });

            List<ArticleDto> result = _articleService.GetArticleByAdmin(articleCount);

            return result != null && result.Count > 0
                ? Json(result)
                : Json(new { isError = true, message = "Editörün seçtiði bir eser bulunamadý..." });

        }

        /// <summary>
        /// Ýlgili eser id'yi ait eseri döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult GetArticleById(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isError = true, message = "Hata Oluþtu" });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            int userId = user == null ? -1 : user.Id;
            ArticleDto article = _articleService.GetArticleWithAuthor(articleId, userId);

            return article != null && article.Id > 0
                ? Json(article)
                : Json(new { isError = true, message = "Ýlgili eser getirilirken hata oluþtu" });
        }

        /// <summary>
        /// Ýlgili parametreler ile uyaþan eserleri getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetFilterArticle(ArticleDto model)
        {
            if (model == null || (string.IsNullOrWhiteSpace(model.Header) && string.IsNullOrWhiteSpace(model.Content)))
                return BadRequest("Lütfen gerekli bilgileri doldurunuz...");

            List<ArticleDto> result = _articleService.GetFilterArticle(model);

            return result == null || result.Count <= 0
                ? NotFound("Kayýt Bulunamadý...")
                : (IActionResult)Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetPassifeArticle(int articleId)
        {
            if (articleId <= 0)
                return BadRequest("Eser seçimi sýrasýnda hata meydana geldi...");

            bool result = _articleService.SetPassifeArticle(articleId);

            return result
                ? NotFound("Hata Oluþtu...")
                : (IActionResult)Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetActiveArticle(int articleId)
        {
            if (articleId <= 0)
                return BadRequest("Eser seçimi sýrasýnda hata meydana geldi...");

            bool result = _articleService.SetActiveArticle(articleId);

            return !result
                ? NotFound("Hata Oluþtu...")
                : (IActionResult)Ok(result);
        }


    }
}
