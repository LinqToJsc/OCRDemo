using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization.Attributes;
using TDF.Data.MongoDB.Test.TestModels;

namespace TDF.Data.MongoDB.Test
{
    [TestClass]
    public class InsertTest : TestBase
    {
        [TestMethod]
        public void 能插入单条数据()
        {
            var user = new User {Id = Guid.NewGuid(), Name = "Name1"};
            MongodbHelper.Default.Insert(user);
            var userFormDb = MongodbHelper.Default.Find<User>(x => user.Id == x.Id).FirstOrDefault();
            Assert.IsNotNull(userFormDb);
            Assert.AreEqual(user.Name, userFormDb.Name);
        }

        [TestMethod]
        public void 能插入多条数据()
        {
            var users = new List<User>();
            for (var i = 0; i < 10000; i++)
            {
                var user = new User { Id = Guid.NewGuid(), Name = "Name"+i };
                users.Add(user);
            }
           
            MongodbHelper.Default.InsertBulk(users);
            var userFormDb = MongodbHelper.Default.Find<User>(x =>x.Name == "Name9999").FirstOrDefault();
            Assert.IsNotNull(userFormDb);
        }
    }
}
