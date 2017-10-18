using System.Data.Entity.Migrations;
using TDF.Data.EntityFramework.DbContext;
using TDF.Data.EntityFramework.Initializers;

namespace TDF.Data.EntityFramework.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DefaultDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DefaultDbContext context)
        {
            DbSeed.Seed(context);
        }
    }
}
