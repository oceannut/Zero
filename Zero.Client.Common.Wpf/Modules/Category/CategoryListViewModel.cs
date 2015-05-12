using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Nega.WpfCommon;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Client.Common.Wpf
{

    public class CategoryListViewModel : Screen
    {

        private ICategoryService categoryService;

        private ObservableCollection<TreeNodeModel> categoryList;

        public ObservableCollection<TreeNodeModel> CategoryList
        {
            get { return categoryList; }
            set
            {
                if (categoryList != value)
                {
                    categoryList = value;
                    NotifyOfPropertyChange(() => CategoryList);
                }
            }
        }

        public CategoryListViewModel(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            this.categoryService.ListCategoryAsync(1)
                .ContinueWith((task) =>
                {
                    if (task.Exception == null)
                    {
                        var categories = task.Result;
                        if (categories != null && categories.Count() > 0)
                        {
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                new System.Action(() =>
                                {
                                    this.CategoryList = CategoryViewModel.BuildTree(categories);
                                }));
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(task.Exception.ToString());
                    }
                });
        }

    }

}
