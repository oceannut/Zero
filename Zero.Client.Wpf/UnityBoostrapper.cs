using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Caliburn.Micro;

using Nega.Entlib;

namespace Zero.Client.Wpf
{

    public class UnityBoostrapper : BootstrapperBase
    {

        private IUnityContainer container;
        //private readonly List<IModule> modules = new List<IModule>();

        public UnityBoostrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            this.container = new UnityContainer();

            ObjectsFactory factory = ObjectsFactory.SoloInstance;
            factory.AddConfig(new CaliburnObjects());
            factory.AddConfig(new CommonObjects());
            factory.AddConfig(new ZeroObjects());

            factory.Registrate(container);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(Zero.Client.Common.Wpf.ModuleName)),
            };
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return this.container.Resolve(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.ResolveAll(service);
        }

    }

}
