using System.Data.Entity;
using YanZhiwei.DotNet.MVC.AdminPanel.Config;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet4.Framework.Data;

namespace YanZhiwei.DotNet.MVC.AdminPanel.DAL
{
    public class AdminPanelDbContext : DbContextBase
    {
        public AdminPanelDbContext()
            : base(CachedConfigContext.Current.DaoConfig.AdminPanel, null)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdminPanelDbContext>(null);

            //modelBuilder.Entity<Article>()
            //    .HasMany(e => e.Tags)
            //    .WithMany(e => e.Articles)
            //    .Map(m =>
            //    {
            //        m.ToTable("ArticleTag");
            //        m.MapLeftKey("ArticleId");
            //        m.MapRightKey("TagId");
            //    });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}