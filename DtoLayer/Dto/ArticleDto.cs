using System.Collections.Generic;

namespace DtoLayer.Dto
{
    public class ArticleDto : BaseDto
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

        public virtual AuthorDto Author { get; set; }

        /// <summary>
        /// Eser detaylarının bulunduğua model
        /// </summary>
        public virtual List<ArticleAuditDto> ReadedArticle { get; set; }
    }
}
