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

        public void UpdateCategory(IEnumerable<Category> col)
        {
            if (col == null || col.Count() == 0)
            {
                throw new ArgumentNullException();
            }
            categoryDao.Update(col);
        }

        public Task UpdateCategoryAsync(IEnumerable<Category> col)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    UpdateCategory(col);
                });
        }

        //public void DeleteCategory(int scope, IEnumerable<string> idCol)
        //{
        //    if (idCol == null || idCol.Count() == 0)
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    var categories = ListCategory(scope);
        //    if (categories != null && categories.Count() > 0)
        //    {
        //        var deleteList = categories.Where(e => idCol.Contains(e.Id));
        //        if (deleteList != null && deleteList.Count() > 0)
        //        {
        //            categoryDao.Delete(deleteList.ToArray());
        //        }
        //    }
        //}

        //public Task DeleteCategoryAsync(int scope, IEnumerable<string> idCol)
        //{
        //    return Task.Factory.StartNew(
        //        () =>
        //        {
        //            DeleteCategory(scope, idCol);
        //        });
        //}

        public void DeleteCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }
            categoryDao.Delete(category);
        }

        public Task DeleteCategoryAsync(Category category)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(category);
                });
        }

        public void DeleteCategory(IEnumerable<Category> col)
        {
            if (col == null || col.Count() == 0)
            {
                throw new ArgumentNullException();
            }
            categoryDao.Delete(col);
        }

        public Task DeleteCategoryAsync(IEnumerable<Category> col)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(col);
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
            return categoryDao.GetByCode(scope, code);
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

        public int CountCategory(int? scope = null, bool? isDisused = null)
        {
            return categoryDao.Count(scope, isDisused);
        }

        public Task<int> CountCategoryAsync(int? scope = null, bool? isDisused = null)
        {
            return Task.Factory.StartNew<int>(
                () =>
                {
                    return CountCategory(scope, isDisused);
                });
        }

        
        public IEnumerable<Category> ListCategory(int? scope, bool? isDisused = null)
        {
            return categoryDao.List(scope, isDisused);
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int? scope, bool? isDisused = null)
        {
            return Task.Factory.StartNew<IEnumerable<Category>>(
                () =>
                {
                    return ListCategory(scope, isDisused);
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
