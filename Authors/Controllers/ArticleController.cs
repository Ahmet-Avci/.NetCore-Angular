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
                return Json(new { isError = true, message = "L�tfen gerekli alanlar� doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return Json(new { isError = true, message = "Giri� yapmadan yaz� ekleyemezsiniz :(" });

            model.CreatedBy = user.Id;
            var articleDto = _articleService.AddArticle(model);

            return articleDto != null && articleDto.Id > 0
                ? Json(articleDto)
                : Json(new ArticleDto());
        }

        /// <summary>
        /// Login olmu� olan ki�iye ait eserleri getirir
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult GetArticlesByAuthorId()
        {
            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id <= 0)
                return Json(new { isError = true, message = "L�tfen giri� yap�n�z.." });

            var result = _articleService.GetArticlesByAuthorId(user);

            return result == null || result.Id <= 0
                ? Json(new { isError = true, message = "Bir hata olu�tu." })
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
                return Json(new { isError = true, message = "Hata Olu�tu" });

            List<ArticleDto> result = _articleService.GetArticleByAdmin(articleCount);

            return result != null && result.Count > 0
                ? Json(result)
                : Json(new { isError = true, message = "Edit�r�n se�ti�i bir eser bulunamad�..." });

        }

        /// <summary>
        /// �lgili eser id'yi ait eseri d�nd�r�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult GetArticleById(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isError = true, message = "Hata Olu�tu" });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            int userId = user == null ? -1 : user.Id;
            ArticleDto article = _articleService.GetArticleWithAuthor(articleId, userId);

            return article != null && article.Id > 0
                ? Json(article)
                : Json(new { isError = true, message = "�lgili eser getirilirken hata olu�tu" });
        }

        /// <summary>
        /// �lgili parametreler ile uya�an eserleri getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetFilterArticle(ArticleDto model)
        {
            if (model == null || (string.IsNullOrWhiteSpace(model.Header) && string.IsNullOrWhiteSpace(model.Content)))
                return BadRequest("L�tfen gerekli bilgileri doldurunuz...");

            List<ArticleDto> result = _articleService.GetFilterArticle(model);

            return result == null || result.Count <= 0
                ? NotFound("Kay�t Bulunamad�...")
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
                return BadRequest("Eser se�imi s�ras�nda hata meydana geldi...");

            bool result = _articleService.SetPassifeArticle(articleId);

            return result
                ? NotFound("Hata Olu�tu...")
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
                return BadRequest("Eser se�imi s�ras�nda hata meydana geldi...");

            bool result = _articleService.SetActiveArticle(articleId);

            return !result
                ? NotFound("Hata Olu�tu...")
                : (IActionResult)Ok(result);
        }


    }
}
