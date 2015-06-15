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
using Zero.Client.Common;

namespace Zero.Client.Common.Wpf
{
    public class CategoryDetailsViewModel : Screen
    {

        private readonly ICategoryClient categoryClient;
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

        public CategoryDetailsViewModel(ICategoryClient categoryClient,
            CategoryListViewModel summary,
            CategoryViewModel current)
        {
            this.categoryClient = categoryClient;
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
                this.categoryClient.SaveCategoryAsync(category.Scope, Name, Desc, null)
                    .ContinueWith(
                        (task)=>
                        {
                            if (task.Exception == null)
                            {
                                UIThreadHelper.BeginInvoke(
                                    () =>
                                    {
                                        this.current.Model = task.Result;
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
                                    });
                            }
                            else
                            {
                                MessageBox.Show("保存失败: " + task.Exception);
                            }
                        });
            }
            else
            {
                this.categoryClient.UpdateCategoryAsync(category.Scope, category.Id, Name, Desc)
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Exception == null)
                            {
                                UIThreadHelper.BeginInvoke(
                                    () =>
                                    {
                                        this.current.Model = task.Result;
                                        this.summary.ClearDetail();
                                    });
                            }
                            else
                            {
                                MessageBox.Show("更新失败: " + task.Exception);
                            }
                        });
            }
        }

        public void Cancel()
        {
            this.summary.ClearDetail();
        }

    }

}
