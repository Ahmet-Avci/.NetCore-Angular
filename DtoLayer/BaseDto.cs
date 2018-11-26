using System;

namespace DtoLayer
{
    /// <summary>
    /// Data Transfer Object base class'ıdır.
    /// </summary>
    public class BaseDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}
