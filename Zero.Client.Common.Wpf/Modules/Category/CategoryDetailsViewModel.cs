using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using Nega.WpfCommon;

using Zero.BLL;
using Zero.Domain;

namespace Zero.Client.Common.Wpf
{
    public class CategoryDetailsViewModel : Screen
    {

        private readonly ICategoryService categoryService;
        private readonly IEventAggregator eventAggregator;
        private readonly CategoryListViewModel summary;

        private CategoryViewModel current;
        public CategoryViewModel Current
        {
            get { return this.current; }
            set
            {
                if (current != value)
                {
                    current = value;
                    NotifyOfPropertyChange(() => Current);
                }
            }
        }

        public CategoryDetailsViewModel(ICategoryService categoryService, 
            IEventAggregator eventAggregator,
            CategoryListViewModel summary,
            CategoryViewModel current)
        {
            this.categoryService = categoryService;
            this.eventAggregator = eventAggregator;
            this.summary = summary;
            this.current = current;
        }

        public void Save()
        {
            Category category = this.Current.Model;
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                category.Id = Guid.NewGuid().ToString();
                category.Save(
                    (e) => 
                    {
                        this.categoryService.SaveCategoryAsync(e)
                            .ExcuteOnUIThread(
                            () =>
                            {
                                this.summary.RefreshWhenSave(this.current);
                                this.eventAggregator.PublishOnUIThread(new CategoryEditEvent(CategoryEditAction.Save, this.current));
                                if (MessageBox.Show("是否继续添加类型?", "添加类型", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    CategoryViewModel nextViewModel = this.Current.Next();
                                    this.Current = nextViewModel;
                                }
                                else
                                {
                                    this.summary.ClearDetailWhenCancel();
                                }
                            },
                            (ex) =>
                            {
                                MessageBox.Show("保存失败: " + ex.Message);
                            });
                    });
                
            }
            else
            {
                category.Update(
                    (e) =>
                    {
                        this.categoryService.UpdateCategoryAsync(e)
                            .ExcuteOnUIThread(
                            () =>
                            {
                            },
                            (ex) =>
                            {
                                MessageBox.Show("更新失败: " + ex.Message);
                            });
                    });
            }
        }

        public void Cancel()
        {
            this.summary.ClearDetailWhenCancel();
        }

    }

}
