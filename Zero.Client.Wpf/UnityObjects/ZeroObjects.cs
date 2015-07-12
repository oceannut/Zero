using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Nega.Entlib;
using Nega.Common;

using Zero.Client.Common;
using Zero.Client.Common.Wpf;
using Nega.Modularity;

namespace Zero.Client.Wpf
{

    public class ZeroObjects : IObjectsConfig
    {

        private const string url = "http://localhost:49938";

        public void Config(IUnityContainer container)
        {

            #region services

            container.RegisterType<ISignClient, WebSignClient>(new ContainerControlledLifetimeManager(), new InjectionConstructor(url));
            container.RegisterType<ICategoryClient, WebCategoryClient>(new ContainerControlledLifetimeManager(), new InjectionConstructor(url));

            #endregion

            #region view models

            container.RegisterType<SigninViewModel>();

            container.RegisterType<CategoryListViewModel>("CategoryListViewModel1",
                new InjectionConstructor(container.Resolve<ICategoryClient>(), 1));
            container.RegisterType<CategoryListViewModel>("CategoryListViewModel2",
                new InjectionConstructor(container.Resolve<ICategoryClient>(), 2));
            container.RegisterType<CategoryListViewModel>("CategoryListViewModel100",
                new InjectionConstructor(container.Resolve<ICategoryClient>(), 100));

            container.RegisterType<UserListViewModel>();
            container.RegisterType<RoleListViewModel>();
            container.RegisterType<ResourceAccessListViewModel>();

            container.RegisterType<IModuleContainer, SimpleModuleContainer>(new ContainerControlledLifetimeManager());

            container.RegisterType<ShellViewModel>(new InjectionProperty("Navs",
                    new ResolvedArrayParameter<NavViewModel>(
                        new NavViewModel("车辆分类", container.Resolve<CategoryListViewModel>("CategoryListViewModel1")),
                        new NavViewModel("案件分类", container.Resolve<CategoryListViewModel>("CategoryListViewModel2")),
                        new NavViewModel("部门管理", container.Resolve<CategoryListViewModel>("CategoryListViewModel100")),
                        new NavViewModel("用户管理", container.Resolve<UserListViewModel>()),
                        new NavViewModel("角色管理", container.Resolve<RoleListViewModel>()),
                        new NavViewModel("权限管理", container.Resolve<ResourceAccessListViewModel>())
                        )));

            #endregion

        }

    }

}