using System.Data.Entity.Migrations;
using MySql.Data.Entity;
using TDF.Data.EntityFramework.DbContext;
using TDF.Data.EntityFramework.Initializers;

namespace TDF.Data.EntityFramework.Migrations
{
    public sealed class MySqlConfiguration : DbMigrationsConfiguration<MySqlDbContext>
    {
        public MySqlConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new IgnoreDboSqlGenerator());
            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, ""));
            CodeGenerator = new MySqlMigrationCodeGenerator();
        }

        protected override void Seed(MySqlDbContext context)
        {
            DbSeed.Seed(context);
        }
    }
}
