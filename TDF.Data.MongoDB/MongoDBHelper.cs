using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TDF.Core.Configuration;
using TDF.Core.Log;

namespace TDF.Data.MongoDB
{
    public class MongodbHelper
    {
        private readonly string _databasename;
        private static readonly string ConnectionString = Configs.Instance.GetValue("MongoDBConnectionString");

        private static MongodbHelper _instance;

        public static MongodbHelper Default
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MongodbHelper();
                }
                return _instance;
            }
        }

        public static void SetDBConfig(bool replica, string databasename, string conn = null)
        {
            if (string.IsNullOrEmpty(conn))
            {
                conn = ConnectionString;
            }
            _instance = new MongodbHelper(replica, conn, databasename);
        }

        private MongoClientSettings setting = null;
        public MongoClient Client { get; set; }
        public string DatabaseName { get; set; }
        public bool Replica { get; set; }

        public MongodbHelper() : this(true) { }
        public MongodbHelper(bool replica) : this(replica, ConnectionString) { }
        public MongodbHelper(bool replica, string conn) : this(replica, conn, string.Empty) { }

        public MongodbHelper(string databasename) : this(true, databasename, string.Empty) { }

        public MongodbHelper(bool replica, string conn, string databasename)
        {
            DatabaseName = databasename;
            //副本集
            if (replica)
            {
                var ips = conn.Split(',');
                var serviceList = new List<MongoServerAddress>();
                foreach (var ip in ips)
                {
                    var host = ip.Split(':')[0];
                    var port = Convert.ToInt32(ip.Split(':')[1]);
                    serviceList.Add(new MongoServerAddress(host, port));
                }
                setting = new MongoClientSettings()
                {
                    ReplicaSetName = "Replica_TDF",
                    Servers = serviceList
                };
                Client = new MongoClient(setting);
            }
            else
            {
                Client = new MongoClient(conn);
            }
        }
        public IMongoCollection<T> GetCollection<T>(string databaseName,string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }
            var db = Client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(tableName);
        }


        public IMongoCollection<T> GetCollection<T>(string tableName = null)
        {
            return GetCollection<T>(DatabaseName, tableName);
        }

        #region 删除操作

        public void Remove<T>(Expression<Func<T, bool>> func) => Remove<T>(DatabaseName, string.Empty, func);

        public void Remove<T>(string tableName, Expression<Func<T, bool>> func) => Remove<T>(DatabaseName, tableName, func);

        public void Remove<T>(string database, string tableName, Expression<Func<T, bool>> func)
        {
            var collection = GetCollection<T>(database,tableName);
            collection.DeleteMany(func);
        }

        #endregion

        #region 插入操作

        public void Insert<T>(T table) => Insert<T>(DatabaseName, string.Empty, table);

        public void Insert<T>(string database, string tableName, T table)
        {
            var collection = GetCollection<T>(database, tableName);
            collection.InsertOne(table);
        }

        #endregion

        #region 批量插入

        public bool InsertBulk<T>(IEnumerable<T> list) => InsertBulk(string.Empty, list);

        public bool InsertBulk<T>(string tableName, IEnumerable<T> list) => InsertBulk(DatabaseName, tableName, list);

        public bool InsertBulk<T>(string database, string tableName, IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                return true;
            }
            GetCollection<T>(database, tableName).InsertMany(list);
            return true;
        }

        #endregion

        #region 修改

        public bool Update<T>(Expression<Func<T, bool>> func, UpdateDefinition<T> updateDefinination) => Update(string.Empty, func, updateDefinination);

        public bool Update<T>(string tableName, Expression<Func<T, bool>> func, UpdateDefinition<T> updateDefinination) => Update(DatabaseName, tableName, func, updateDefinination);

        public bool Update<T>(string database, string tableName, Expression<Func<T, bool>> func,
            UpdateDefinition<T> updateDefinination)
        {
            GetCollection<T>(database, tableName)
                .UpdateMany(func, updateDefinination);
            return true;
        }

        #endregion

        #region 查找

        public IMongoQueryable<T> Find<T>(Expression<Func<T, bool>> func) => Find(string.Empty, func);

        public IMongoQueryable<T> Find<T>(string tableName, Expression<Func<T, bool>> func) => Find(DatabaseName, tableName, func);

        public IMongoQueryable<T> Find<T>(string database, string tableName, Expression<Func<T, bool>> func)
        {
            return GetCollection<T>(database, tableName).AsQueryable().Where(func);
        }
        #endregion
    }
}
