using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;

namespace DataTransferObject.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuthorEntity,AuthorDto>();
            CreateMap<AuthorDto, AuthorEntity>();
            CreateMap<ArticleEntity, ArticleDto>();
            CreateMap<ArticleDto, ArticleEntity>();
        }
    }
}
