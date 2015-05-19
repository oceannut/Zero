﻿using System;
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
using Zero.Client.Common.Wpf;

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

            this.container.RegisterType<ICacheFactory, PermanentCacheFactory>("PermanentCacheFactory", new ContainerControlledLifetimeManager());
            this.container.RegisterType<CacheManager>("PermanentCacheManager", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(this.container.Resolve<ICacheFactory>("PermanentCacheFactory")));

            this.container.RegisterType<CategoryDao>("CategoryDao", new ContainerControlledLifetimeManager(), new InjectionConstructor(connectionString));
            this.container.RegisterType<ICategoryDao, CategoryCache>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(this.container.Resolve<CategoryDao>("CategoryDao"), this.container.Resolve<CacheManager>("PermanentCacheManager")));

            (this.container.Resolve<CategoryCache>() as IModule).Initialize();

            this.container.RegisterType<ICategoryService, CategoryServiceImpl>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<CategoryListViewModel>("CategoryListViewModel1", new InjectionConstructor(this.container.Resolve<ICategoryService>(),
                this.container.Resolve<IEventAggregator>(),
                1));
            this.container.RegisterType<CategoryListViewModel>("CategoryListViewModel2", new InjectionConstructor(this.container.Resolve<ICategoryService>(),
                this.container.Resolve<IEventAggregator>(),
                2));
            

            this.container.RegisterType<IModuleContainer, SimpleModuleContainer>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<ShellViewModel>(new InjectionProperty("Navs",
                    new ResolvedArrayParameter<NavViewModel>(
                        new NavViewModel("车辆分类", this.container.Resolve<CategoryListViewModel>("CategoryListViewModel1")),
                        new NavViewModel("案件分类", this.container.Resolve<CategoryListViewModel>("CategoryListViewModel2"))
                        )));

        }

    }

}