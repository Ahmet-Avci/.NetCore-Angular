using AutoMapper;
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
        public ArticleDto AddArticle(ArticleDto model)
        {
            return _mapper.Map<ArticleEntity, ArticleDto>(_articleRepository.Save(_mapper.Map<ArticleDto, ArticleEntity>(model)));
        }

        /// <summary>
        /// Tüm eserleri getirir
        /// </summary>
        /// <returns></returns>
        public List<ArticleDto> GetAllArticles()
        {
            return _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.IsActive && x.IsShare && !x.IsDeleted).ToList());
        }

        /// <summary>
        /// Adminlerin parametredeki sayı kadar eklemiş olduğu son aktif ve paylaşımdaki  eserleri getirir
        /// <param name="articleCount"></param>
        /// <returns></returns>
        public List<ArticleDto> GetArticleByAdmin(int articleCount)
        {
            List<int> adminIds = _authorRepository.Filter(x => x.AuthorType == DataBaseContext.Enums.AuthorType.admin).Select(x => x.Id).ToList();
            var result = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => adminIds.Contains(x.CreatedBy.Value) && x.IsActive && x.IsShare).OrderByDescending(x => x.CreatedDate).Take(articleCount).ToList());
            var resultIds = result.Select(x => x.Id).ToList();

            return result;
        }

        /// <summary>
        /// İlgili eseri döndürür
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public ArticleDto GetArticleById(int articleId)
        {
            return _mapper.Map<ArticleDto>(_articleRepository.GetById(articleId));
        }

        /// <summary>
        /// Yazar id'sine göre tüm eserleri getirir
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public AuthorDto GetArticlesByAuthorId(AuthorDto author)
        {
            var articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.CreatedBy.Value == author.Id && !x.IsDeleted).ToList());
            var articleIdList = articleList.Select(x => x.Id).ToList();

            if (articleList == null || articleList.Count <= 0)
                return new AuthorDto();
            author.ArticleList = articleList;
            return author;
        }

        /// <summary>
        /// Kategoriye ait olan eserleri getirir. Eserler pagging mantığı ile gelmektedir.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<ArticleDto> GetArticlesByCategoryId(int categoryId, int skipCount, int takeCount)
        {
            var resultQuery = _authorRepository.GetAll().Join(_articleRepository.GetAll(),
                author => author.Id,
                article => article.CreatedBy,
                (author, article) => new { author, article }).Where(x => x.article.IsShare && x.article.CategoryId == categoryId && x.author.IsActive);

            var articleList = _mapper.Map<List<ArticleDto>>(resultQuery.Select(x => x.article).OrderByDescending(x => x.CreatedDate).Skip(skipCount).Take(takeCount).ToList());
            var authorList = _mapper.Map<List<AuthorDto>>(resultQuery.Select(x => x.author).ToList());

            if ((authorList == null || authorList.Count <= 0) || (articleList == null || articleList.Count <= 0))
            {
                return new List<ArticleDto>();
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

            return articleList;
        }

        /// <summary>
        /// Eseri eser sahibinin bilgileri ile birlikte getirir
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public ArticleDto GetArticleWithAuthor(int articleId, int authorId)
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

            return articleDto;
        }

        /// <summary>
        /// İlgili parametreler ile uyan eserleri döndürür
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ArticleDto> GetFilterArticle(ArticleDto model)
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

            return articleList;
        }

        /// <summary>
        /// İlgili eseri siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveArticleById(int id)
        {
            ArticleEntity article = _articleRepository.GetById(id);
            article.IsDeleted = true;
            return _articleRepository.Update(article).IsDeleted;
        }

        /// <summary>
        /// Eseri yayına almak için aktif eder
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool SetActiveArticle(int articleId)
        {
            ArticleEntity articleEntity = _articleRepository.GetById(articleId);
            articleEntity.IsShare = true;
            articleEntity.ModifiedDate = DateTime.Now;
            return _articleRepository.Update(articleEntity).IsShare;
        }

        /// <summary>
        /// Eseri yayından kaldırmak için pasife alır
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool SetPassifeArticle(int articleId)
        {
            ArticleEntity articleEntity = _articleRepository.GetById(articleId);
            articleEntity.IsShare = false;
            articleEntity.ModifiedDate = DateTime.Now;
            return _articleRepository.Update(articleEntity).IsShare;
        }

        /// <summary>
        /// Eseri yayına alır veya yayından kaldırır
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isShare"></param>
        /// <returns></returns>
        public ArticleDto SetShareStatus(int id, bool isShare)
        {
            var result = _articleRepository.GetById(id);
            if (result == null)
                return new ArticleDto();

            result.IsShare = isShare;
            var articleDto = _mapper.Map<ArticleDto>(_articleRepository.Update(result));

            return articleDto != null && articleDto.IsShare == isShare
                ? articleDto
                : new ArticleDto();
        }

        /// <summary>
        /// İlgili eseri günceller
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ArticleDto UpdateArticle(ArticleDto model)
        {
            var articleEntity = _articleRepository.GetById(model.Id);
            articleEntity.Header = model.Header;
            articleEntity.Content = model.Content;
            articleEntity.ImagePath = !string.IsNullOrWhiteSpace(model.ImagePath) ? model.ImagePath : articleEntity.ImagePath;
            articleEntity.ModifiedDate = DateTime.Now;
            return _mapper.Map<ArticleDto>(_articleRepository.Update(articleEntity));
        }
    }
}
