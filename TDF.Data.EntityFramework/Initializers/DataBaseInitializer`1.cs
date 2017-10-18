using System;
using System.Data.Entity;
using TDF.Core.Configuration;

namespace TDF.Data.EntityFramework.Initializers
{
    public class DataBaseInitializer<T> : AbstractInitializer where T : System.Data.Entity.DbContext
    {
        public IDatabaseInitializer<T> DatabaseInitializer { get; set; }

        public string ConnectionString { get; set; }

        public string ConnectionName { get; set; }

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                Configs.Instance.ConnectionString = ConnectionString;
            }
            if (!string.IsNullOrEmpty(ConnectionName))
            {
                Configs.Instance.ConnectionString = ConnectionName;
            }
            Database.SetInitializer(DatabaseInitializer);
            base.Initialize();
        }
    }

    public static class DataBaseInitializerExt
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public static InitializerContext InitializeDatabase<T>(this InitializerContext context,
            Action<DataBaseInitializer<T>> configure = null) where T : System.Data.Entity.DbContext
        {
            var dataBaseInitialzer = new DataBaseInitializer<T>();
            if (configure != null)
            {
                configure.Invoke(dataBaseInitialzer);
            }
            context.Initialzer.Add(dataBaseInitialzer);
            return context;
        }

    }
}
