using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using MySql.Data.Entity;

namespace TDF.Data.EntityFramework.Migrations
{
    public class IgnoreDboSqlGenerator : MySqlMigrationSqlGenerator
    {
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            var res = base.Generate(migrationOperations, providerManifestToken).ToList();
            foreach (var ms in res)
            {
                ms.Sql = ms.Sql.Replace("dbo.", "");
            }
            return res;
        }
    }
}
