using System.Collections.Generic;
using DataTransferObject.Dto;
using DtoLayer.Dto;

namespace Services.Interface
{
    public interface ICategoryService
    {
        Result<List<CategoryDto>> GetAll();
        Result<CategoryDto> AddCategory(CategoryDto model);
        Result<bool> RemoveCategory(int categoryId);
    }
}
