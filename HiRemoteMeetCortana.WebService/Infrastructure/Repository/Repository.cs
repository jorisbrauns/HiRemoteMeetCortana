using EntityFramework.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebService.Infrastructure.Context;
using WebService.Infrastructure.DataAccess;

namespace WebService.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : Entity, IObjectWithState
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly DbSet<TEntity> _EntitySet;
        private readonly IHiRemoteMeetCortanaContext _HiRemoteMeetCortanaContext;
        private readonly IDeleteManager _DeleteManager;

        public Repository(IUnitOfWork unitOfWork, IHiRemoteMeetCortanaContext context, IDeleteManager deleteManager)
        {
            _DeleteManager = deleteManager;
            _HiRemoteMeetCortanaContext = context;
            _UnitOfWork = unitOfWork;
            _EntitySet = _HiRemoteMeetCortanaContext.Set<TEntity>();

            _HiRemoteMeetCortanaContext
                .ExtractDbContext()
                .EnableFilter("SoftDelete");
        }


        public TEntity Get(long id)
        {
            return _EntitySet.AsNoTracking().SingleOrDefault(c => c.Id == id);
        }

        public TEntity Get(string includePath, long id)
        {
            return _EntitySet.Include(includePath)
                             .AsNoTracking()
                             .SingleOrDefault(c => c.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _EntitySet.AsNoTracking();
        }

        public IQueryable<TEntity> GetAll(string includePath)
        {
            return _EntitySet.Include(includePath).AsNoTracking();
        }

        public void AddOrUpdate(TEntity entity)
        {
            if (entity.Id == Constants.NewId)
            {
                _EntitySet.Add(entity);
                entity.State = State.Added;
                return;
            }

            entity.State = State.Modified;
            _EntitySet.Add(entity);

        }

        public void Remove(long id)
        {
            var _toRemove = _EntitySet.AsNoTracking().SingleOrDefault(c => c.Id == id);
            if (_toRemove == null)
            {
                return;
            }
      
            _DeleteManager.Delete(_toRemove);
        }

        public void SubmitChanges()
        {
            _UnitOfWork.Commit();
        }
    }
    public static class Constants
    {
        public const long NewId = 0;
    }
}