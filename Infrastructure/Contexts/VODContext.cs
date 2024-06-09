
using Common.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace Database.Contexts
{
    public class VODContext : IdentityDbContext<VODUser>
    {
        #region DbSet Properties

        public DbSet<Download> Downloads { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Media>  medias { get; set; }

        public DbSet<Topic> topics { get; set; } 

        public DbSet<TopicType> topicTypes { get; set; }

        public DbSet<UserTopic> userTopics { get; set; }
        #endregion

        #region Constructor
        public VODContext(DbContextOptions<VODContext> options) : base(options)
        {
        }
        #endregion

        #region Overrides
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserTopic>().HasKey(uc => new { uc.UserId, uc.TopicId });
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        #endregion
    }
}
