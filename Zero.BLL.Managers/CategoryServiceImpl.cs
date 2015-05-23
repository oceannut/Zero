using System;
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

        public void DeleteCategory(ICollection<Category> categories)
        {
            categoryDao.Delete(categories);
        }

        public Task DeleteCategoryAsync(ICollection<Category> categories)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(categories);
                });
        }

        public Category GetCategoryByCode(int scope, string code, bool? includeParent = null)
        {
            return categoryDao.GetByCode(scope, code, includeParent);
        }

        public Task<Category> GetCategoryByCodeAsync(int scope, string code, bool? includeParent = null)
        {
            return Task.Factory.StartNew<Category>(
                () =>
                {
                    return GetCategoryByCode(scope, code, includeParent);
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
