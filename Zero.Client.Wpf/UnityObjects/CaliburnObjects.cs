using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Caliburn.Micro;

using Nega.Entlib;

namespace Zero.Client.Wpf
{

    public class CaliburnObjects : IObjectsConfig
    {

        public void Config(IUnityContainer container)
        {
            container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
        }

    }

}
