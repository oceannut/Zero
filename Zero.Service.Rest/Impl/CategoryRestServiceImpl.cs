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

    public class CategoryRestServiceImpl : ICategoryRestService
    {

        private ICategoryService categoryService;

        public CategoryRestServiceImpl(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public Category SaveCategory(string scope, string name, string description, string parentId)
        {
            try
            {
                int scopeInt = Convert.ToInt32(scope);
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                Category parent = null;
                if (!string.IsNullOrWhiteSpace(parentId))
                {
                    parent = this.categoryService.GetCategory(scopeInt, parentId);
                }
                Category category = new Category
                {
                    Name = name,
                    Desc = description,
                    Scope = scopeInt,
                    Parent = parent
                };
                category.Id = Guid.NewGuid().ToString();
                category.Save(tree,
                    (e) =>
                    {
                        this.categoryService.SaveCategory(category);
                    });

                return category;
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public Category UpdateCategory(string scope, string id, string name, string description)
        {
            try
            {
                int scopeInt = Convert.ToInt32(scope);
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                Category category = this.categoryService.GetCategory(scopeInt, id);
                category.Name = name;
                category.Desc = description;
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

    }

}
