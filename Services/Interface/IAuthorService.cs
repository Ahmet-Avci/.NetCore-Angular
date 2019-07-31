using DataTransferObject.Dto;
using DtoLayer.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAuthorService 
    {
        Result<List<AuthorDto>> GetAll();
        Result<AuthorDto> GetUser(string MailAddress, string Password);
        Result<AuthorDto> AddUser(AuthorDto model, string Password);
        Task<Result<List<AuthorDto>>> GetPopularAuthor(int authorCount);
        Result<List<AuthorDto>> GetFilterAuthor(AuthorDto model);
        Result<bool> SetPassifeAuthor(int userId);
        Result<bool> SetActiveAuthor(int userId);
        Result<AuthorDto> GetAuthorById(int authorId);
        Result<AuthorDto> EditAuthor(AuthorDto model);
        Result<AuthorDto> ChangePasword(int id, string oldPassword, string password);
    }
}