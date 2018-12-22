using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Collections.Generic;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthenticationController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Angular tarafından aldığı giriş bilgileri ile login işlemini gerçekleştirecek metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ActionResult Login(AuthorDto model)
        {
            if (string.IsNullOrWhiteSpace(model.MailAddress) && string.IsNullOrWhiteSpace(model.Password))
                return Json(new { isError = true, message = "Lütfen gerekli bilgileri doldurunuz." });

            var user = _authorService.GetUser(model);

            if (user.Id > 0)
            {
                HttpContext.Session.SetObject("LoginUser", user);
                return Json(new { isError = false, message = user });
            }
            else
            {
                return Json(new { isError = true, message = "Giriş Bilgileriniz Hatalı." });
            }
        }

        /// <summary>
        /// Giriş yapılıp yapılmadığını kontrol eden metod
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public JsonResult SessionControl()
        {
            var result = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            return Json(result != null ? result : new AuthorDto());
        }

        /// <summary>
        /// Session'ı temizleyen - çıkış yapan metod
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("LoginUser");
                return Json(true);
            }
            catch (System.Exception)
            {
                return Json(false);
            }
        }

        [HttpPost("[action]")]
        public JsonResult RegisterUser(AuthorDto model)
        {
            if (string.IsNullOrEmpty(model.MailAddress) || string.IsNullOrEmpty(model.Password))
                return Json(new { isError = true, message = "Mail ve şifre alanları boş bırakılamaz." });

            var result = _authorService.AddUser(model);

            return result != null && result.Id > 0
                ? Json(result)
                : Json(new { isError = true, message = "Kullanıcı Eklenemedi." });
        }

        [HttpPost("[action]")]
        public List<AuthorDto> GetTopAuthorArticle(int authorCount)
        {
            if (authorCount <= 0)
                return new List<AuthorDto>();

            return _authorService.GetPopularAuthor(authorCount);
        }

    }
}