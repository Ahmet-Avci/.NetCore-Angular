using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace Authors.Controllers
{
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
        public ActionResult Login(AuthorDto model)
        {
            if (string.IsNullOrWhiteSpace(model.MailAddress) && string.IsNullOrWhiteSpace(model.Password))
                return Json(new { isError = true, message = "Lütfen gerekli bilgileri doldurunuz." });

            var user = _authorService.GetUser(model);

            if (user != null)
            {
                HttpContext.Session.SetObject("LoginUser", user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Json(new { isError = true, message = "Giriş Bilgileriniz Hatalı." });
            }
        }


    }
}