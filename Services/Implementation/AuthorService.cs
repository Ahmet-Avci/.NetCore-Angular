using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleAuditRepository _articleAuditRepository;
        private readonly IMapper _mapper;
        private static List<AuthorDto> HomePageData { get; set; }

        public AuthorService(IAuthorRepository authorRepository, IArticleRepository articleRepository, IArticleAuditRepository articleAuditRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _articleRepository = articleRepository;
            _articleAuditRepository = articleAuditRepository;
            _mapper = mapper;
        }

        public AuthorDto AddUser(AuthorDto model)
        {
            var value = _authorRepository.Save(_mapper.Map<AuthorDto, AuthorEntity>(model));
            return _mapper.Map<AuthorEntity, AuthorDto>(value);
        }

        public List<AuthorDto> GetAll()
        {
            return _mapper.Map<List<AuthorEntity>, List<AuthorDto>>(_authorRepository.GetAll().ToList());
        }

        public List<AuthorDto> GetPopularAuthor(int authorCount)
        {
            if (HomePageData == null || (DateTime.Now.AddDays(7) - HomePageData.FirstOrDefault().ModifiedDate.Value).Days >= 7)
            {
                var result = _authorRepository.GetAll().Join(_articleRepository.GetAll(),
                    author => author.Id,
                    article => article.CreatedBy,
                    (author, article) => new { author, article }).Where(x => x.author.Id == x.article.CreatedBy).Join(_articleAuditRepository.GetAll(),
                    authorArticle => authorArticle.article.Id,
                    articleAudit => articleAudit.ArticleId,
                    (authorArticle, articleAudit) => new { authorArticle, articleAudit }
                    ).Where(x => x.articleAudit.ArticleId == x.authorArticle.article.Id).OrderByDescending(x => x.articleAudit.ReadCount).ToList();

                var authorsEntity = result.Select(x => x.authorArticle.author).Distinct().Take(authorCount).ToList();
                var articlesEntity = result.Select(x => x.authorArticle.article).ToList();
                var articlesAuditEntity = result.Select(x => x.articleAudit).ToList();

                List<AuthorDto> authors = new List<AuthorDto>();
                List<ArticleDto> articles = new List<ArticleDto>();
                List<ArticleAuditDto> articlesAudit = new List<ArticleAuditDto>();

                authorsEntity.ForEach(x =>
                {
                    x.ArticleList = new List<ArticleEntity>();
                    articlesEntity.Where(z => z.CreatedBy == x.Id).ToList().ForEach(z =>
                      {
                          if (articles.Where(art => art.CreatedBy == x.CreatedBy).Count() == 1)
                              return;
                          z.Content = z.Content.Length > 240 ? string.Concat(z.Content.Substring(0, 240), "...") : z.Content;
                          z.ImagePath = string.IsNullOrWhiteSpace(z.ImagePath) ? "" : Encoding.UTF8.GetString(Convert.FromBase64String(z.ImagePath));
                          articles.Add(_mapper.Map<ArticleEntity, ArticleDto>(z));
                          z.ReadedArticle = new List<ArticleAuditEntity>();
                          articlesAuditEntity.Where(y => y.ArticleId == z.Id).ToList().ForEach(y =>
                          {
                              articlesAudit.Add(_mapper.Map<ArticleAuditEntity, ArticleAuditDto>(y));
                              z.ReadedArticle.Add(y);
                          });
                          x.ArticleList.Add(z);
                      });
                    x.ModifiedDate = DateTime.Now;
                    authors.Add(_mapper.Map<AuthorEntity, AuthorDto>(x));
                });
                HomePageData = authors;
                return HomePageData;
            }
            else
            {
                return HomePageData;
            }
        }

        /// <summary>
        /// İlgili kullanıcıyının mail adresi ve şifresine göre kullanıcıyı getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthorDto GetUser(AuthorDto model)
        {
            var authorEntity = _authorRepository.Filter(x => x.MailAddress.Equals(model.MailAddress) && x.Password.Equals(model.Password) && !x.IsDeleted).FirstOrDefault();
            return authorEntity != null ? _mapper.Map<AuthorEntity, AuthorDto>(authorEntity) : new AuthorDto();
        }

        /// <summary>
        /// Gelen data'ya göre ilgili kullanıcıyı aratır
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<AuthorDto> GetFilterAuthor(AuthorDto model)
        {
            List<AuthorEntity> authorList = new List<AuthorEntity>();

            if (!string.IsNullOrEmpty(model.Name) && string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                authorList = _authorRepository.Filter(x => x.Name.ToLower().StartsWith(model.Name.ToLower())).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && string.IsNullOrWhiteSpace(model.Name))
            {
                authorList = _authorRepository.Filter(x => x.PhoneNumber.ToLower().StartsWith(model.PhoneNumber.ToLower())).ToList();
            }
            else
            {
                authorList = _authorRepository.Filter(x => x.Name.ToLower().StartsWith(model.Name.ToLower()) && x.PhoneNumber.ToLower().StartsWith(model.PhoneNumber.ToLower())).ToList();
            }

            return _mapper.Map<List<AuthorEntity>, List<AuthorDto>>(authorList);
        }

        /// <summary>
        /// İlgili kullanıcıyı pasife alır
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SetPassifeAuthor(int userId)
        {
            AuthorEntity author = _authorRepository.GetById(userId);
            author.IsActive = false;
            AuthorEntity updated = _authorRepository.Update(author);
            return updated.IsActive;
        }

        public bool SetActiveAuthor(int userId)
        {
            AuthorEntity author = _authorRepository.GetById(userId);
            author.IsActive = true;
            AuthorEntity updated = _authorRepository.Update(author);
            return updated.IsActive;
        }

        /// <summary>
        /// İlgili id'ye ait kullanıcıyı döndürür
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public AuthorDto GetAuthorById(int authorId)
        {
            return _mapper.Map<AuthorDto>(_authorRepository.GetById(authorId));
        }
    }
}