using AutoMapper;
using DataTransferObject.Dto;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni eser ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<ArticleDto> AddArticle(ArticleDto model)
        {
            var result = _mapper.Map<ArticleEntity, ArticleDto>(_articleRepository.Save(_mapper.Map<ArticleDto, ArticleEntity>(model)));
            return new Result<ArticleDto>(result);
        }

        /// <summary>
        /// Tüm eserleri getirir
        /// </summary>
        /// <returns></returns>
        public Result<List<ArticleDto>> GetAllArticles()
        {
            var result = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.IsActive && x.IsShare && !x.IsDeleted).ToList());
            return result != null && result.Count > 0
                ? new Result<List<ArticleDto>>(result)
                : new Result<List<ArticleDto>>();
        }

        /// <summary>
        /// Adminlerin parametredeki sayı kadar eklemiş olduğu son aktif ve paylaşımdaki  eserleri getirir
        /// <param name="articleCount"></param>
        /// <returns></returns>
        public Result<List<ArticleDto>> GetArticleByAdmin(int articleCount)
        {
            List<int> adminIds = _authorRepository.Filter(x => x.AuthorType == DataBaseContext.Enums.AuthorType.admin).Select(x => x.Id).ToList();
            var result = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => adminIds.Contains(x.CreatedBy.Value) && x.IsActive && x.IsShare).OrderByDescending(x => x.CreatedDate).Take(articleCount).ToList());
            var resultIds = result.Select(x => x.Id).ToList();

            return result != null && result.Count > 0
                ? new Result<List<ArticleDto>>(result)
                : new Result<List<ArticleDto>>();
        }

        /// <summary>
        /// İlgili eseri döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public Result<ArticleDto> GetArticleById(int articleId)
        {
            var result = _mapper.Map<ArticleDto>(_articleRepository.GetById(articleId));
            return new Result<ArticleDto>(result);
        }

        /// <summary>
        /// Yazar id'sine göre tüm eserleri getirir
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Result<AuthorDto> GetArticlesByAuthorId(AuthorDto author)
        {
            var articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.CreatedBy.Value == author.Id && !x.IsDeleted).ToList());
            var articleIdList = articleList.Select(x => x.Id).ToList();

            if (articleList == null || articleList.Count <= 0)
                return new Result<AuthorDto>() { Message = "Yazar bilgisi ya da eser bilgisi bulunamadı..." };
            author.ArticleList = articleList;
            return new Result<AuthorDto>(author);
        }

        /// <summary>
        /// Kategoriye ait olan eserleri getirir. Eserler pagging mantığı ile gelmektedir.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Result<List<ArticleDto>> GetArticlesByCategoryId(int categoryId, int skipCount, int takeCount)
        {
            var resultQuery = _authorRepository.GetAll().Join(_articleRepository.GetAll(),
                author => author.Id,
                article => article.CreatedBy,
                (author, article) => new { author, article }).Where(x => x.article.IsShare && x.article.CategoryId == categoryId && x.author.IsActive);

            var articleList = _mapper.Map<List<ArticleDto>>(resultQuery.Select(x => x.article).OrderByDescending(x => x.CreatedDate).Skip(skipCount).Take(takeCount).ToList());
            var authorList = _mapper.Map<List<AuthorDto>>(resultQuery.Select(x => x.author).ToList());

            if ((authorList == null || authorList.Count <= 0) || (articleList == null || articleList.Count <= 0))
            {
                return new Result<List<ArticleDto>>();
            }

            authorList.ForEach(x =>
            {
                articleList.Where(z => z.CreatedBy == x.Id).ToList().ForEach(z =>
                  {
                      z.Content = z.Content.Length > 190 ? string.Concat(z.Content.Substring(0, 190), "...") : z.Content;
                      z.AuthorName = x.Name;
                      z.AuthorSurname = x.Surname;
                  });
            });

            return articleList != null && articleList.Count > 0
                ? new Result<List<ArticleDto>>(articleList)
                : new Result<List<ArticleDto>>();
        }

        /// <summary>
        /// Eseri eser sahibinin bilgileri ile birlikte getirir
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public Result<ArticleDto> GetArticleWithAuthor(int articleId, int authorId)
        {
            // Eser ile birlikte eser sahibide getiriliyor
            var article = _articleRepository.GetById(articleId);

            //Eserin okunduğuna dair audit tablosuna kayıt düşülüyor
            if (article != null && article.Id > 0 && article.ModifiedBy != authorId)
            {
                article.ReadCount++;
                article.ModifiedBy = authorId;
                article.ModifiedDate = DateTime.Now;
                _articleRepository.Update(article);

            }
            var articleDto = _mapper.Map<ArticleDto>(article);
            var author = _mapper.Map<AuthorDto>(_authorRepository.Filter(x => x.Id == article.CreatedBy).FirstOrDefault());
            articleDto.AuthorName = author.Name;
            articleDto.AuthorSurname = author.Surname;

            return new Result<ArticleDto>(articleDto);
        }

        /// <summary>
        /// İlgili parametreler ile uyan eserleri döndürür
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<List<ArticleDto>> GetFilterArticle(ArticleDto model)
        {
            List<ArticleDto> articleList = new List<ArticleDto>();

            if (!string.IsNullOrEmpty(model.Header) && string.IsNullOrWhiteSpace(model.Content))
            {
                articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.Header.StartsWith(model.Header)).ToList());
            }
            else if (!string.IsNullOrEmpty(model.Content) && string.IsNullOrWhiteSpace(model.Header))
            {
                articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.Content.Contains(model.Content)).ToList());

            }
            else
            {
                articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.Header.StartsWith(model.Header) && x.Content.Contains(model.Content)).ToList());
            }

            return articleList != null && articleList.Count > 0
                ? new Result<List<ArticleDto>>(articleList)
                : new Result<List<ArticleDto>>();
        }

        /// <summary>
        /// İlgili eseri siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<bool> RemoveArticleById(int id)
        {
            ArticleEntity article = _articleRepository.GetById(id);
            article.IsDeleted = true;
            return new Result<bool>(_articleRepository.Update(article).IsDeleted);
        }

        /// <summary>
        /// Eseri yayına almak için aktif eder ve işlem başarılıysa true değer döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public Result<bool> SetActiveArticle(int articleId)
        {
            ArticleEntity articleEntity = _articleRepository.GetById(articleId);
            articleEntity.IsShare = true;
            articleEntity.ModifiedDate = DateTime.Now;
            return new Result<bool>(_articleRepository.Update(articleEntity).IsShare);
        }

        /// <summary>
        /// Eseri yayından kaldırmak için pasife alır ve işlem başarılıysa false değer döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public Result<bool> SetPassifeArticle(int articleId)
        {
            ArticleEntity articleEntity = _articleRepository.GetById(articleId);
            articleEntity.IsShare = false;
            articleEntity.ModifiedDate = DateTime.Now;
            return new Result<bool>(_articleRepository.Update(articleEntity).IsShare);
        }

        /// <summary>
        /// Eseri yayına alır veya yayından kaldırır
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isShare"></param>
        /// <returns></returns>
        public Result<ArticleDto> SetShareStatus(int id, bool isShare)
        {
            var result = _articleRepository.GetById(id);
            if (result == null)
                return new Result<ArticleDto>();

            result.IsShare = isShare;
            var articleDto = _mapper.Map<ArticleDto>(_articleRepository.Update(result));

            return articleDto != null && articleDto.IsShare == isShare
                ? new Result<ArticleDto>(articleDto)
                : new Result<ArticleDto>();
        }

        /// <summary>
        /// İlgili eseri günceller
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<ArticleDto> UpdateArticle(ArticleDto model)
        {
            var articleEntity = _articleRepository.GetById(model.Id);
            articleEntity.Header = model.Header;
            articleEntity.Content = model.Content;
            articleEntity.ImagePath = !string.IsNullOrWhiteSpace(model.ImagePath) ? model.ImagePath : articleEntity.ImagePath;
            articleEntity.ModifiedDate = DateTime.Now;
            var result = _mapper.Map<ArticleDto>(_articleRepository.Update(articleEntity));
            return new Result<ArticleDto>(result);
        }
    }
}
