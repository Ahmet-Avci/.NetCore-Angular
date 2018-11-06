using DbLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DbLayer
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ArticleEntity> Article { get; set; }
        public DbSet<AuthorEntity> Author { get; set; }
        public DbSet<ArticleAuditEntity> ReadedArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
