using System.Collections.Generic;

namespace DbLayer.Entity
{
    /// <summary>
    /// Yazarların yazacağı yazılar ile ilgili bilgileri tutan sınıf
    /// </summary>
    public class ArticleEntity : BaseEntity
    {
        /// <summary>
        /// Yazının İçeriği
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Yazının bağlı olduğu kategori id'si
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Yazının Başlığı
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Yazı ile ilgili resmin yolu
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Eserin yayında olup olmadığına bakar
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// Eserin yazarının bulunduğu model
        /// </summary>
        public virtual AuthorEntity Author { get; set; }

        /// <summary>
        /// Eser detaylarının bulunduğua model
        /// </summary>
        public virtual List<ArticleAuditEntity> ReadedArticle { get; set; }
    }
}
