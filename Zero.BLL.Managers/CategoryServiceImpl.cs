﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

using Zero.Domain;
using Zero.BLL;
using Zero.DAL;

namespace Zero.BLL.Impl
{
    public class CategoryServiceImpl : ICategoryService
    {

        private ICategoryDao categoryDao;

        public CategoryServiceImpl(ICategoryDao categoryDao)
        {
            this.categoryDao = categoryDao;
        }

        public void SaveCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }
            categoryDao.Save(category);
        }

        public Task SaveCategoryAsync(Category category)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    SaveCategory(category);
                });
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }
            categoryDao.Update(category);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    UpdateCategory(category);
                });
        }

        public void DeleteCategory(int scope, ICollection<string> idCol)
        {
            if (idCol == null || idCol.Count == 0)
            {
                throw new ArgumentNullException();
            }

            var categories = ListCategory(scope);
            if (categories != null && categories.Count() > 0)
            {
                var deleteList = categories.Where(e => idCol.Contains(e.Id));
                if (deleteList != null && deleteList.Count() > 0)
                {
                    categoryDao.Delete(deleteList.ToArray());
                }
            }
        }

        public Task DeleteCategoryAsync(int scope, ICollection<string> idCol)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(scope, idCol);
                });
        }

        public Category GetCategory(int scope, string id)
        {
            return categoryDao.Get(scope, id);
        }

        public Task<Category> GetCategoryAsync(int scope, string id)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return GetCategory(scope, id);
                });
        }

        public Category GetCategoryByCode(int scope, string code)
        {
            return categoryDao.GetByCode(scope, code, true);
        }

        public Task<Category> GetCategoryByCodeAsync(int scope, string code)
        {
            return Task.Factory.StartNew<Category>(
                () =>
                {
                    return GetCategoryByCode(scope, code);
                });
        }

        public bool IsCategoryCodeExisted(int scope, string code)
        {
            return categoryDao.IsCodeExisted(scope, code);
        }

        public Task<bool> IsCategoryCodeExistedAsync(int scope, string code)
        {
            return Task.Factory.StartNew<bool>(
                () =>
                {
                    return IsCategoryCodeExisted(scope, code);
                });
        }

        public int CountCategory(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            return categoryDao.Count(scope, parentId, isDisused);
        }

        public Task<int> CountCategoryAsync(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            return Task.Factory.StartNew<int>(
                () =>
                {
                    return CountCategory(scope, parentId, isDisused);
                });
        }

        public IEnumerable<Category> ListCategory(int? scope, string parentId = null, bool? isDisused = null)
        {
            return categoryDao.List(scope, parentId, isDisused);
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int? scope, string parentId = null, bool? isDisused = null)
        {
            return Task.Factory.StartNew<IEnumerable<Category>>(
                () =>
                {
                    return ListCategory(scope, parentId, isDisused);
                });
        }

        public TreeNodeCollection<Category> TreeCategory(int scope)
        {
            return categoryDao.Tree(scope);
        }

        public Task<TreeNodeCollection<Category>> TreeCategoryAsync(int scope)
        {
            return Task.Factory.StartNew<TreeNodeCollection<Category>>(
                () =>
                {
                    return TreeCategory(scope);
                });
        }


    }
}
