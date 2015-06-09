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

        //private readonly ICategoryClientService categoryClientService;
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

        //public CategoryDetailsViewModel(ICategoryClientService categoryClientService,
        //    CategoryListViewModel summary,
        //    CategoryViewModel current)
        //{
        //    this.categoryClientService = categoryClientService;
        //    this.summary = summary;
        //    this.current = current;

        //    this.Name = this.current.Model.Name;
        //    this.Desc = this.current.Model.Desc;
        //}

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
            category.Name = Name;
            category.Desc = Desc;
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                category.Id = Guid.NewGuid().ToString();
                //this.categoryClientService.SaveCategory(category,
                //    (result) =>
                //    {
                //        UIThreadHelper.BeginInvoke(
                //            () =>
                //            {
                //                this.current.Name = result.Name;
                //                this.summary.RefreshWhenSave(this.current);
                //                if (MessageBox.Show("是否继续添加类型?", "添加类型", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //                {
                //                    CategoryViewModel nextViewModel = this.current.Next();
                //                    this.current = nextViewModel;
                //                }
                //                else
                //                {
                //                    this.summary.ClearDetail();
                //                }
                //            });
                //    },
                //    (ex) =>
                //    {
                //        MessageBox.Show("保存失败: " + ex.Message);
                //    });
            }
            else
            {
                //this.categoryClientService.UpdateCategory(category,
                //    (result) =>
                //    {
                //        UIThreadHelper.BeginInvoke(
                //            () =>
                //            {
                //                this.current.Name = name;
                //                this.summary.ClearDetail();
                //            });
                //    },
                //    (ex) =>
                //    {
                //        MessageBox.Show("更新失败: " + ex.Message);
                //    });
            }
        }

        public void Cancel()
        {
            this.summary.ClearDetail();
        }

    }

}
