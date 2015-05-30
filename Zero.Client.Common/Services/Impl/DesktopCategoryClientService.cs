using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Client.Common
{

    public class DesktopCategoryClientService : ICategoryClientService
    {

        private ICategoryService categoryService;

        public DesktopCategoryClientService(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public void SaveCategory(Category category,
            Action<Category> success,
            Action<Exception> failure)
        {
            category.Id = Guid.NewGuid().ToString();
            this.categoryService.TreeCategoryAsync(category.Scope)
                .ContinueWith((treeTask) =>
                {
                    if (treeTask.Exception == null)
                    {
                        category.Save(treeTask.Result,
                            (e) =>
                            {
                                this.categoryService.SaveCategoryAsync(e)
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
                            });
                    }
                    else
                    {
                        failure(treeTask.Exception);
                    }
                });
        }


        public void UpdateCategory(Category category, 
            Action<Category> success, 
            Action<Exception> failure)
        {
            this.categoryService.TreeCategoryAsync(category.Scope)
                .ContinueWith((treeTask) =>
                {
                    if (treeTask.Exception == null)
                    {
                        category.Update(treeTask.Result,
                            (e) =>
                            {
                                this.categoryService.UpdateCategoryAsync(e)
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
                            });
                    }
                    else
                    {
                        failure(treeTask.Exception);
                    }
                });
        }

        public void DeleteCategory(Category category, 
            Action<Category> success, 
            Action<Exception> failure)
        {
            this.categoryService.TreeCategoryAsync(category.Scope)
                .ContinueWith((treeTask) =>
                {
                    if (treeTask.Exception == null)
                    {
                        category.Delete(treeTask.Result,
                            (e) =>
                            {
                                this.categoryService.DeleteCategoryAsync(e.ToArray())
                                    .ContinueWith((deleteTask) =>
                                    {
                                        if (deleteTask.Exception == null)
                                        {
                                            success(category);
                                        }
                                        else
                                        {
                                            failure(deleteTask.Exception);
                                        }
                                    });
                            });
                    }
                    else
                    {
                        failure(treeTask.Exception);
                    }
                });
        }

        public void ListCategory(int scope, 
            Action<IEnumerable<Category>> success, 
            Action<Exception> failure)
        {
            this.categoryService.ListCategoryAsync(scope)
                .ContinueWith((task) =>
                {
                    if (task.Exception == null)
                    {
                        success(task.Result);
                    }
                    else
                    {
                        failure(task.Exception);
                    }
                });
        }

    }

}
