using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using Nega.Common;
using Nega.Entlib;
using Nega.Modularity;

using Zero.DAL;
using Zero.DAL.EF;
using Zero.DAL.Caching;
using Zero.BLL;
using Zero.BLL.Impl;
using Zero.Service.Rest;

namespace Zero.Service.IISWASHost
{

    public class ZeroObjects : IObjectsConfig
    {

        private const string connectionString = "connectionString";

        public void Config(IUnityContainer container)
        {

            #region DAL

            container.RegisterType<IAuditEntryDao, AuditEntryDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<IAuditWriter, AuditEntryDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));

            container.RegisterType<IUserDao, UserDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<IRoleDao, RoleDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));

            container.RegisterType<CategoryDao>("CategoryDao", new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<ICategoryDao, CategoryCache>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(container.Resolve<CategoryDao>("CategoryDao"), container.Resolve<CacheManager>("PermanentCacheManager")));

            (container.Resolve<CategoryCache>() as IModule).Initialize();

            #endregion

            #region BLL

            container.RegisterType<IResourceAccessService, ResourceAccessServiceImpl>(new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            container.RegisterType<IUserService, UserServiceImpl>(new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            container.RegisterType<IAuthenticationProvider, UserServiceImpl>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICategoryService, CategoryServiceImpl>(new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            container.RegisterType<IResourceAuthorizationProvider, ResourceAuthorizationProvider>(new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>());

            #endregion

            #region Wcf

            container.RegisterType<ISignRestService, SignRestServiceImpl>();
            container.RegisterType<ICategoryRestService, CategoryRestServiceImpl>();

            #endregion

            AuditManager.Auditor = container.Resolve<IAuditor>();

        }

    }

}