﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zero.Domain;
using Zero.DAL.EF;

namespace UnitTest.Zero.DAL.EF.MySQL__
{
    [TestClass]
    public class CategoryDaoUnitTest
    {

        

        [TestMethod]
        public void TestMethod1()
        {
            Category category = TestHelper.CreateCategory();

            CategoryDao categoryDao = new CategoryDao(TestHelper.ConnectionString);
            bool isSave = categoryDao.Save(category);
            Assert.IsTrue(isSave);

            category.Name = "hello";
            bool isUpdate = categoryDao.Update(category);
            Assert.IsTrue(isUpdate);

            bool isDelete = categoryDao.Delete(category.Id);
            Assert.IsTrue(isDelete);
        }

    }
}
