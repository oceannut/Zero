using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zero.Domain;
using Zero.DAL.EF;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    [TestClass]
    public class RoleDaoUnitTest
    {

        private const string connectionString = "connectionString";

        [TestMethod]
        public void TestCURD()
        {

            Role role = TestHelper.CreateRole();

            RoleDao roleDao = new RoleDao(connectionString);
            bool isSave = roleDao.Save(role);
            Assert.IsTrue(isSave);

            role.Name = "hello";
            bool isUpdate = roleDao.Update(role);
            Assert.IsTrue(isUpdate);

            bool isDelete = roleDao.Delete(role.Id);
            Assert.IsTrue(isDelete);

        }

        [TestMethod]
        public void TestAddUser()
        {
            UserDao userDao = new UserDao(connectionString);
            RoleDao roleDao = new RoleDao(connectionString);

            User user = TestHelper.CreateUser();
            Role role = TestHelper.CreateRole();
            role.Users = new List<User>();
            role.Users.Add(user);
            bool isSave = roleDao.Save(role);
            Assert.IsTrue(isSave);

            user = TestHelper.CreateUser();
            role.Users.Add(user);
            isSave = roleDao.Update(role);
            Assert.IsTrue(isSave);

        }
    }
}
