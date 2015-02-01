using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Microsoft.Practices.Unity;

using Nega.WcfUnity;

using Zero.Service.Rest;
using Zero.BLL;
using Zero.BLL.Managers;
using Zero.DAL;
using Zero.DAL.EF.MySQL;

namespace Zero.Service.IISWASHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Console.WriteLine("app start");

            IUnityContainer container = ObjectsRegistry.SoloInstance.Container;
            container.RegisterType<ISignService, SignService>();
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