using Microsoft.EntityFrameworkCore;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<News> News => Set<News>();
        public DbSet<NewsImage> NewsImages => Set<NewsImage>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<NewsReaction> NewsReactions => Set<NewsReaction>();
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PasswordResetToken> PasswordResetTokens
    => Set<PasswordResetToken>();

        public DbSet<EmailVerificationToken> EmailVerificationTokens
    => Set<EmailVerificationToken>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<NewsletterSubscriber> NewsletterSubscribers
    => Set<NewsletterSubscriber>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<NewsTag> NewsTags => Set<NewsTag>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDBContext).Assembly);
        }
    }
}
