using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Service.Rest
{

    public class CategoryRestServiceImpl : MarshalByRefObject, ICategoryRestService
    {

        private ICategoryService categoryService;

        public CategoryRestServiceImpl(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public Category SaveCategory(string scope, string name, string desc, string parentId)
        {
            int scopeInt = Scope2Int(scope);
            ValidateNameAndDesc(name, desc);
            Category parent = GetParent(scopeInt, parentId);

            try
            {
                Category category = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Desc = desc,
                    Scope = scopeInt,
                    Parent = parent
                };
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                category.Save(tree,
                    (e) =>
                    {
                        this.categoryService.SaveCategory(category);
                    });
                //throw new Exception("测试");
                return category;
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public Category UpdateCategory(string scope, string id, string name, string desc)
        {
            try
            {
                int scopeInt = Convert.ToInt32(scope);
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                Category category = this.categoryService.GetCategory(scopeInt, id);
                category.Name = name;
                category.Desc = desc;
                category.Update(tree,
                    (e) =>
                    {
                        this.categoryService.UpdateCategory(category);
                    });

                return category;
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public void DeleteCategory(string scope, string id)
        {
            try
            {
                int scopeInt = Convert.ToInt32(scope);
                Category category = this.categoryService.GetCategory(scopeInt, id);
                this.categoryService.DeleteCategory(category);
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public Category GetCategory(string scope, string id)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryByCode(string scope, string code)
        {
            throw new NotImplementedException();
        }

        public Category[] ListCategories(string scope)
        {
            try
            {
                return this.categoryService.ListCategory(Convert.ToInt32(scope)).ToArray();
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        private int Scope2Int(string scope)
        {
            int scopeInt = 0;
            try
            {
                scopeInt = Convert.ToInt32(scope);
            }
            catch (Exception)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            return scopeInt;
        }

        private void ValidateNameAndDesc(string name, string desc)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
        }

        private Category GetParent(int scopeInt, string parentId)
        {
            Category parent = null;
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                parent = this.categoryService.GetCategory(scopeInt, parentId);
            }

            return parent;
        }

    }

}
