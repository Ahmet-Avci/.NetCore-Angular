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
    /// Eserlerle ilgili iþlemleri kontrol eden sýnýf
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
                return Json(new { isNull = true, message = "Lütfen gerekli alanlarý doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return Json(new { isNull = true, message = "Giriþ yapmadan yazý ekleyemezsiniz :(" });

            model.CreatedBy = user.Id;
            return Ok(_articleService.AddArticle(model));
        }

        /// <summary>
        /// Login olmuþ olan kiþiye ait eserleri getirir
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticlesByAuthorId()
        {
            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id <= 0)
                return Json(new { isNull = true, message = "Giriþ yapmadan bu sayfaya eriþemezsiniz :(" });

            return Ok(_articleService.GetArticlesByAuthorId(user));
        }



        /// <summary>
        /// Ýlgili eser id'yi ait eseri, eser okunma ekraný için döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleById(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Beklenmedik bir hata oluþtu :(" });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            int userId = user == null ? -1 : user.Id;
            return Ok(_articleService.GetArticleWithAuthor(articleId, userId));
        }

        /// <summary>
        /// Ýlgili eseri döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleByIdForEdit(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Lütfen giriþ bilgilerinizi kontrol ediniz." });

            return Ok(_articleService.GetArticleById(articleId));
        }

        /// <summary>
        /// Ýlgili parametreler ile uyaþan eserleri getirir. Admin filtreleme ekraný için
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetFilterArticle(ArticleDto model)
        {
            if (model == null || (string.IsNullOrWhiteSpace(model.Header) && string.IsNullOrWhiteSpace(model.Content)))
                return Json(new { isNull = true, message = "Lütfen gerekli bilgileri doldurunuz." });

            return Ok(_articleService.GetFilterArticle(model));
        }

        /// <summary>
        /// Ýlgili eseri pasife alýr
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetPassifeArticle(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Eser seçimi sýrasýnda bir hata meydana geldi :(" });

            return Ok(_articleService.SetPassifeArticle(articleId));
        }

        /// <summary>
        /// Ýlgili eseri aktife alýr
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetActiveArticle(int articleId)
        {
            if (articleId <= 0)
                return Json(new { isNull = true, message = "Eser seçimi sýrasýnda bir hata meydana geldi :(" });

            return Ok(_articleService.SetActiveArticle(articleId));
        }

        /// <summary>
        /// Kategori id'ye ait olan aktif ve paylaþýmdaki eserleri paging mantýðý ile getirir
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleByCategoryId(int categoryId, int skipCount, int takeCount)
        {
            if (categoryId <= 0)
                return Json(new { isNull = true, message = "Kategori seçimi sýrasýnda bir hata meydana geldi :(" });

            return Ok(_articleService.GetArticlesByCategoryId(categoryId, skipCount, takeCount));
        }

        /// <summary>
        /// Ýlgili eseri paylaþýma alýr
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
        /// Ýlgili eseri paylaþýmdan kaldýrýr
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
        /// Ýlgili eseri siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult DeleteArticle(int id)
        {
            if (id <= 0)
                return Json(new { isNull = true, message = "Beklenmedik bir hata oluþtu :(" });

            return Ok(_articleService.RemoveArticleById(id));
        }

        /// <summary>
        /// Ýlgili eseri günceller
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult UpdateArticle(ArticleDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Content) || string.IsNullOrWhiteSpace(model.Header))
                return Json(new { isNull = true, message = "Hata oluþtu. Girdiðiniz bilgiler eksik olabilir :(" });

            return Ok(_articleService.UpdateArticle(model));
        }

    }
}
