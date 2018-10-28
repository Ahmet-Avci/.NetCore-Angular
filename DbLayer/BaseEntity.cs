using System;

namespace DbLayer
{

    /// <summary>
    /// Bütün veritabanı sınıflarının ortak olarak kullanacağı kolonlar
    /// </summary>
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
