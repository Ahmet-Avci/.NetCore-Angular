using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Yeni kategori ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CategoryDto AddCategory(CategoryDto model)
        {
            return _mapper.Map<CategoryDto>(_categoryRepository.Save(_mapper.Map<CategoryEntity>(model)));    
        }

        /// <summary>
        /// Tüm kategorileri döndüren metod
        /// </summary>
        /// <returns>Liste tipinde categorydto döner</returns>
        public List<CategoryDto> GetAll()
        {
            return _mapper.Map<List<CategoryEntity>,List<CategoryDto>>(_categoryRepository.Filter(x=> !x.IsDeleted).ToList());
        }

        /// <summary>
        /// İlgili kategoriyi siler
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool RemoveCategory(int categoryId)
        {
            CategoryEntity categoryEntity = _categoryRepository.GetById(categoryId);
            categoryEntity.IsDeleted = true;
            return _categoryRepository.Update(categoryEntity).IsDeleted;
        }
    }
}
