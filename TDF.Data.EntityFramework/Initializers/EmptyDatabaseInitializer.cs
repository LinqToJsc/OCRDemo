using System.Data.Entity;

namespace TDF.Data.EntityFramework.Initializers
{
    /// <summary>
    /// 空的数据库初始化器，适用于正是环境
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmptyDatabaseInitializer<T> : IDatabaseInitializer<T> where T : System.Data.Entity.DbContext
    {
        public void InitializeDatabase(T context)
        {

        }
    }
}
