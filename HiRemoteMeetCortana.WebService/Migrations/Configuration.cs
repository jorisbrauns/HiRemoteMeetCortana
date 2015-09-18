namespace WebService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebService.Infrastructure.Context.HiRemoteMeetCortanaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebService.Infrastructure.Context.HiRemoteMeetCortanaContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Settings.AddOrUpdate(new Models.Settings
            {
                IsOn = true, TimeToWake = DateTime.Now, Daily = true
            });
        }
    }
}
