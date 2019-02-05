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
                return BadRequest("Lutfen gerekli alanlari doldurunuz...");

            List<AuthorDto> result = _authorService.GetFilterAuthor(model);

            return result != null && result.Count > 0
                ? (IActionResult)Ok(result)
                : NotFound("");
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
                return BadRequest("Lutfen gerekli alanlari doldurunuz...");

            AuthorDto author = _authorService.GetAuthorById(authorId);

            return author != null && author.Id > 0
                ? (IActionResult)Ok(author)
                : NotFound("Kayit Bulunamadi");
        }

        [HttpPost("[action]")]
        public IActionResult EditAuthor(AuthorDto model)
        {
            if (model.Id <= 0 || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                return BadRequest("Lütfen gerekli alanları doldurunuz...");

            AuthorDto author = _authorService.EditAuthor(model);

            return author != null && author.Id > 0
                ? (IActionResult)Ok(author)
                : NotFound("Kayıt güncellenirken hata oluştu...");
        }

        /// <summary>
        /// İlgili kişinin şifresini değiştirir
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult ChangePassword(int id, string oldPassword, string password)
        {
            AuthorDto author = _authorService.ChangePasword(id, oldPassword, password);

            return author != null && author.Id > 0
                ? (IActionResult)Ok(author)
                : NotFound("Şifre değiştirilemedi...");
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
                return BadRequest(new Exception("İstek Hatalı!"));

            bool isSuccess = _authorService.SetPassifeAuthor(userId);

            return !isSuccess
                ? (IActionResult)Ok(isSuccess)
                : NotFound("Silme işlemi başarısızlıkla sonuçlandı");
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
                return BadRequest(new Exception("İstek Hatalı!"));

            bool isSuccess = _authorService.SetActiveAuthor(userId);

            return isSuccess
                ? (IActionResult)Ok(isSuccess)
                : NotFound("Aktife alma işlemi başarısız...");
        }
        
    }
}