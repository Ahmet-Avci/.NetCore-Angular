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
                ? Json(new { isError = true, message = "Hen�z hi� eseriniz yok :(" })
                : Json(result);
        }

        /// <summary>
        /// Admin'in eklemi� oldu�u aktif eserleri getirir
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
        /// �lgili eser id'yi ait eseri, eser okunma ekran� i�in d�nd�r�r
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
        /// �lgili eseri d�nd�r�r
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetArticleByIdForEdit(int articleId)
        {
            if (articleId <= 0)
                return NotFound("L�tfen giri� bilgilerinizi kontrol ediniz...");

            ArticleDto result = _articleService.GetArticleById(articleId);

            return result != null && result.Id > 0
                ? (IActionResult)Ok(result)
                : NotFound("Eser getirilirken bir hata olu�tu");
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
                return BadRequest("L�tfen gerekli bilgileri doldurunuz...");

            List<ArticleDto> result = _articleService.GetFilterArticle(model);

            return result == null || result.Count <= 0
                ? NotFound("Kay�t Bulunamad�...")
                : (IActionResult)Ok(result);
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
                return BadRequest("Eser se�imi s�ras�nda hata meydana geldi...");

            bool result = _articleService.SetPassifeArticle(articleId);

            return result
                ? NotFound("Hata Olu�tu...")
                : (IActionResult)Ok(result);
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
                return BadRequest("Eser se�imi s�ras�nda hata meydana geldi...");

            bool result = _articleService.SetActiveArticle(articleId);

            return !result
                ? NotFound("Hata Olu�tu...")
                : (IActionResult)Ok(result);
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
                return BadRequest("Kategori se�imi s�ras�nda bir hata meydana geldi...");

            List<ArticleDto> result = _articleService.GetArticlesByCategoryId(categoryId, skipCount, takeCount);

            return result != null && result.Count > 0
                ? (IActionResult)Ok(result)
                : NotFound("Hata Olustu...");
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
            ArticleDto articleDto = _articleService.SetShareStatus(id, isShare);

            return articleDto != null && articleDto.IsShare
                ? (IActionResult)Ok(articleDto)
                : NotFound("Eser Yay�na Al�n�rken Hata Olu�tu...");
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
            ArticleDto articleDto = _articleService.SetShareStatus(id, isShare);

            return articleDto != null && !articleDto.IsShare
                ? (IActionResult)Ok(articleDto)
                : NotFound("Eser Yay�ndan Kald�r�l�rken Hata Olu�tu...");
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
                return NotFound("Giri� Bilgileriniz Hatal�...");

            var result = _articleService.RemoveArticleById(id);

            return result
                ? (IActionResult)Ok(result)
                : NotFound("Eser Silinirken Bir Hata Olu�tu...");
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
                return NotFound("Giri� Bilgileriniz Hatal�...");

            ArticleDto result = _articleService.UpdateArticle(model);

            return result != null
                ? (IActionResult)Ok(result)
                : NotFound("Eser g�ncellenirken bir hata ile kar��la��ld�");
        }

    }
}
