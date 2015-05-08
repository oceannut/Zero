using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.BLL;
using Zero.DAL;
using Nega.Common;

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

        public void DeleteCategory(string id)
        {
            categoryDao.Delete(id);
        }

        public Task DeleteCategoryAsync(string id)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(id);
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

        public IEnumerable<Category> ListCategory(int scope, string parentId = null, bool? isDisused = null)
        {
            return categoryDao.List(scope, parentId, isDisused);
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int scope, string parentId = null, bool? isDisused = null)
        {
            return Task.Factory.StartNew<IEnumerable<Category>>(
                () =>
                {
                    return ListCategory(scope, parentId, isDisused);
                });
        }

        public bool IsCategoryCyclicReference(Category category)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(category.ParentId))
            {
                if (category.Id == category.ParentId)
                {
                    result = true;
                }
                else
                {
                    TreeNodeCollection<Category> tree = categoryDao.Tree(category.Scope);
                    Tree<Category>.PreorderTraverse(tree,
                        (e) =>
                        {
                            if (e.Data.Id == category.ParentId)
                            {
                                TreeNode<Category> parent = e;
                                while (parent != null)
                                {
                                    if (category.Id == parent.Data.Id)
                                    {
                                        result = true;
                                    }
                                    parent = parent.Parent;
                                }
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                }
            }

            return result;
        }

    }
}
