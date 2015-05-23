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
        private readonly CategoryListViewModel summary;
        private CategoryViewModel current;

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
            CategoryListViewModel summary,
            CategoryViewModel current)
        {
            this.categoryService = categoryService;
            this.summary = summary;
            this.current = current;

            this.Name = this.current.Model.Name;
            this.Desc = this.current.Model.Desc;
        }

        public void Save()
        {
            Category category = this.current.Model;
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                category.Id = Guid.NewGuid().ToString();
                category.Name = Name;
                category.Desc = Desc;
                this.categoryService.TreeCategoryAsync(category.Scope)
                    .ContinueWith((task) =>
                    {
                        if (task.Exception == null)
                        {
                            category.Save(task.Result,
                                (e) =>
                                {
                                    this.categoryService.SaveCategoryAsync(e)
                                        .ExcuteOnUIThread(
                                        () =>
                                        {
                                            this.current.Name = name;
                                            this.summary.RefreshWhenSave(this.current);
                                            if (MessageBox.Show("是否继续添加类型?", "添加类型", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                            {
                                                CategoryViewModel nextViewModel = this.current.Next();
                                                this.current = nextViewModel;
                                            }
                                            else
                                            {
                                                this.summary.ClearDetail();
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
                            MessageBox.Show("加载类型树失败: " + task.Exception);
                        }
                    });
            }
            else
            {
                category.ChangeNameAndDesc(Name, Desc,
                    (e) =>
                    {
                        this.categoryService.UpdateCategoryAsync(e)
                            .ExcuteOnUIThread(
                            () =>
                            {
                                this.current.Name = name;
                                this.summary.ClearDetail();
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
            this.summary.ClearDetail();
        }

    }

}
