using System.Collections.Generic;
using DtoLayer.Dto;

namespace Services.Interface
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAll();
        CategoryDto AddCategory(CategoryDto model);
        bool RemoveCategory(int categoryId);
    }
}
