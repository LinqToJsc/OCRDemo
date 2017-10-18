using System.Linq;
using System.Reflection;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Data.EntityFramework.DbContext
{
    public class DefaultDbContext : AbstractDbContext
    {
        public DefaultDbContext(string nameOrConnectionstring) : base(nameOrConnectionstring)
        {
        }

        public DefaultDbContext()
        {
        }

        public override Assembly[] GetMappingAssemblies()
        {
            return Ioc.Resolve<ITypeFinder>().GetAssemblies().ToArray();
        }
    }
}
