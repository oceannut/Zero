using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Client.Common
{

    public class CategoryClientServiceImpl : ICategoryClientService
    {

        private ICategoryService categoryService;

        public CategoryClientServiceImpl(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public void SaveCategory(Category category, 
            Action<Category> success, 
            Action<Exception> failure)
        {
            this.categoryService.SaveCategoryAsync(category)
                .ContinueWith((saveTask) =>
                {
                    if (saveTask.Exception == null)
                    {
                        success(category);
                    }
                    else
                    {
                        failure(saveTask.Exception);
                    }
                });
        }

    }

}
