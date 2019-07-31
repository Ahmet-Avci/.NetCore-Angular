using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System;
using System.Security.Cryptography;
using System.Text;

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
        public IActionResult Login(string MailAddress, string Password)
        {
            if (string.IsNullOrWhiteSpace(MailAddress) || string.IsNullOrWhiteSpace(Password))
                return Json(new { isNull = true, message = "Lütfen gerekli bilgileri doldurunuz." });

            HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            var result = _authorService.GetUser(MailAddress, Password);

            if (!result.IsNull && result.Data.Id > 0)
            {
                HttpContext.Session.SetObject("LoginUser", result.Data);
                return Ok(result);
            }
            else
            {
                return Json(new { isNull = true, message = "Giriş Bilgileriniz Hatalı." });
            }
        }

        /// <summary>
        /// Giriş yapılıp yapılmadığını kontrol eden metod
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult SessionControl()
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
        public IActionResult RegisterUser(AuthorDto model, string Password)
        {
            if (string.IsNullOrEmpty(model.MailAddress) || string.IsNullOrEmpty(Password))
                return Json(new { isNull = true, message = "Mail ve şifre alanları boş bırakılamaz." });

            string hashed = string.Empty;
            using (var sha = SHA1.Create())
            {
                var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(model.MailAddress.Substring(0, 4), Password)));
                hashed = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

            return Ok(_authorService.AddUser(model, hashed));
        }
    }
}