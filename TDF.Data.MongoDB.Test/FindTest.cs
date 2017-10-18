using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TDF.Core.Models;
using TDF.Data.MongoDB.Test.TestModels;

namespace TDF.Data.MongoDB.Test
{
    [TestClass]
    public class FindTest : TestBase
    {
        [TestMethod]
        public void 能普通查询()
        {
            var users = MongodbHelper.Default.Find<User>(x => x.Age == 19).ToList();
            Assert.AreEqual(users.Count, 2);
        }

        [TestMethod]
        public void 能复杂查询()
        {
            var users = MongodbHelper.Default.GetCollection<User>().AsQueryable()
                .WhereByRole("admin")
                .WhereByCity("重庆市")
                .ToList();
            Assert.AreEqual(users.Count, 2);
        }

        [TestMethod]
        public void 能分页查询()
        {
            Cleanup();
            var inserts = new List<User>();
            for (var i = 0; i < 10000; i++)
            {
                var user = new User { Id = Guid.NewGuid(), Name = "Name" + i, Age = i };
                inserts.Add(user);
            }
            MongodbHelper.Default.InsertBulk(inserts);
            var results = MongodbHelper.Default.GetCollection<User>().AsQueryable()
                .OrderBy(x => x.Age)
                .ToPageResult(0, 10);
            //var results = Queryable.OrderBy(MongodbHelper.Default.GetCollection<User>().AsQueryable(), x => x.Age)
            //    .ToPageResult(0, 10);
            Assert.AreEqual(results.PageSize, 10);
            Assert.AreEqual(results.TotalPages, 1000);
            Assert.AreEqual(results.TotalCount, 10000);
            Assert.AreEqual(results.Rows[9].Age, 9);
        }

        [TestMethod]
        public void 能聚合查询()
        {
            var results = MongodbHelper.Default.GetCollection<User>().AsQueryable()
                .GroupBy(x => x.Age)
                .Select(x => new {Age = x.Key, Count = x.Count()})
                .ToList();
                
            //var results = Queryable.Select(MongodbHelper.Default.GetCollection<User>().AsQueryable()
            //        .GroupBy(x => x.Age), x => new { Age = x.Key, Count = x.Count() })
            //    .ToList();
            Assert.AreEqual(results.Count(), 5);
            var count = results.First(x => x.Age == 19).Count;
            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void 能关联查询()
        {
            var table1 = MongodbHelper.Default.GetCollection<User>().AsQueryable();
            var table2 = MongodbHelper.Default.GetCollection<Course>().AsQueryable();
            var results =
                table1.Join(table2, user => user.CourseId, course => course.Id,
                    (user, course) => new {User = user, Course = course})
                    .AsQueryable()
                    .Where(x => x.User.Name == "aa")
                    .Where(x => x.Course.Name == "语文")
                    .ToList();
            Assert.AreEqual(results.Count,1);
        }
    }
}
