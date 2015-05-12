using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zero.Domain;
using Zero.DAL.EF;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    [TestClass]
    public class CategoryDaoUnitTest
    {


        [TestMethod]
        public void TestSave1()
        {
            Category category = TestHelper.CreateCategory("test", "测试");

            CategoryDao categoryDao = new CategoryDao(TestHelper.ConnectionString);
            int isSave = categoryDao.Save(category);
            Assert.IsTrue(isSave > 0);
        }

        [TestMethod]
        public void TestSave2()
        {
            Category category = TestHelper.CreateCategory("test", "测试");

            CategoryDao categoryDao = new CategoryDao(TestHelper.ConnectionString);
            int isSave = categoryDao.Save(category);
            Assert.IsTrue(isSave > 0);

            category.Name = "hello";
            int isUpdate = categoryDao.Update(category);
            Assert.IsTrue(isUpdate > 0);

            int isDelete = categoryDao.Delete(new Category[] { category });
            Assert.IsTrue(isDelete > 0);
        }

    }
}
