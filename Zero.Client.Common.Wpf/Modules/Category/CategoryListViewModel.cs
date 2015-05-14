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

    public class CategoryListViewModel : Conductor<IScreen>.Collection.OneActive
    {

        private ICategoryService categoryService;
        private TreeNodeModel selectedItem;

        private ObservableCollection<TreeNodeModel> categoryList;
        /// <summary>
        /// 
        /// </summary>
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

        public void SelectCategory(TreeNodeModel selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void AddCategory()
        {
            ActivateItem(new CategoryDetailsViewModel());
        }

        public void EditCategory()
        {
            if (this.selectedItem == null)
            {
                System.Windows.MessageBox.Show("请选择编辑的节点");
                return;
            }
            System.Windows.MessageBox.Show(this.selectedItem.Name);
        }

        public void RemoveCategory()
        {

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
