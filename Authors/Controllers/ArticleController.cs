using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace Authors.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public JsonResult AddArticle(ArticleDto model)
        {
            if (string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.Header))
                return Json(new { isError = true, message = "Lütfen gerekli alanlarý doldurunuz." });

            var articleDto = _articleService.AddArticle(model);

            return articleDto != null && articleDto.Id > 0
                ? Json(articleDto)
                : Json(new ArticleDto());
        }
        
    }
}
