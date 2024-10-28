namespace Demo_WCF_NetFramework.Migrations
{
    using Demo_WCF_NetFramework.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Demo_WCF_NetFramework.Model.MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Demo_WCF_NetFramework.Model.MyDbContext context)
        {
            context.Users.AddOrUpdate(
                    new User { Id = 1, Name = "Thai" },
                    new User { Id = 2, Name = "Lam" },
                    new User { Id = 3, Name = "Phat" },
                    new User { Id = 4, Name = "Tam" },
                    new User { Id = 5, Name = "Sao" }
                );
        }
    }
}
