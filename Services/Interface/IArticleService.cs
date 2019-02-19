using System.Collections.Generic;
using DataTransferObject.Dto;
using DtoLayer.Dto;

namespace Services.Interface
{
    public interface IArticleService
    {
        Result<ArticleDto> AddArticle(ArticleDto model);
        Result<AuthorDto> GetArticlesByAuthorId(AuthorDto author);
        Result<List<ArticleDto>> GetAllArticles();
        Result<List<ArticleDto>> GetArticleByAdmin(int articleCount);
        Result<ArticleDto> GetArticleWithAuthor(int articleId, int authorId);
        Result<List<ArticleDto>> GetFilterArticle(ArticleDto model);
        Result<bool> SetPassifeArticle(int articleId);
        Result<bool> SetActiveArticle(int articleId);
        Result<List<ArticleDto>> GetArticlesByCategoryId(int categoryId, int skipCount, int takeCount);
        Result<ArticleDto> SetShareStatus(int id, bool isShare);
        Result<bool> RemoveArticleById(int id);
        Result<ArticleDto> GetArticleById(int articleId);
        Result<ArticleDto> UpdateArticle(ArticleDto model);
    }
}
