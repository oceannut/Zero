using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Nega.Entlib;
using Nega.Common;

namespace Zero.Client.Wpf
{

    public class CommonObjects : IObjectsConfig
    {

        public void Config(IUnityContainer container)
        {

            container.RegisterType<ILoggerFactory, LoggerFactoryImpl>(new ContainerControlledLifetimeManager(), new InjectionConstructor("ErrorCategory"));
            LogManager.Factory = container.Resolve<ILoggerFactory>();

            
        }

    }

}