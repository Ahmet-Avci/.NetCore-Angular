using DtoLayer.Dto;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IAuthorService 
    {
        List<AuthorDto> GetAll();
        AuthorDto GetUser(AuthorDto model);
        AuthorDto AddUser(AuthorDto model);
        List<AuthorDto> GetPopularAuthor(int authorCount);
        List<AuthorDto> GetFilterAuthor(AuthorDto model);
        bool SetPassifeAuthor(int userId);
        bool SetActiveAuthor(int userId);
        AuthorDto GetAuthorById(int authorId);
        AuthorDto EditAuthor(AuthorDto model);
        AuthorDto ChangePasword(int id, string oldPassword, string password);
    }
}