using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using Nega.WpfCommon;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Client.Common.Wpf
{

    public class CategoryListViewModel : Conductor<IScreen>.Collection.OneActive
    {

        private readonly ICategoryService categoryService;
        private readonly IEventAggregator eventAggregator;
        private readonly int scope;
        private CategoryViewModel selectedItem;
        //private CategoryViewModel newViewModel;

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

        public CategoryListViewModel(ICategoryService categoryService, 
            IEventAggregator eventAggregator,
            int scope)
        {
            this.categoryService = categoryService;
            this.eventAggregator = eventAggregator;
            this.scope = scope;
        }

        public void SelectCategory(TreeNodeModel selectedItem)
        {
            this.selectedItem = selectedItem as CategoryViewModel;
        }

        public void AddRootCategory()
        {
            CategoryViewModel newViewModel = new CategoryViewModel();
            newViewModel.Scope = this.scope;
            newViewModel.Sequence = this.CategoryList.Count + 1;
            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this, newViewModel));
        }

        public void AddChildCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择要添加子节点的父节点");
                return;
            }

            CategoryViewModel newViewModel = new CategoryViewModel();
            newViewModel.Scope = this.scope;
            newViewModel.Sequence = this.selectedItem.Children.Count + 1;
            newViewModel.Parent = this.selectedItem;
            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this, newViewModel));
        }

        public void EditCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择编辑的节点");
                return;
            }

            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this, this.selectedItem));
        }

        public void RemoveCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择要删除的节点");
                return;
            }
            if (MessageBox.Show(string.Format("您确定要删除\"{0}\"?", this.selectedItem.Name), "删除对话框", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }
        }

        public void RefreshCategory()
        {
            LoadCategoryList();
        }

        public void Detail_Drop(object sender, DragEventArgs e)
        {
            CategoryViewModel category = e.Data.GetData(typeof(CategoryViewModel)) as CategoryViewModel;
            if (category == null)
            {
                return;
            }

            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator, 
                this, category));
        }

        public void Node_Drop(object sender, DragEventArgs e)
        {
            CategoryViewModel category = e.Data.GetData(typeof(CategoryViewModel)) as CategoryViewModel;
            if (category == null)
            {
                return;
            }
        }

        internal void RefreshWhenSave(CategoryViewModel newViewModel)
        {
            if (this.selectedItem == null)
            {
                this.CategoryList.Add(newViewModel);
            }
            else
            {
                this.selectedItem.AddChild(newViewModel);
            }
        }

        internal void ClearDetailWhenCancel()
        {
             for (int i = this.Items.Count - 1; i >= 0; i--)
             {
                 DeactivateItem(this.Items[i], true);
             }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            LoadCategoryList();
        }

        private void LoadCategoryList()
        {
            CategoryViewModel.ClearTree(this.CategoryList);
            this.categoryService.ListCategoryAsync(this.scope)
                .ExcuteOnUIThread<IEnumerable<Category>>(
                    (e) =>
                    {
                        if (e != null && e.Count() > 0)
                        {
                            this.CategoryList = CategoryViewModel.BuildTree(e);
                        }
                    },
                    (ex) =>
                    {
                        MessageBox.Show(ex.ToString());
                    });
        }

    }

}
