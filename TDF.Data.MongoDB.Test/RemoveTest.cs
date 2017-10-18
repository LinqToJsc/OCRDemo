using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDF.Data.MongoDB.Test.TestModels;

namespace TDF.Data.MongoDB.Test
{
    [TestClass]
    public class RemoveTest : TestBase
    {
        [TestMethod]
        public void 能删除单条记录()
        {
            var user = new User()
            {
                Id = Guid.NewGuid()
            };
            MongodbHelper.Default.Insert(user);
            var userFormDb = MongodbHelper.Default.Find<User>(x => x.Id == user.Id).FirstOrDefault();
            Assert.IsNotNull(userFormDb);
            MongodbHelper.Default.Remove<User>(x => x.Id == user.Id);
            userFormDb = MongodbHelper.Default.Find<User>(x => x.Id == user.Id).FirstOrDefault();
            Assert.IsNull(userFormDb);
        }

        [TestMethod]
        public void 能删除多条记录()
        {
            var user1 = new User()
            {
                Id = Guid.NewGuid(),
                Name="a1"
            };
            var user2 = new User()
            {
                Id = Guid.NewGuid(),
                Name = "a1"
            };
            MongodbHelper.Default.InsertBulk(new List<User>() {user1,user2});
            var count = MongodbHelper.Default.Find<User>(x => x.Name == "a1").Count();
            Assert.AreEqual(count,2);
            MongodbHelper.Default.Remove<User>(x => x.Name == "a1");
            count = MongodbHelper.Default.Find<User>(x => x.Name == "a1").Count();
            Assert.AreEqual(count, 0);
        }
    }
}
