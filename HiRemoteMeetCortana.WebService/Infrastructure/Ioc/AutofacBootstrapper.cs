using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Controllers.Infrastructure.Ioc
{
    public class AutofacBootstrapper
    {
        public ContainerBuilder RegisterModules()
        {
            var _builder = new ContainerBuilder();

            //_builder.RegisterModule(new ServiceModule());
            _builder.RegisterModule(new RepositoryModule());
            return _builder;
        }
    }
}