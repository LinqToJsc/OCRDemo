using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using TDF.Data.MongoDB.Extensions;
using TDF.Data.MongoDB.Test;
using TDF.Data.MongoDB.Test.TestModels;

namespace TDF.Data.MongoDB.Test
{
    [TestClass]
    public class UpdateTest : TestBase
    {
        [TestMethod]
        public void 能修改单个属性()
        {
            MongodbHelper.Default.Update<User>(x => x.Age > 18, Builders<User>.Update.Set(x => x.Age, 18));
        }

        [TestMethod]
        public void 能修改多个属性()
        {
            MongodbHelper.Default.Update<User>(x => x.Age == 18 || x.Age == 49,
                 Builders<User>.Update.Set(x => x.Age, 100)
                    .Set(x => x.Name, "feijie")
                    .Set(x => x.Extend, new { p1 = "100" }));

            //MongodbHelper.Default.GetCollection<User>()
            //    .UpdateMany(x => x.Age == 18 || x.Age == 49, BsonDocument.Parse("{$set:{'Age':100,'Name':'feijie','Extend.p1':'100'}}"));

            MongodbHelper.Default.Update<User>
                (x => x.Age == 18 || x.Age == 49, BsonDocument.Parse("{$set:{'Age':100,'Name':'feijie','Extend.p1':'100'}}"));

            //MongodbHelper.Default.GetCollection<User>()
            //    .UpdateMany(x => x.Age>18, Builders<User>.Update.Inc(y => y.Age, 1));
        }
    }
}
