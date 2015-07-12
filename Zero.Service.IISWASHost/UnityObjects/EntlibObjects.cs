using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;

using Nega.Entlib;

namespace Zero.Service.IISWASHost
{

    public class EntlibObjects : IObjectsConfig
    {

        public void Config(IUnityContainer container)
        {

            string exceptionHandlingLogAndWrapPolicy = "LogAndWrap";

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
            container.Configure<Interception>().AddPolicy("List")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("List*")))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(exceptionHandlingLogAndWrapPolicy))
                 .AddCallHandler<ResourceAuthorizationCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(container));

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

    }

}