using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System;
using System.Collections.Generic;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetFilterAuthors(AuthorDto model)
        {
            if (model == null || (string.IsNullOrWhiteSpace(model.Name) && string.IsNullOrWhiteSpace(model.PhoneNumber)))
                return Json(new { isNull = true, message = "Lutfen gerekli alanlari doldurunuz." });

            return Ok(_authorService.GetFilterAuthor(model));
        }

        /// <summary>
        /// İlgili kullanıcıyı döndürür
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetAuthorById(int authorId)
        {
            if (authorId <= 0)
                return Json(new { isNull = true, message = "Lutfen gerekli alanlari doldurunuz." });

            return Ok(_authorService.GetAuthorById(authorId));
        }

        [HttpPost("[action]")]
        public IActionResult EditAuthor(AuthorDto model)
        {
            if (model.Id <= 0 || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                return Json(new { isNull = true, message = "Lutfen gerekli alanlari doldurunuz." });

            return Ok(_authorService.EditAuthor(model));
        }

        /// <summary>
        /// İlgili kişinin şifresini değiştirir
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult ChangePassword(int id, string oldPassword, string password)
        {
            return Ok(_authorService.ChangePasword(id, oldPassword, password));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetPassifeAuthor(int userId)
        {
            if (userId <= 0)
                return Json(new { isNull = true, message = "Malesef beklenmedik bir hata oluştu :(" });

            return Ok(_authorService.SetPassifeAuthor(userId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult SetActiveAuthor(int userId)
        {
            if (userId <= 0)
                return Json(new { isNull = true, message = "Malesef beklenmedik bir hata oluştu :(" });

            return Ok(_authorService.SetActiveAuthor(userId));
        }

    }
}