namespace DbLayer.Entity
{
    public class ArticleAuditEntity : BaseEntity
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

        public virtual AuthorEntity Author { get; set; }
        public virtual ArticleEntity Article { get; set; }
    }
}
