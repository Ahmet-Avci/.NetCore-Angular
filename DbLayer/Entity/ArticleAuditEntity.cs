namespace DbLayer.Entity
{
    public class ArticleAuditEntity : BaseEntity
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

        public virtual AuthorEntity Author { get; set; }
        public virtual ArticleEntity Article { get; set; }
    }
}
