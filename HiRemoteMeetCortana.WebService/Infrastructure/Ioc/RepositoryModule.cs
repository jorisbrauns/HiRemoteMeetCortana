using Autofac;
using WebService.Infrastructure.Context;
using WebService.Infrastructure.DataAccess;
using WebService.Infrastructure.Repository;

namespace WebService.Controllers.Infrastructure.Ioc
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                   .As(typeof(IUnitOfWork))
                   .InstancePerLifetimeScope();

            builder.RegisterType<HiRemoteMeetCortanaContext>()
                  .As(typeof(IHiRemoteMeetCortanaContext))
                  .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<DeleteManager>()
                    .As(typeof(IDeleteManager))
                   .InstancePerLifetimeScope();

            //builder.RegisterType<SettingsRepository>()
            //    .As<ISettingsRepository>()
            //    .InstancePerRequest();
        }
    }
}