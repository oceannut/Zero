using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

using Microsoft.Practices.Unity;

using Nega.Entlib;
using Nega.Common;
using Nega.WcfCommon;
using Nega.WcfUnity;

namespace Zero.Service.IISWASHost
{

    public class CommonObjects : IObjectsConfig
    {

        public void Config(IUnityContainer container)
        {

            container.RegisterType<ILoggerFactory, LoggerFactoryImpl>(new ContainerControlledLifetimeManager(), new InjectionConstructor("ErrorCategory"));
            LogManager.Factory = container.Resolve<ILoggerFactory>();

            container.RegisterType<IAuditor, GenericAuditor>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICacheFactory, PermanentCacheFactory>("PermanentCacheFactory", new ContainerControlledLifetimeManager());
            container.RegisterType<CacheManager>("PermanentCacheManager", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(container.Resolve<ICacheFactory>("PermanentCacheFactory")));

            container.RegisterType<ICredentialsProvider, GenericCredentialsProvider>(new ContainerControlledLifetimeManager());

            WebClientManager webClientManager = new WebClientManager();
            container.RegisterInstance<IClientManager>(webClientManager);
            container.RegisterInstance<IClientFinder>(webClientManager);

            container.RegisterType<ServiceAuthorizationManager, WebServiceAuthorizationManager>(new ContainerControlledLifetimeManager());

        }

    }

}