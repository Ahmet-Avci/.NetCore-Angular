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

            if (user.Id > 0)
            {
                HttpContext.Session.SetObject("LoginUser", user);
                return Json(new { isError = false, route = "" });
            }
            else
            {
                return Json(new { isError = true, message = "Giriş Bilgileriniz Hatalı." });
            }
        }

        public JsonResult SessionControl()
        {
            var result = HttpContext.Session.GetObject<AuthorDto>("LoginUser");
            return Json(result != null ? result : new AuthorDto());
        }

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

        public PartialViewResult LoginScreen(bool loginCheckbox)
        {
            return loginCheckbox ? PartialView("Pages/Authentication/_LoginScreenPartial.cshtml") : PartialView("Pages/Authentication/_RegisterScreenPartial.cshtml"); ;
        }

    }
}