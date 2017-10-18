namespace TDF.Data.EntityFramework.Initializers
{
    public class InitTableAndInitSqlIfNotExists<T> : InitDatabaseAndInitSqlIfNotExists<T> where T : System.Data.Entity.DbContext
    {
        public virtual string InitSqlCreateTablePath { get; set; }

        protected override void CreateDatabase(T context)
        {
            var commands = ParseCommands(InitSqlCreateTablePath);
            foreach (var command in commands)
            {
                context.Database.ExecuteSqlCommand(command);
            }
        }
    }
}
