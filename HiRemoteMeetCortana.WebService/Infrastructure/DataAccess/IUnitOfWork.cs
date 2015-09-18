using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Infrastructure.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}