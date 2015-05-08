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
            TreeNodeCollection<Category> tree = categoryDao.Tree(category.Scope);
            CheckCircleRefrence(tree, category);

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
            TreeNodeCollection<Category> tree = categoryDao.Tree(category.Scope);
            CheckCircleRefrence(tree, category);

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

        //public Paging<Category> PagingCategory(int pageIndex, int pageSize, int scope, string parentId = null)
        //{
        //    int count = CountCategory(scope, parentId);
        //    IEnumerable<Category> list = categoryDao.List(pageIndex, pageSize, scope, parentId);
        //    return new Paging<Category>
        //    {
        //        TotalCount = count,
        //        Collection = list
        //    };
        //}

        //public Task<Paging<Category>> PagingCategoryAsync(int pageIndex, int pageSize, int scope, string parentId = null)
        //{
        //    return Task.Factory.StartNew<Paging<Category>>(
        //        () =>
        //        {
        //            return PagingCategory(pageIndex, pageSize, scope, parentId);
        //        });
        //}

        private void CheckCircleRefrence(TreeNodeCollection<Category> tree, Category category)
        {
            if (!string.IsNullOrWhiteSpace(category.ParentId))
            {
                if (category.Id == category.ParentId)
                {
                    throw new ArgumentException("自己不能是自己的父节点");
                }
                else
                {
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
                                        throw new ArgumentException("父节点形成了环");
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
        }

    }
}
