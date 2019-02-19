using Authors.Helpers;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

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
            return Ok(_categoryService.GetAll());
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
                return Json(new { isNull = true, message = "Lütfen gerekli alanları doldurunuz." });

            var user = HttpContext.Session.GetObject<AuthorDto>("LoginUser");

            if (user == null || user.Id < 0)
                return Json(new { isNull = true, message = "Giriş yapmadan kategori ekleyemezsiniz :(" });

            model.CreatedBy = user.Id;
            return Ok(_categoryService.AddCategory(model));
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
                return Json(new { isNull = true, message = "Lütfen kategori seçiniz..." });

            return Ok(_categoryService.RemoveCategory(categoryId));
        }
    }
}