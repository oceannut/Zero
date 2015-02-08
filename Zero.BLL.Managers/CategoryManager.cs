using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.BLL;
using Zero.DAL;

namespace Zero.BLL.Managers
{
    public class CategoryManager : ICategoryService
    {

        private ICategoryDao categoryDao;

        public CategoryManager(ICategoryDao categoryDao)
        {
            this.categoryDao = categoryDao;
        }

        public void SaveCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task SaveCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(string id, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryAsync(string id, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryByCode(int scope, string code, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryByCodeAsync(int scope, string code, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public bool IsCategoryCodeExisted(int scope, string code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryCodeExistedAsync(int scope, string code)
        {
            throw new NotImplementedException();
        }

        public int CountCategory(int? scope = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountCategoryAsync(int? scope = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> ListCategory(int scope, bool? includeParent = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int scope, bool? includeParent = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public Paging<Category> PagingCategory(PagingRequest request, int scope, bool? includeParent = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Paging<Category>> PagingCategoryAsync(PagingRequest request, int scope, bool? includeParent = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

    }
}
