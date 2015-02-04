using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;

using Nega.WcfUnity;

using Zero.Service.Rest;
using Zero.BLL;
using Zero.BLL.Managers;
using Zero.DAL;
using Zero.DAL.EF.MySQL;
using Nega.Entlib;

namespace Zero.Service.IISWASHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Console.WriteLine("app start");

            IUnityContainer container = ObjectsRegistry.SoloInstance.Container;

            container.AddNewExtension<Interception>();

            container.Configure<Interception>().AddPolicy("Save")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Save*")))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            container.Configure<Interception>().AddPolicy("Update")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Update*")))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());

            container.RegisterType<IUserDao, UserDao>();
            container.RegisterType<IRoleDao, RoleDao>();

            //container.RegisterType<IUserService, UserManager>();
            container.RegisterType<IUserService, UserManager>(new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            container.RegisterType<ISignService, SignService>();

            PolicyInjection.SetPolicyInjector(new PolicyInjector(new SystemConfigurationSource(false)), false);

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