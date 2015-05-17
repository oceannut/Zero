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

        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                if (header != value)
                {
                    header = value;
                    NotifyOfPropertyChange(() => this.Header);
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

        public NavViewModel()
        {

        }

        public NavViewModel(string header, IScreen target)
        {
            this.Header = header;
            this.Target = target;
        }

    }
}
