using AutoMapper;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class ArticleAuditService : IArticleAuditService
    {
        private readonly IArticleAuditRepository _articleAuditRepository;
        private readonly IMapper _mapper;

        public ArticleAuditService(IArticleAuditRepository articleAuditRepository, IMapper mapper)
        {
            _articleAuditRepository = articleAuditRepository;
            _mapper = mapper;
        }

    }
}
