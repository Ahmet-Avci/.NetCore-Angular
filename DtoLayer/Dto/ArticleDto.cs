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

        /// <summary>
        /// İlgili eserin bağlı olduğu kategori
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Eserin yayında olup olmadığına bakar
        /// </summary>
        public bool IsShare { get; set; }

        public virtual AuthorDto Author { get; set; }

        //Dto Areas
        public int ReadCount { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        /// <summary>
        /// Eser detaylarının bulunduğua model
        /// </summary>
        public virtual List<ArticleAuditDto> ReadedArticle { get; set; }
    }
}
