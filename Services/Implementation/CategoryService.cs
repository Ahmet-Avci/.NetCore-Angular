using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataTransferObject.Dto;
using DbLayer.Entity;
using DtoLayer.Dto;
using Repository.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IArticleRepository articleRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Yeni kategori ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<CategoryDto> AddCategory(CategoryDto model)
        {
            return new Result<CategoryDto>(_mapper.Map<CategoryDto>(_categoryRepository.Save(_mapper.Map<CategoryEntity>(model))));    
        }

        /// <summary>
        /// Tüm kategorileri döndüren metod
        /// </summary>
        /// <returns>Liste tipinde categorydto döner</returns>
        public Result<List<CategoryDto>> GetAll()
        {
            List<CategoryDto> result = _mapper.Map<List<CategoryEntity>, List<CategoryDto>>(_categoryRepository.Filter(x => !x.IsDeleted).ToList());
            

            result.ForEach(x => {
                x.ArticleCount = _articleRepository.Filter(z => z.CategoryId == x.Id && z.IsActive && z.IsShare).Count();
            });

            return result != null && result.Count > 0
                ? new Result<List<CategoryDto>>(result)
                : new Result<List<CategoryDto>>();
        }

        /// <summary>
        /// İlgili kategoriyi siler işlem başarılıysa true değer dönrürür
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Result<bool> RemoveCategory(int categoryId)
        {
            CategoryEntity categoryEntity = _categoryRepository.GetById(categoryId);
            categoryEntity.IsDeleted = true;
            return new Result<bool>(_categoryRepository.Update(categoryEntity).IsDeleted);
        }
    }
}
