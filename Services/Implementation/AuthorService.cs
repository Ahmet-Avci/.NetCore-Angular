using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;


        public AuthorService(IAuthorRepository authorRepository, IArticleRepository articleRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni Kullanıcı Ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthorDto AddUser(AuthorDto model)
        {
            model.IsActive = true;
            var value = _authorRepository.Save(_mapper.Map<AuthorDto, AuthorEntity>(model));
            return _mapper.Map<AuthorEntity, AuthorDto>(value);
        }

        /// <summary>
        /// Tüm kullanıcıları getirir
        /// </summary>
        /// <returns></returns>
        public List<AuthorDto> GetAll()
        {
            return _mapper.Map<List<AuthorEntity>, List<AuthorDto>>(_authorRepository.GetAll().ToList());
        }

        /// <summary>
        /// Parametredeki sayı kadar popüler yazar ve en popüler eserini getirir
        /// </summary>
        /// <param name="authorCount"></param>
        /// <returns></returns>
        public List<AuthorDto> GetPopularAuthor(int authorCount)
        {
            var authorsEntity = _authorRepository.GetAll().Join(_articleRepository.GetAll(),
            author => author.Id,
            article => article.CreatedBy,
            (author, article) => new { author, article }
            ).Where(x => x.article.CreatedBy == x.author.Id && x.author.AuthorType != DataBaseContext.Enums.AuthorType.admin).OrderByDescending(x => x.article.ReadCount).Select(x => x.author).Distinct().Take(authorCount).ToList();

            List<int> authorsIds = authorsEntity.Select(x => x.Id).ToList();
            var articlesEntity = _articleRepository.Filter(x => authorsIds.Contains(x.CreatedBy.Value)).OrderByDescending(x => x.ReadCount).Take(authorCount).ToList();

            List<AuthorDto> authors = new List<AuthorDto>();
            List<ArticleDto> articles = new List<ArticleDto>();

            authorsEntity.ForEach(author =>
            {
                author.ArticleList = new List<ArticleEntity>();
                articlesEntity.Where(article => article.CreatedBy == author.Id).ToList().ForEach(article =>
                {
                    article.Content = article.Content.Length > 240 ? string.Concat(article.Content.Substring(0, 240), "...") : article.Content;
                    article.ImagePath = string.IsNullOrWhiteSpace(article.ImagePath) ? "" : Encoding.UTF8.GetString(Convert.FromBase64String(article.ImagePath));
                    author.ArticleList.Add(article);
                });
                author.ModifiedDate = DateTime.Now;
                authors.Add(_mapper.Map<AuthorDto>(author));
            });
            return authors;
        }

        /// <summary>
        /// İlgili kullanıcıyının mail adresi ve şifresine göre kullanıcıyı getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthorDto GetUser(AuthorDto model)
        {
            var authorEntity = _authorRepository.Filter(x => x.MailAddress.Equals(model.MailAddress) && x.Password.Equals(model.Password) && !x.IsDeleted && x.IsActive).FirstOrDefault();
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

        /// <summary>
        /// Yazarın banını kaldırır
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
            var author = _mapper.Map<AuthorDto>(_authorRepository.GetById(authorId));
            var articles = _mapper.Map<List<ArticleDto>>(_articleRepository.Filter(x => x.CreatedBy == authorId).ToList());
            author.ArticleCount = articles.Count;
            author.TotalReadCount = articles.Sum(x => x.ReadCount);
            return author;
        }

        /// <summary>
        /// İlgili yazar'ı günceller
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthorDto EditAuthor(AuthorDto model)
        {
            var authorEntity = _authorRepository.GetById(model.Id);
            authorEntity.Name = model.Name;
            authorEntity.Surname = model.Surname;
            authorEntity.PhoneNumber = model.PhoneNumber;
            authorEntity.MailAddress = model.MailAddress;
            authorEntity.Autobiography = model.Autobiography;
            authorEntity.Image = !string.IsNullOrWhiteSpace(model.Image) ? model.Image : authorEntity.Image;
            var statusEntiy = _authorRepository.Update(authorEntity);

            return statusEntiy != null && statusEntiy.Id > 0
                ? model
                : new AuthorDto();
        }

        /// <summary>
        /// İlgili kişinin şifresini değiştirir
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthorDto ChangePasword(int id, string oldPassword, string password)
        {
            var authorEntity = _authorRepository.GetById(id);
            if (authorEntity.Password.Equals(oldPassword))
            {
                authorEntity.Password = password;
                return _mapper.Map<AuthorDto>(_authorRepository.Update(authorEntity));
            }
            else
            {
                return new AuthorDto();
            }
            
        }
    }
}