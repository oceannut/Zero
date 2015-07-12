using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Microsoft.Practices.Unity;

using Nega.Entlib;
using Nega.WcfUnity;

namespace Zero.Service.IISWASHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            ObjectsFactory factory = ObjectsFactory.SoloInstance;
            factory.AddConfig(new EntlibObjects());
            factory.AddConfig(new CommonObjects());
            factory.AddConfig(new ZeroObjects());

            IUnityContainer container = ObjectsRegistry.SoloInstance.Container;
            factory.Registrate(container);

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