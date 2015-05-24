using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Service.Rest
{
    public class CategoryQueryService : ICategoryQueryService
    {

        private readonly Zero.BLL.ICategoryService categoryService;

        public CategoryQueryService(Zero.BLL.ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public Category[] GetCategories(string scope)
        {
            if (string.IsNullOrWhiteSpace(scope))
            {
                throw new WebFaultException<string>("应用范围不能为空", HttpStatusCode.BadRequest);
            }
            int scopeInt = 0;
            try
            {
                scopeInt = Convert.ToInt32(scope);
            }
            catch
            {
                throw new WebFaultException<string>("应用范围为一个正整数值", HttpStatusCode.BadRequest);
            }

            try
            {
                return this.categoryService.ListCategory(scopeInt, null, false).ToArray();
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

    }
}
