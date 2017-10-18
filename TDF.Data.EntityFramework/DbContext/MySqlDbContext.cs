using System.Data.Entity;
using System.Linq;
using System.Reflection;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Data.EntityFramework.DbContext
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MySqlDbContext : DefaultDbContext
    {
        public MySqlDbContext(string nameOrConnectionstring) : base(nameOrConnectionstring)
        {
        }

        public MySqlDbContext()
        {
        }

        public override Assembly[] GetMappingAssemblies()
        {
            return Ioc.Resolve<ITypeFinder>().GetAssemblies().ToArray();
        }
    }
}
