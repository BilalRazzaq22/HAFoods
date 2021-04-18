namespace ERP.Entities.Migrations
{
    using ERP.Entities.DBModel.AppSettings;
    using ERP.Entities.DBModel.Payments;
    using ERP.Entities.DBModel.Users;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ERP.Entities.DbContext.HAFoodDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ERP.Entities.DbContext.HAFoodDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.AppSettings.AddOrUpdate(p => p.AppVersion, new AppSetting { AppVersion = "1.0.0.0", AppStartDate = DateTime.Now, AppEndDate = DateTime.Now.AddDays(15) });

            //context.Users.AddOrUpdate(p => p.Username, new User { Username = "admin", Email = "admin@hafoods.com", Password = "admin123", UserGroup = "Admin" });

            //context.Payments.AddOrUpdate(p => p.PaymentType,
            //    new Payment { PaymentType = "Cash" },
            //    new Payment { PaymentType = "Credit" });
        }
    }
}
