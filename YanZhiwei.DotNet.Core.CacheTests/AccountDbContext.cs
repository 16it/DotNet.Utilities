using System.Data.Entity;
using YanZhiwei.DotNet.Core.CacheTests.Model;
using YanZhiwei.DotNet.Framework.Data;

namespace YanZhiwei.DotNet.Core.CacheTests
{
    public class AccountDbContext : DbContextBase<int>
    {
        public AccountDbContext()
        : base(@"Data Source=KEDE-YANZHIWEI\SQLEXPRESS;Initial Catalog=GMSAccount;Persist Security Info=True;User ID=sa;Password=sasa", null)
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AccountDbContext>(null);
            modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Users)
            .Map(m =>
            {
                m.ToTable("UserRole");
                m.MapLeftKey("UserID");
                m.MapRightKey("RoleID");
            });
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<LoginInfo> LoginInfos
        {
            get;
            set;
        }
        public DbSet<User> Users
        {
            get;
            set;
        }
        public DbSet<Role> Roles
        {
            get;
            set;
        }
    }
}