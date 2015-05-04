using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zero.Domain;
using Zero.DAL.EF;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    [TestClass]
    public class UserDaoUnitTest
    {

        private const string connectionString = "connectionString";

        [TestMethod]
        public void TestCURD()
        {
            User user = TestHelper.CreateUser();

            UserDao userDao = new UserDao(connectionString);
            int isSave = userDao.Save(user);
            Assert.IsTrue(isSave > 0);

            user.Name = "hello";
            int isUpdate = userDao.Update(user);
            Assert.IsTrue(isUpdate > 0);

            int isDelete = userDao.Delete(user.Id);
            Assert.IsTrue(isDelete > 0);
        }

        [TestMethod]
        public void TestSave()
        {
            int isSave = 0;
            UserDao userDao = new UserDao(connectionString);
            RoleDao roleDao = new RoleDao(connectionString);

            Role role = TestHelper.CreateRole();
            isSave = roleDao.Save(role);
            Assert.IsTrue(isSave > 0);

            User user = TestHelper.CreateUser();
            user.Roles = new List<Role>();
            user.Roles.Add(role);
            role = TestHelper.CreateRole();
            user.Roles.Add(role);
            isSave = userDao.Save(user);
            Assert.IsTrue(isSave > 0);
        }

        [TestMethod]
        public void TestAddRole()
        {
            UserDao userDao = new UserDao(connectionString);
            RoleDao roleDao = new RoleDao(connectionString);

            User user = TestHelper.CreateUser();
            Role role = TestHelper.CreateRole();
            user.Roles = new List<Role>();
            user.Roles.Add(role);
            int isSave = userDao.Save(user);
            Assert.IsTrue(isSave > 0);

            role = TestHelper.CreateRole();
            user.Roles.Add(role);
            isSave = userDao.Update(user);
            Assert.IsTrue(isSave > 0);

        }

        [TestMethod]
        public void TestAddRole2()
        {
            UserDao userDao = new UserDao(connectionString);
            RoleDao roleDao = new RoleDao(connectionString);

            User user = TestHelper.CreateUser();
            int isSave = userDao.Save(user);
            Assert.IsTrue(isSave > 0);

            Role role = TestHelper.CreateRole();
            isSave = roleDao.Save(role);
            Assert.IsTrue(isSave > 0);

            user.Roles = new List<Role>();
            user.Roles.Add(role);
            isSave = userDao.Update(user);
            Assert.IsTrue(isSave > 0);
        }

        [TestMethod]
        public void TestList()
        {
            UserDao userDao = new UserDao(connectionString);
            RoleDao roleDao = new RoleDao(connectionString);

            Role role = TestHelper.CreateRole();
            int isSave = roleDao.Save(role);
            Assert.IsTrue(isSave > 0);

            User user1 = TestHelper.CreateUser();
            User user2 = TestHelper.CreateUser();
            role.Users = new List<User>();
            role.Users.Add(user1);
            role.Users.Add(user2);
            isSave = roleDao.Update(role);
            Assert.IsTrue(isSave > 0);

            IList<User> list = userDao.List(role.Id);
            Assert.AreEqual(2, list.Count);

            int isDelete = roleDao.Delete(role.Id);
            Assert.IsTrue(isDelete > 0);

            list = userDao.List(role.Id);
            Assert.AreEqual(0, list.Count);

        }



    }
}
