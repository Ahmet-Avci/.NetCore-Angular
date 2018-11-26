using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public ArticleDto AddArticle(ArticleDto model)
        {
            model.CreatedBy = 1;
            return _mapper.Map<ArticleEntity, ArticleDto>(_articleRepository.Save(_mapper.Map<ArticleDto, ArticleEntity>(model)));
        }
    }
}
