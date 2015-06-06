using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Validation;

using Nega.Common;
using Nega.Entlib;
using Nega.Modularity;
using Nega.WcfUnity;

using Zero.Service.Rest;
using Zero.BLL;
using Zero.BLL.Impl;
using Zero.DAL;
using Zero.DAL.EF;
using Zero.DAL.Caching;

namespace Zero.Service.IISWASHost
{
    public class Global : System.Web.HttpApplication
    {

        private const string connectionString = "connectionString";

        protected void Application_Start(object sender, EventArgs e)
        {
            string exceptionHandlingLogPolicy = "Log";
            string exceptionHandlingLogAndWrapPolicy = "LogAndWrap";

            IUnityContainer container = ObjectsRegistry.SoloInstance.Container;

            #region interception

            container.AddNewExtension<Interception>();
            container.Configure<Interception>().AddPolicy("Save")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Save*")))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(exceptionHandlingLogAndWrapPolicy))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            container.Configure<Interception>().AddPolicy("Update")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Update*")))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(exceptionHandlingLogAndWrapPolicy))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            container.Configure<Interception>().AddPolicy("Delete")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Delete*")))
                 .AddCallHandler<LogCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(9001, true, false,
                  "Pending Deletion",
                  "Delete successfully", true, false, true, 10))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(exceptionHandlingLogAndWrapPolicy))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());

            container.Configure<Interception>().AddPolicy("Wcf")
                 .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor(new InjectionParameter("Zero.Service.Rest")))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(exceptionHandlingLogPolicy));

            #endregion

            #region DAL

            container.RegisterType<ICacheFactory, PermanentCacheFactory>("PermanentCacheFactory", new ContainerControlledLifetimeManager());
            container.RegisterType<CacheManager>("PermanentCacheManager", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(container.Resolve<ICacheFactory>("PermanentCacheFactory")));

            container.RegisterType<IUserDao, UserDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<IRoleDao, RoleDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));

            container.RegisterType<CategoryDao>("CategoryDao", new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<ICategoryDao, CategoryCache>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(container.Resolve<CategoryDao>("CategoryDao"), container.Resolve<CacheManager>("PermanentCacheManager")));

            (container.Resolve<CategoryCache>() as IModule).Initialize();

            #endregion

            #region BLL

            container.RegisterType<IUserService, UserServiceImpl>(new ContainerControlledLifetimeManager(), 
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
            container.RegisterType<ICategoryService, CategoryServiceImpl>(new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            #endregion

            #region Wcf

            container.RegisterType<ServiceAuthorizationManager, WebServiceAuthorizationManager>();

            container.RegisterType<ISignService, SignService>(
                new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
            container.RegisterType<ICategoryRestService, CategoryRestServiceImpl>();

            #endregion

            #region logging

            LogWriterFactory logWriterFactory = new LogWriterFactory();
            LogWriter logWriter = logWriterFactory.Create();
            Logger.SetLogWriter(logWriter, false);

            #endregion

            #region exception handling

            ExceptionPolicyFactory policyFactory = new ExceptionPolicyFactory();
            ExceptionManager exceptionManager = policyFactory.CreateManager();
            ExceptionPolicy.SetExceptionManager(exceptionManager);

            #endregion

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}