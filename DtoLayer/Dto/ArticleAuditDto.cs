using DtoLayer;
using DtoLayer.Dto;

namespace DtoLayer.Dto
{
    public class ArticleAuditDto : BaseDto
    {
        /// <summary>
        /// Eser id'sini tutar
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// İlgili eserin pasif/aktif bilgisini tutar
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Eserin okunma sayısı
        /// </summary>
        public int ReadCount { get; set; }


        public virtual AuthorDto Author { get; set; }
        public virtual ArticleDto Article { get; set; }
    }
}
