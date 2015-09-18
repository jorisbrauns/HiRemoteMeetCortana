using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebService.Infrastructure.Context;

namespace WebService.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        // Readonly Fields
        private readonly IHiRemoteMeetCortanaContext _Context;

        // Methods
        public UnitOfWork(IHiRemoteMeetCortanaContext context)
        {
            _Context = context;
        }

        public void Commit()
        {
            try
            {
                var _trackedEntities = _Context.ExtractDbContext().ChangeTracker.Entries<IObjectWithState>().ToList();
                foreach (var _entry in _trackedEntities)
                {
                    var _stateInfo = _entry.Entity;
                    _entry.State = ConvertState(_stateInfo.State);
                }

                _Context.SaveChanges();
            }
            catch (DbEntityValidationException _dbEx)
            {
                foreach (var _validationErrors in _dbEx.EntityValidationErrors)
                {
                    foreach (var _validationError in _validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine("Property: {0} Error: {1}", _validationError.PropertyName,
                            _validationError.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private static EntityState ConvertState(State state)
        {
            switch (state)
            {
                case State.Added:
                    return EntityState.Added;
                case State.Deleted:
                    return EntityState.Deleted;
                case State.Modified:
                    return EntityState.Modified;
                case State.Unchanged:
                    return EntityState.Unchanged;
                default:
                    return EntityState.Unchanged;
            }
        }

        public void Rollback()
        {
            //Entity Framework Rollsback automatically (SaveChanges)
        }

        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    if (_Context != null)
            //    {
            //        _Context.Dispose();
            //    }
            //}
        }
    }
    public enum State
    {
        Added,
        Unchanged,
        Modified,
        Deleted
    }
}