using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

namespace Zero.Client.Common.Wpf
{
    public class NavViewModel : PropertyChangedBase
    {

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    NotifyOfPropertyChange(() => this.IsActive);
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyOfPropertyChange(() => this.Name);
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyOfPropertyChange(() => this.Title);
                }
            }
        }

        private object data;
        public object Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    NotifyOfPropertyChange(() => this.Data);
                }
            }
        }

        private IScreen target;
        public IScreen Target
        {
            get { return target; }
            set
            {
                if (target != value)
                {
                    target = value;
                    NotifyOfPropertyChange(() => this.Target);
                }
            }
        }

    }
}
