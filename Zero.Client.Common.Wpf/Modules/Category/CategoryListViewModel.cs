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
using System.Windows;

namespace Zero.Client.Common.Wpf
{

    public class CategoryListViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<CategoryEditEvent>
    {

        private readonly ICategoryService categoryService;
        private readonly IEventAggregator eventAggregator;
        private readonly int scope;
        private CategoryViewModel selectedItem;
        private CategoryViewModel newViewModel;

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

            this.eventAggregator.Subscribe(this);
        }

        public void SelectCategory(TreeNodeModel selectedItem)
        {
            this.selectedItem = selectedItem as CategoryViewModel;
        }

        public void AddRootCategory()
        {
            this.newViewModel = new CategoryViewModel();
            this.newViewModel.Model.Scope = this.scope;
            this.newViewModel.Model.Sequence = this.CategoryList.Count + 1;
            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this.newViewModel));
        }

        public void AddChildCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择要添加子节点的父节点");
                return;
            }

            this.newViewModel = new CategoryViewModel();
            this.newViewModel.Model.Scope = this.scope;
            this.newViewModel.Model.Sequence = this.selectedItem.Children.Count + 1;
            this.newViewModel.Model.Parent = this.selectedItem.Model;
            //this.newViewModel.Parent = this.selectedItem;
            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this.newViewModel));
        }

        public void EditCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择编辑的节点");
                return;
            }

            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator,
                this.selectedItem));
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

        public void Detail_Drop(object sender, DragEventArgs e)
        {
            CategoryViewModel category = e.Data.GetData(typeof(CategoryViewModel)) as CategoryViewModel;
            if (category == null)
            {
                return;
            }

            ActivateItem(new CategoryDetailsViewModel(this.categoryService, this.eventAggregator, 
                category));
        }

        public void Node_Drop(object sender, DragEventArgs e)
        {
            CategoryViewModel category = e.Data.GetData(typeof(CategoryViewModel)) as CategoryViewModel;
            if (category == null)
            {
                return;
            }
        }

        public void Handle(CategoryEditEvent message)
        {
            switch(message.Action)
            {
                case CategoryEditAction.Save:
                    if (message.Exception == null && this.CategoryList != null)
                    {
                        if (this.selectedItem == null)
                        {
                            this.CategoryList.Add(this.newViewModel);
                        }
                        else
                        {
                            this.selectedItem.AddChild(this.newViewModel);
                        }
                        this.newViewModel = null;
                    }
                    break;
                case CategoryEditAction.Cancel:
                    for (int i = this.Items.Count - 1; i >= 0; i--)
                    {
                        DeactivateItem(this.Items[i], true);
                    }
                    break;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            this.categoryService.ListCategoryAsync(1)
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
