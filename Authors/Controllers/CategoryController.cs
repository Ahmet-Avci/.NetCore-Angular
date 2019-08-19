using Authors.Helpers;
using DataTransferObject.Dto;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Interface;
using System;
using System.Collections.Generic;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private IMemoryCache _cache;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IMemoryCache cache)
        {
            _categoryService = categoryService;
            _cache = cache;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllCategory()
        {
            if (_cache.TryGetValue("Categories", out Result<List<CategoryDto>> categories))
            {
                return Ok(categories);
            }
            else
            {
                var cacheEntry = _categoryService.GetAll();
                var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(6));
                _cache.Set("Categories", cacheEntry, cacheEntryOption);

                return Ok(cacheEntry);
            }
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