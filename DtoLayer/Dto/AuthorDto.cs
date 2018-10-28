using System.Collections.Generic;

namespace DtoLayer.Dto
{
    /// <summary>
    /// Yazarlar ve kullanıcıları tutar
    /// </summary>
    public class AuthorDto : BaseDto
    {

        /// <summary>
        /// Kullanıcıya ait eserin id'si
        /// </summary>
        public int? ArticleId { get; set; }

        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kullanıcı Soyadı
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Kullanıcı Telefon Numarası
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Kullanıcı mail adresi
        /// </summary>
        public string MailAddress { get; set; }

        public virtual List<ArticleDto> ArticleList { get; set; }
    }
}
