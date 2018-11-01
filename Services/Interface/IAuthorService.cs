using DtoLayer.Dto;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IAuthorService 
    {
        List<AuthorDto> GetAll();
    }
}