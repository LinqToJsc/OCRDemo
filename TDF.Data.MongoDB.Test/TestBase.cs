using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using TDF.Data.MongoDB.Test.TestModels;

namespace TDF.Data.MongoDB.Test
{
    [TestClass]
    public class TestBase
    {

        public TestBase()
        {
           
        }

        [TestInitialize]
        public void Init()
        {
            MongodbHelper.SetDBConfig(false, "test", "mongodb://192.168.184.128:27017");
            Cleanup();
            var users = new List<User>();
            var courses = new List<Course>();
            courses.Add(new Course()
            {
                Id = Guid.NewGuid(),
                Name = "语文"
            });
            courses.Add(new Course()
            {
                Id = Guid.NewGuid(),
                Name = "数学"
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                CourseId = courses[0].Id,
                Name = "aa",
                Age = 19,
                //Address = new Address() { City = "重庆市", Province = "重庆" },
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "admin",
                        Sort = 1
                    },
                    new Role()
                    {
                        Name = "customer",
                        Sort = 2
                    }
                }
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "bb",
                Age = 30,
                CourseId = courses[0].Id,
                Address = new Address() { City = "北京市", Province = "北京" },
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "customer",
                        Sort = 2
                    }
                }
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "cc",
                CourseId = courses[0].Id,
                Age = 20,
                Address = new Address() { City = "成都", Province = "四川" },
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "admin",
                        Sort = 1
                    }
                }
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "dd",
                Age = 15,
                Address = new Address() { City = "贵阳市", Province = "贵州" },
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "ee",
                CourseId = courses[1].Id,
                Age = 19,
                Address = new Address() { City = "重庆市", Province = "重庆" },
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "admin",
                        Sort = 1
                    },
                    new Role()
                    {
                        Name = "customer",
                        Sort = 2
                    }
                }
            });
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "ff",
                Age = 49,
                CourseId = courses[1].Id,
                Address = new Address() { City = "重庆市", Province = "重庆" },
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "admin",
                        Sort = 1
                    },
                    new Role()
                    {
                        Name = "customer",
                        Sort = 2
                    }
                },
                Extend = new {p1="1",p2="2"}
            });

            MongodbHelper.Default.InsertBulk(users);
            MongodbHelper.Default.InsertBulk(courses);
        }

       [TestCleanup]
        public void Cleanup()
        {
            MongodbHelper.Default.Client.GetDatabase("test").ListCollections().ToList().ForEach(x =>
            {
                MongodbHelper.Default.Client.GetDatabase("test").DropCollection(x.Elements.ElementAt(0).Value.AsString);
            });
        }
    }
}
