using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Nega.Modularity;

using Zero.Client.Common.Wpf;

namespace Zero.Client.Wpf
{

    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {

        #region fields

        private readonly IModuleContainer container;
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        #endregion

        #region properties

        #region navigation

        public NavViewModel[] Navs { get; set; }

        private ObservableCollection<NavViewModel> navList;
        public ObservableCollection<NavViewModel> NavList
        {
            get { return navList; }
            set
            {
                if (navList != value)
                {
                    navList = value;
                    NotifyOfPropertyChange(() => this.NavList);
                }
            }
        }

        private NavViewModel selectedNav;
        public NavViewModel SelectedNav
        {
            get { return selectedNav; }
            set
            {
                if (selectedNav != value)
                {
                    selectedNav = value;
                    NotifyOfPropertyChange(() => this.SelectedNav);

                    if (selectedNav != null)
                    {
                        ActivateItem(selectedNav.Target);
                    }
                    else
                    {
                        ActivateItem(null);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region contructors

        public ShellViewModel(IModuleContainer container,
            IWindowManager windowManager,
            IEventAggregator eventAggregator)
        {
            this.container = container;
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
        }

        #endregion

        #region methods

        #region window buttons event handle

        public void MinWindow()
        {
            App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        public void NormalWindow()
        {
            if (System.Windows.WindowState.Normal == App.Current.MainWindow.WindowState)
            {
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            }
        }

        public void CloseWindow()
        {
            App.Current.MainWindow.Close();
        }

        #endregion

        #region overrides

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            this.NavList = new ObservableCollection<NavViewModel>();
            if (this.Navs != null && this.Navs.Length > 0)
            {
                foreach (var nav in this.Navs)
                {
                    this.NavList.Add(nav);
                }
            }
        }

        #endregion

        #endregion

    }

}
