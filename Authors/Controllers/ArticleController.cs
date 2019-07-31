using Authors.Extensions;
using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Collections.Generic;

namespace Authors.Controllers
{

    /// <summary>
    /// Eserlerle ilgili i�lemleri kontrol eden s�n�f
    /// </summary>
    [Route("api/[controller]")]
    public class ArticleController : SpeacialController
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
        [LoginControlAttribute]
        public IActionResult AddArticle(ArticleDto model)
        {
            if (string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.Header))
                return Json(new { isNull = true, message = "L�tfen gerekli alanlar� doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return Json(new { isNull = true, message = "Giri� yapmadan yaz� ekleyemezsiniz :(" });

            model.CreatedBy = user.Id;
            return Ok(_articleService.AddArticle(model));
        }

        /// <summary>
        /// Login olmu� olan ki�iye ait eserleri getirir
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticlesByAuthorId()
        {
            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id <= 0)
                return Json(new { isNull = true, message = "Giri� yapmadan bu sayfaya eri�emezsiniz :(" });

            return Ok(_articleService.GetArticlesByAuthorId(user));
        }



        /// <summary>
        /// �lgili eser id'yi ait eseri, eser okunma ekran� i�in d�nd�r�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleById(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Beklenmedik bir hata olu�tu :(" });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            int userId = user == null ? -1 : user.Id;
            return Ok(_articleService.GetArticleWithAuthor(articleId, userId));
        }

        /// <summary>
        /// �lgili eseri d�nd�r�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleByIdForEdit(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "L�tfen giri� bilgilerinizi kontrol ediniz." });

            return Ok(_articleService.GetArticleById(articleId));
        }

        /// <summary>
        /// �lgili parametreler ile uya�an eserleri getirir. Admin filtreleme ekran� i�in
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetFilterArticle(ArticleDto model)
        {
            if (model == null || (string.IsNullOrWhiteSpace(model.Header) && string.IsNullOrWhiteSpace(model.Content)))
                return Json(new { isNull = true, message = "L�tfen gerekli bilgileri doldurunuz." });

            return Ok(_articleService.GetFilterArticle(model));
        }

        /// <summary>
        /// �lgili eseri pasife al�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetPassifeArticle(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Eser se�imi s�ras�nda bir hata meydana geldi :(" });

            return Ok(_articleService.SetPassifeArticle(articleId));
        }

        /// <summary>
        /// �lgili eseri aktife al�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetActiveArticle(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Eser se�imi s�ras�nda bir hata meydana geldi :(" });

            return Ok(_articleService.SetActiveArticle(articleId));
        }

        /// <summary>
        /// Kategori id'ye ait olan aktif ve payla��mdaki eserleri paging mant��� ile getirir
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleByCategoryId(int categoryId, int skipCount, int takeCount)
        {
            if (categoryId <= 0)
                return Json(new { isNull = true, message = "Kategori se�imi s�ras�nda bir hata meydana geldi :(" });

            return Ok(_articleService.GetArticlesByCategoryId(categoryId, skipCount, takeCount));
        }

        /// <summary>
        /// �lgili eseri payla��ma al�r
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isShare"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult ShareArticle(int id, bool isShare)
        {
            return Ok(_articleService.SetShareStatus(id, isShare));
        }

        /// <summary>
        /// �lgili eseri payla��mdan kald�r�r
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isShare"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult UnShareArticle(int id, bool isShare)
        {
            return Ok(_articleService.SetShareStatus(id, isShare));
        }

        /// <summary>
        /// �lgili eseri siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult DeleteArticle(int id)
        {
            if (id <= 0)
                return Json(new { isNull = true, message = "Beklenmedik bir hata olu�tu :(" });

            return Ok(_articleService.RemoveArticleById(id));
        }

        /// <summary>
        /// �lgili eseri g�nceller
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult UpdateArticle(ArticleDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Content) || string.IsNullOrWhiteSpace(model.Header))
                return Json(new { isNull = true, message = "Hata olu�tu. Girdi�iniz bilgiler eksik olabilir :(" });

            return Ok(_articleService.UpdateArticle(model));
        }

    }
}
