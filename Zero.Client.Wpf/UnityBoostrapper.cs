using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Caliburn.Micro;

using Nega.Common;
using Nega.Modularity;

using Zero.Domain;
using Zero.DAL;
using Zero.DAL.EF;
using Zero.DAL.Caching;
using Zero.BLL;
using Zero.BLL.Impl;

namespace Zero.Client.Wpf
{

    public class UnityBoostrapper : BootstrapperBase
    {

        private const string connectionString = "connectionString";

        private IUnityContainer container;
        private readonly List<IModule> modules = new List<IModule>();

        public UnityBoostrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            this.container = new UnityContainer();
            
            RegisterTypes();
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

        private void RegisterTypes()
        {

            //CacheManager permanentCacheManager = new CacheManager();
            //permanentCacheManager.Factory = new PermanentCacheFactory();

            container.RegisterType<ICategoryDao, CategoryDao>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));

            //ICategoryDao categoryDao = new CategoryDao("connectionString");
            //this.container.RegisterInstance<ICategoryDao>(new CategoryCache(categoryDao, permanentCacheManager), new ContainerControlledLifetimeManager());

            this.container.RegisterType<ICategoryService, CategoryServiceImpl>(new ContainerControlledLifetimeManager());
            
            

            this.container.RegisterType<Zero.Client.Common.Wpf.CategoryListViewModel>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<IModuleContainer, SimpleModuleContainer>(new ContainerControlledLifetimeManager());

            this.container.RegisterInstance<IWindowManager>(new WindowManager());
            this.container.RegisterInstance<IEventAggregator>(new EventAggregator());
            this.container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());

            

        }

    }

}
