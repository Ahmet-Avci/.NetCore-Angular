namespace DataBaseContext.Enums
{
    public enum FilterType
    {
        /// <summary>
        /// yazar adına göre arama işlemi
        /// </summary>
        AuthorName = 0,

        /// <summary>
        /// yazar telefon numarasına göre arama işlemi
        /// </summary>
        AuthorPhoneNumber = 1,

        /// <summary>
        /// yazar Ad ve telefon numarasına göre arama işlemi
        /// </summary>
        AuthorNameAndNumber = 2,

        /// <summary>
        /// eser adına göre arama işlemi
        /// </summary>
        ArticleName = 3,

        /// <summary>
        /// eser içeriğine göre arama işlemi
        /// </summary>
        ArticleContent = 4,

        /// <summary>
        /// eser içeriğine ve adına göre arama işlemi
        /// </summary>
        ArticleNameAndContent = 5
    }
}
