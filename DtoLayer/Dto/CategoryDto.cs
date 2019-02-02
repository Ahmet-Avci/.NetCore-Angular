using System.Collections.Generic;

namespace DtoLayer.Dto
{
    public class CategoryDto : BaseDto
    {
        /// <summary>
        /// İlgili kategorinin varsa üst id bilgisini tutar
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Kategorinin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kategorinin açıklaması
        /// </summary>
        public string Description { get; set; }

        public List<ArticleDto> Articles { get; set; }
    }
}
