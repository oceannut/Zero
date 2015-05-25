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

        public Category SaveCategory(Category category)
        {
            try
            {
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(category.Scope);
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

        public Category UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(string id)
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
                if (scope == "0")
                {
                    return this.categoryService.ListCategory(null).ToArray();
                }
                else
                {
                    return this.categoryService.ListCategory(Convert.ToInt32(scope)).ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }




    }

}
