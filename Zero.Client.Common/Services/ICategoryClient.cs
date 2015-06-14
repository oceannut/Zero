using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

using Zero.Domain;

namespace Zero.Client.Common
{

    public interface ICategoryClient
    {

        Category SaveCategory(int scope, string name, string desc, string parentId);

        Task<Category> SaveCategoryAsync(int scope, string name, string desc, string parentId);

        IEnumerable<Category> ListCategory(int scope);

        Task<IEnumerable<Category>> ListCategoryAsync(int scope);

        TreeNodeCollection<Category> Tree(int scope);

    }

}
