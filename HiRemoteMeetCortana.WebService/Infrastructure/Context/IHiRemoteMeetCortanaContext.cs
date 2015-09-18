using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Infrastructure.Context
{
    public interface IHiRemoteMeetCortanaContext
    {

        System.Data.Entity.Database Database { get; }
        int SaveChanges();
        DbEntityEntry Entry(object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<Settings> Settings { get; set; }
        DbContext ExtractDbContext();
    }
}