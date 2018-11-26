using DtoLayer.Dto;

namespace Services.Interface
{
    public interface IArticleService
    {
        ArticleDto AddArticle(ArticleDto model);
    }
}
