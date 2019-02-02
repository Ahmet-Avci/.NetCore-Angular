using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Collections.Generic;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllCategory()
        {
            List<CategoryDto> result = _categoryService.GetAll();

            return result != null && result.Count > 0
                ? (IActionResult)Ok(result)
                : NotFound("Kayit Bulunamadi...");
        }

        /// <summary>
        /// Yeni kategori ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult AddCategory(CategoryDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                return Json(new { isError = true, message = "Lütfen gerekli alanları doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return BadRequest("Giriş yapmadan kategori ekleyemezsiniz :(");

            model.CreatedBy = user.Id;
            CategoryDto categoryDto = _categoryService.AddCategory(model);

            return categoryDto != null && categoryDto.Id > 0
                ? (IActionResult)Ok(categoryDto)
                : NotFound("Kayit Bulunamadi...");
        }

        /// <summary>
        /// İlgili kategoriyi siler
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult RemoveCategory(int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("Lütfen kategori seçiniz...");

            bool isDeleted = _categoryService.RemoveCategory(categoryId);

            return isDeleted
                ? (IActionResult)Ok(isDeleted)
                : NotFound("Kayıt Silinemedi");
        }
    }
}