using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

namespace Zero.Client.Common.Wpf
{

    public class ClosableViewModel : Screen
    {

        private System.Windows.Visibility closeButtonVisibility;
        public System.Windows.Visibility CloseButtonVisibility
        {
            get { return closeButtonVisibility; }
            set
            {
                if (closeButtonVisibility != value)
                {
                    closeButtonVisibility = value;
                    NotifyOfPropertyChange(() => CloseButtonVisibility);
                }
            }
        }

    }

}
