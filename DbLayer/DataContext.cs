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

        public DbSet<CategoryEntity> Category { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AuthorEntity>()
                .HasMany(a => a.ArticleList)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.CreatedBy);

            builder.Entity<ArticleEntity>()
                .HasMany(a => a.ReadedArticle)
                .WithOne(a => a.Article)
                .HasForeignKey(a => a.ArticleId);

        }

        
    }
}
