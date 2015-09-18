using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using WebService.Models;

namespace WebService.Infrastructure.Context
{
  
    public class HiRemoteMeetCortanaContext : DbContext, IHiRemoteMeetCortanaContext
    {
        public DbSet<Settings> Settings { get; set; }
        public HiRemoteMeetCortanaContext()
            : base("HiRemoteMeetCortanaContext")
        {
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public void Commit()
        {
            SaveChanges();
        }

        // Has no meaning in Entity Framework.
        public void Rollback()
        {
        }
        public DbContext ExtractDbContext()
        {
            return this;
        }
    }


}