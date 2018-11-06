namespace DataBaseContext.Enums
{
    /// <summary>
    /// Kullanıcı tiplerini tutan enum
    /// </summary>
    public enum AuthorType
    {
        /// <summary>
        /// Sadece giriş yapmadan yazıları okuyabilecek kişi tipi
        /// </summary>
        voyager = 0,

        /// <summary>
        /// Üyeliğe sahip ama yazı yazmak istemeyen kullanıcı tipi
        /// </summary>
        bookworm = 1,

        /// <summary>
        /// Yazı yazıp paylaşabilen kullanıcı tipi
        /// </summary>
        author = 2,

        /// <summary>
        /// Admin kullanıcı tipi
        /// </summary>
        admin = 3,
    }
}
