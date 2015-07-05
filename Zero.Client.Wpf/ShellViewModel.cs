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

    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<SigninEvent>
    {

        #region fields

        private const double windowHeight = 620;
        private const double windowWidth = 1000;

        private readonly IModuleContainer container;
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        private readonly SigninViewModel signinViewModel;

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
            IEventAggregator eventAggregator,
            SigninViewModel signinViewModel)
        {
            this.container = container;
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;

            this.signinViewModel = signinViewModel;
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

        public void Handle(SigninEvent message)
        {
            this.NavList = new ObservableCollection<NavViewModel>();
            if (this.Navs != null && this.Navs.Length > 0)
            {
                foreach (var nav in this.Navs)
                {
                    this.NavList.Add(nav);
                }
            }
        }

        #region overrides

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            App.Current.MainWindow.Height = 0;
            App.Current.MainWindow.Width = 0;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            this.eventAggregator.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("WindowStyle", System.Windows.WindowStyle.None);
            bool? dialogResult = this.windowManager.ShowDialog(this.signinViewModel, null, properties);
            if (dialogResult.HasValue && dialogResult.Value)
            {
                App.Current.MainWindow.Top = App.Current.MainWindow.Top - windowHeight / 2;
                App.Current.MainWindow.Left = App.Current.MainWindow.Left - windowWidth / 2;
                App.Current.MainWindow.Height = windowHeight;
                App.Current.MainWindow.Width = windowWidth;
            }
            else
            {
                CloseWindow();
            }
        }

        protected override void OnDeactivate(bool close)
        {
            this.eventAggregator.Unsubscribe(this);

            base.OnDeactivate(close);
        }

        #endregion

        #endregion

    }

}
