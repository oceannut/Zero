using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;

namespace Zero.BLL
{

    public interface ICategoryService
    {

        void SaveCategory(Category category);

        Task SaveCategoryAsync(Category category);

        void UpdateCategory(Category category);

        Task UpdateCategoryAsync(Category category);

        void DeleteCategory(string id);

        Task DeleteCategoryAsync(string id);

        Category GetCategory(string id, bool? includeParent = null);

        Task<Category> GetCategoryAsync(string id, bool? includeParent = null);

        Category GetCategoryByCode(int scope, string code, bool? includeParent = null);

        Task<Category> GetCategoryByCodeAsync(int scope, string code, bool? includeParent = null);

        bool IsCategoryCodeExisted(int scope, string code);

        Task<bool> IsCategoryCodeExistedAsync(int scope, string code);

        int CountCategory(int? scope = null, string parentId = null);

        Task<int> CountCategoryAsync(int? scope = null, string parentId = null);

        IEnumerable<Category> ListCategory(int scope, bool? includeParent = null, string parentId = null);

        Task<IEnumerable<Category>> ListCategoryAsync(int scope, bool? includeParent = null, string parentId = null);

        Paging<Category> PagingCategory(PagingRequest request, int scope, bool? includeParent = null, string parentId = null);

        Task<Paging<Category>> PagingCategoryAsync(PagingRequest request, int scope, bool? includeParent = null, string parentId = null);

    }

}
