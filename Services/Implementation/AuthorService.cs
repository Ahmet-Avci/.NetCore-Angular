using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository;
using Services.Interface;

namespace Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository,IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public List<AuthorDto> GetAll()
        {
            return _mapper.Map<List<AuthorEntity>,List<AuthorDto>>(_authorRepository.GetAll().ToList());
        }
    }
}
