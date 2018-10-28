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
        /// Yazının Başlığı
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Yazı ile ilgili resmin yolu
        /// </summary>
        public string ImagePath { get; set; }

        public virtual AuthorEntity Author { get; set; }
    }
}
