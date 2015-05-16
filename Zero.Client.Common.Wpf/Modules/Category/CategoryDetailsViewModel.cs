using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using Nega.WpfCommon;

using Zero.BLL;

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
            IEventAggregator eventAggregator)
            : this(categoryService, eventAggregator, new CategoryViewModel())
        {
            
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
                this.category.Model.Name = this.Name;
                this.category.Model.Desc = this.Desc;
                this.category.Model.Save(
                    (e) => 
                    {
                        this.categoryService.SaveCategoryAsync(this.category.Model)
                            .ExcuteOnUIThread(
                            () =>
                            {
                            },
                            (ex) =>
                            {
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
                            });
                    });
            }
        }

        public void Cancel()
        {
            this.eventAggregator.PublishOnUIThread("CloseCategoryDetail");
        }

    }

}
