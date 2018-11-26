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
        /// İlgili eserin okunma bilgisini tutar
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Eserin beğenilme bilgisini tutar
        /// </summary>
        public bool IsLike { get; set; }

        /// <summary>
        /// İlgili eserin favorilere alınma bilgisini tutar
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// İlgili eserin pasif/aktif bilgisini tutar
        /// </summary>
        public bool IsActive { get; set; }

        public virtual AuthorDto Author { get; set; }
        public virtual ArticleDto Article { get; set; }
    }
}
