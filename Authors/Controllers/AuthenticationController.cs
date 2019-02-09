using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

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
        public ActionResult Login(string MailAddress, string Password)
        {
            if (string.IsNullOrWhiteSpace(MailAddress) && string.IsNullOrWhiteSpace(Password))
                return Json(new { isError = true, message = "Lütfen gerekli bilgileri doldurunuz." });

            var user = _authorService.GetUser(MailAddress, Password);

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
        public JsonResult RegisterUser(AuthorDto model, string Password)
        {
            if (string.IsNullOrEmpty(model.MailAddress) || string.IsNullOrEmpty(Password))
                return Json(new { isError = true, message = "Mail ve şifre alanları boş bırakılamaz." });

            var result = _authorService.AddUser(model, Password);

            return result != null && result.Id > 0
                ? Json(result)
                : Json(new { isError = true, message = "Kullanıcı Eklenemedi." });
        }
    }
}