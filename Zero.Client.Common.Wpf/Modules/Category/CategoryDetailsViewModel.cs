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

        private ICategoryService categoryService;
        private IEventAggregator eventAggregator;
        private CategoryViewModel category;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        private string desc;

        public string Desc
        {
            get { return desc; }
            set
            {
                if (desc != value)
                {
                    desc = value;
                    NotifyOfPropertyChange(() => Desc);
                }
            }
        }

        public CategoryDetailsViewModel(ICategoryService categoryService, 
            IEventAggregator eventAggregator,
            CategoryViewModel category)
        {
            this.categoryService = categoryService;
            this.eventAggregator = eventAggregator;
            this.category = category;

            this.Name = category.Model.Name;
            this.Desc = category.Model.Desc;
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(this.category.Model.Id))
            {
                Category entity = this.category.Model;
                entity.Id = Guid.NewGuid().ToString();
                entity.Name = this.Name;
                entity.Desc = this.Desc;
                entity.Save(
                    (e) => 
                    {
                        this.categoryService.SaveCategoryAsync(this.category.Model)
                            .ExcuteOnUIThread(
                            () =>
                            {
                                this.eventAggregator.PublishOnUIThread(new CategoryEditEvent(CategoryEditAction.Save, this.category));
                            },
                            (ex) =>
                            {
                                this.eventAggregator.PublishOnUIThread(new CategoryEditEvent(CategoryEditAction.Save, this.category, ex));
                                MessageBox.Show("保存失败: " + ex.Message);
                            });
                    });
                
            }
            else
            {
                this.category.Model.ChangeNameAndDesc(this.Name, this.Desc, 
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
            this.eventAggregator.PublishOnUIThread(new CategoryEditEvent(CategoryEditAction.Cancel, this.category));
        }

    }

}
