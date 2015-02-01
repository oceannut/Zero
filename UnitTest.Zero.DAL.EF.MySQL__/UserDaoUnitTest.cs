using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zero.Domain;
using Zero.DAL.EF.MySQL;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    [TestClass]
    public class UserDaoUnitTest
    {
        [TestMethod]
        public void TestCURD()
        {
            User user = TestHelper.CreateUser();

            UserDao userDao = new UserDao();
            bool isSave = userDao.Save(user);
            Assert.IsTrue(isSave);

            user.Name = "hello";
            bool isUpdate = userDao.Update(user);
            Assert.IsTrue(isUpdate);

            bool isDelete = userDao.Delete(user.Id);
            Assert.IsTrue(isDelete);
        }

        [TestMethod]
        public void TestSave()
        {
            bool isSave = false;
            UserDao userDao = new UserDao();
            RoleDao roleDao = new RoleDao();

            Role role = TestHelper.CreateRole();
            isSave = roleDao.Save(role);
            Assert.IsTrue(isSave);

            User user = TestHelper.CreateUser();
            user.Roles = new List<Role>();
            user.Roles.Add(role);
            role = TestHelper.CreateRole();
            user.Roles.Add(role);
            isSave = userDao.Save(user);
            Assert.IsTrue(isSave);
        }

        [TestMethod]
        public void TestAddRole()
        {
            UserDao userDao = new UserDao();
            RoleDao roleDao = new RoleDao();

            User user = TestHelper.CreateUser();
            Role role = TestHelper.CreateRole();
            user.Roles = new List<Role>();
            user.Roles.Add(role);
            bool isSave = userDao.Save(user);
            Assert.IsTrue(isSave);

            role = TestHelper.CreateRole();
            user.Roles.Add(role);
            isSave = userDao.Update(user);
            Assert.IsTrue(isSave);

        }

        [TestMethod]
        public void TestAddRole2()
        {
            UserDao userDao = new UserDao();
            RoleDao roleDao = new RoleDao();

            User user = TestHelper.CreateUser();
            bool isSave = userDao.Save(user);
            Assert.IsTrue(isSave);

            Role role = TestHelper.CreateRole();
            isSave = roleDao.Save(role);
            Assert.IsTrue(isSave);

            user.Roles = new List<Role>();
            user.Roles.Add(role);
            isSave = userDao.Update(user);
            Assert.IsTrue(isSave);
        }

        [TestMethod]
        public void TestList()
        {
            UserDao userDao = new UserDao();
            RoleDao roleDao = new RoleDao();

            Role role = TestHelper.CreateRole();
            bool isSave = roleDao.Save(role);
            Assert.IsTrue(isSave);

            User user1 = TestHelper.CreateUser();
            User user2 = TestHelper.CreateUser();
            role.Users = new List<User>();
            role.Users.Add(user1);
            role.Users.Add(user2);
            isSave = roleDao.Update(role);
            Assert.IsTrue(isSave);

            IList<User> list = userDao.List(role.Id);
            Assert.AreEqual(2, list.Count);

            bool isDelete = roleDao.Delete(role.Id);
            Assert.IsTrue(isDelete);

            list = userDao.List(role.Id);
            Assert.AreEqual(0, list.Count);

        }



    }
}
