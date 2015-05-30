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
using Nega.Common;

namespace Zero.Client.Common.Wpf
{

    public class CategoryListViewModel : Conductor<IScreen>.Collection.OneActive
    {

        private readonly ICategoryClientService categoryClientService;
        private readonly int scope;
        private CategoryViewModel selectedItem;

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

        public CategoryListViewModel(ICategoryClientService categoryClientService,
            int scope)
        {
            if (categoryClientService == null)
            {
                throw new ArgumentNullException();
            }

            this.categoryClientService = categoryClientService;
            this.scope = scope;
        }

        public void SelectCategory(TreeNodeModel selectedItem)
        {
            this.selectedItem = selectedItem as CategoryViewModel;
        }

        public void AddRootCategory()
        {
            int sequence = this.CategoryList == null ? 1 : this.CategoryList.Count + 1;
            CategoryViewModel viewModel = new CategoryViewModel(this.scope, sequence);
            ActivateItem(new CategoryDetailsViewModel(this.categoryClientService, this, viewModel));
        }

        public void AddChildCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择要添加子节点的父节点");
                return;
            }

            CategoryViewModel viewModel = new CategoryViewModel(this.scope, this.selectedItem.Children.Count + 1, this.selectedItem.Model);
            ActivateItem(new CategoryDetailsViewModel(this.categoryClientService, this, viewModel));
        }

        public void EditCategory()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择编辑的节点");
                return;
            }

            ActivateItem(new CategoryDetailsViewModel(this.categoryClientService, this, this.selectedItem));
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

            Category category = this.selectedItem.Model;
            this.categoryClientService.DeleteCategory(category,
                (result) =>
                {
                    UIThreadHelper.BeginInvoke(
                        () =>
                        {
                            Tree.PostorderTraverse(this.selectedItem,
                                (node) =>
                                {
                                    CategoryViewModel viewModel = node as CategoryViewModel;
                                    if (viewModel.Children != null && viewModel.Children.Count > 0)
                                    {
                                        for (int i = viewModel.Children.Count - 1; i >= 0; i--)
                                        {
                                            viewModel.RemoveChildAt(i);
                                        }
                                    }
                                });
                            if (this.selectedItem.Parent == null)
                            {
                                this.CategoryList.Remove(this.selectedItem);
                            }
                            else
                            {
                                this.selectedItem.Parent.RemoveChild(this.selectedItem);
                            }
                        });
                },
                (ex) =>
                {
                    MessageBox.Show("删除失败: " + ex.Message);
                });
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

            ActivateItem(new CategoryDetailsViewModel(this.categoryClientService, this, category));
        }

        public void Node_Drop(object sender, DragEventArgs e)
        {
            CategoryViewModel category = e.Data.GetData(typeof(CategoryViewModel)) as CategoryViewModel;
            if (category == null)
            {
                return;
            }
        }

        internal void RefreshWhenSave(CategoryViewModel viewModel)
        {
            if (viewModel.Model.Parent == null)
            {
                this.CategoryList.Add(viewModel);
            }
            else
            {
                TreeNodeModel parent = Tree.Find(this.CategoryList,
                            (e) => (e as CategoryViewModel).Model.Id == (viewModel as CategoryViewModel).Model.Parent.Id) as TreeNodeModel;
                parent.AddChild(viewModel);
            }
        }

        internal void ClearDetail()
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
            CategoryViewModel.CleanupTree(this.CategoryList);
            this.categoryClientService.ListCategory(this.scope,
                (result) =>
                {
                    UIThreadHelper.BeginInvoke(
                        () =>
                        {
                            if (result != null && result.Count() > 0)
                            {
                                this.CategoryList = CategoryViewModel.BuildTree(result);
                            }
                        });
                },
                (ex) =>
                {
                    MessageBox.Show("查询失败: " + ex.Message);
                });
        }

    }

}
