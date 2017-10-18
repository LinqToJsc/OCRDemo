using System.Data.Entity;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Data.EntityFramework.Initializers
{
    /// <summary>
    /// 如果数据库不存在就创建数据库，并初始化种子数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CreateSeedDatabaseIfNotExists<T> : CreateDatabaseIfNotExists<T> where T: System.Data.Entity.DbContext
    {
        protected override void Seed(T context)
        {
            DbSeed.Seed(context);
        }
    }
}
