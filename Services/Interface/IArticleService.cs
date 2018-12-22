using System.Collections.Generic;
using DtoLayer.Dto;

namespace Services.Interface
{
    public interface IArticleService
    {
        ArticleDto AddArticle(ArticleDto model);
        AuthorDto GetArticlesByAuthorId(AuthorDto author);
        List<ArticleDto> GetAllArticles();
        List<ArticleDto> GetArticleByAdmin(int articleCount);
        ArticleDto GetArticleWithAuthor(int articleId, int authorId);
        List<ArticleDto> GetFilterArticle(ArticleDto model);
        bool SetPassifeArticle(int articleId);
        bool SetActiveArticle(int articleId);
    }
}
