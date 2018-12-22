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
        private readonly IArticleAuditRepository _articleAuditRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IArticleAuditRepository articleAuditRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _articleAuditRepository = articleAuditRepository;
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
            return _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.GetAll().ToList());
        }

        /// <summary>
        /// Adminlerin eklemiş olduğu eserleri parametredeki sayı kadarını son eklenen eserler kadar getirir
        /// </summary>
        /// <param name="articleCount"></param>
        /// <returns></returns>
        public List<ArticleDto> GetArticleByAdmin(int articleCount)
        {
            List<int> adminIds = _authorRepository.Filter(x => x.AuthorType == DataBaseContext.Enums.AuthorType.admin).Select(x => x.Id).ToList();
            var result = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => adminIds.Contains(x.CreatedBy.Value)).OrderByDescending(x => x.CreatedDate).Take(articleCount).ToList());
            var resultIds = result.Select(x => x.Id).ToList();
            var auditList = _articleAuditRepository.Filter(x => resultIds.Contains(x.ArticleId));
            result.ForEach(x =>
            {
                var auditReadCount = auditList.Where(z => z.ArticleId == x.Id).FirstOrDefault();
                x.ReadCount = auditReadCount != null ? auditReadCount.ReadCount : 0;
            });

            return result;
        }

        /// <summary>
        /// Yazar id'sine göre eserleri getirir
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public AuthorDto GetArticlesByAuthorId(AuthorDto author)
        {
            var articleList = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(_articleRepository.Filter(x => x.CreatedBy.Value == author.Id).ToList());
            var articleIdList = articleList.Select(x => x.Id).ToList();
            var articleAuditList = _mapper.Map<List<ArticleAuditEntity>, List<ArticleAuditDto>>(_articleAuditRepository.Filter(x => articleIdList.Contains(x.ArticleId)).ToList());

            articleList.ForEach(x =>
            {
                var articleAudit = articleAuditList.Where(z => z.ArticleId == x.Id).FirstOrDefault();
                if (articleAudit != null)
                    x.ReadCount = articleAuditList.Where(z => z.ArticleId == x.Id).FirstOrDefault().ReadCount;
            });

            if (articleList == null || articleList.Count <= 0)
                return new AuthorDto();
            author.ArticleList = articleList;
            return author;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public ArticleDto GetArticleWithAuthor(int articleId, int authorId)
        {
            // Eser ile birlikte eser sahibide getiriliyor
            var article = _mapper.Map<ArticleEntity, ArticleDto>(_articleRepository.GetById(articleId));
            var author = _mapper.Map<AuthorEntity, AuthorDto>(_authorRepository.Filter(x => x.Id == article.CreatedBy).FirstOrDefault());
            article.AuthorName = author.Name;
            article.AuthorSurname = author.Surname;

            //Eserin okunduğuna dair audit tablosuna kayıt düşülüyor
            var articleAudit = _articleAuditRepository.Filter(x => x.ArticleId == articleId).FirstOrDefault();

            if (articleAudit != null && articleAudit.Id > 0 && articleAudit.ModifiedBy != authorId)
            {
                articleAudit.ReadCount++;
                articleAudit.ModifiedBy = authorId;
                articleAudit.ModifiedDate = DateTime.Now;
                _articleAuditRepository.Update(articleAudit);

                article.ReadCount = articleAudit.ReadCount;
            }
            else if (articleAudit == null)
            {
                var articleAuditResult = _articleAuditRepository.Save(new ArticleAuditEntity()
                {
                    CreatedBy = authorId,
                    ModifiedBy = authorId,
                    ReadCount = 1,
                    ArticleId = articleId
                });
            }

            return article;
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
    }
}
