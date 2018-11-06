using DataBaseContext.Enums;
using System.Collections.Generic;

namespace DbLayer.Entity
{
    /// <summary>
    /// Yazarlar ve kullanıcıları tutar
    /// </summary>
    public class AuthorEntity : BaseEntity
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

        /// <summary>
        /// Kullanıcının giriş yapacağı şifresi
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// İlgili kullanıcının tipi
        /// </summary>
        public AuthorType AuthorType { get; set; }

        /// <summary>
        /// Yazarın eserlerinin bulunduğu model
        /// </summary>
        public virtual List<ArticleEntity> ArticleList { get; set; }

        /// <summary>
        /// Eser detaylarının bulunduğua model
        /// </summary>
        public virtual List<ArticleAuditEntity> ReadedArticle { get; set; }
    }
}
