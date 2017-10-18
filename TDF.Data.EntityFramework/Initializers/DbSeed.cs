using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Data.EntityFramework.Initializers
{
    public class DbSeed
    {
        public static void Seed(System.Data.Entity.DbContext context)
        {
            var seedContainers = Ioc.Resolve<ITypeFinder>().CreateInstanceOnFoundType<ISeedContainer>();
            foreach (var seedContainer in seedContainers)
            {
                seedContainer.Seed(context);
            }
        }
    }
}
