using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    public static class TestHelper
    {

        public const string ConnectionString = "connectionString";

        public static User CreateUser()
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "test",
                Pwd = "test",
                Name = "test",
                Email = "test",
                Group = "test",
                Creation = DateTime.Now,
                Modification = DateTime.Now
            };
        }

        public static Role CreateRole()
        {
            return new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test",
                Creation = DateTime.Now,
                Modification = DateTime.Now
            };
        }

        public static Category CreateCategory(string code, string name)
        {
            Category category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Scope = 1,
                Creation = DateTime.Now,
                Modification = DateTime.Now
            };
            category.Code = code;
            return category;
        }

    }
}
