using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Data.MongoDB.Test.TestModels
{

    public class User
    {
        //[BsonId]
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }

        public List<Role> Roles { get; set; }

        public dynamic Extend { get; set; }

        public Guid CourseId { get; set; }
    }

    public class Address
    {
        public string Province { get; set; }

        public string City { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }

        public int Sort { get; set; }
    }

    public static class UserExtensions
    {
        public static IQueryable<User> WhereByRole(this IQueryable<User> query, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return query;
            }
            return query.Where(x => x.Roles.Any(y => y.Name == role));
        }

        public static IQueryable<User> WhereByCity(this IQueryable<User> query, string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return query;
            }
            return query.Where(x => x.Address.City == city);
        }
    }
}
