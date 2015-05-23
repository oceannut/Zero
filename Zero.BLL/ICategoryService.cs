using Nega.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Domain;

namespace Zero.BLL
{

    public interface ICategoryService
    {

        void SaveCategory(Category category);

        Task SaveCategoryAsync(Category category);

        void UpdateCategory(Category category);

        Task UpdateCategoryAsync(Category category);

        void DeleteCategory(ICollection<Category> categories);

        Task DeleteCategoryAsync(ICollection<Category> categories);

        Category GetCategoryByCode(int scope, string code, bool? includeParent = null);

        Task<Category> GetCategoryByCodeAsync(int scope, string code, bool? includeParent = null);

        bool IsCategoryCodeExisted(int scope, string code);

        Task<bool> IsCategoryCodeExistedAsync(int scope, string code);

        int CountCategory(int? scope = null, string parentId = null, bool? isDisused = null);

        Task<int> CountCategoryAsync(int? scope = null, string parentId = null, bool? isDisused = null);

        IEnumerable<Category> ListCategory(int? scope, string parentId = null, bool? isDisused = null);

        Task<IEnumerable<Category>> ListCategoryAsync(int? scope, string parentId = null, bool? isDisused = null);

        TreeNodeCollection<Category> TreeCategory(int scope);

        Task<TreeNodeCollection<Category>> TreeCategoryAsync(int scope);

    }

}
