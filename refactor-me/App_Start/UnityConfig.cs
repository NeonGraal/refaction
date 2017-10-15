using Microsoft.Practices.Unity;
using refactor_me.Model.Contract;
using refactor_me.Model.Implementation;
using refactor_me.Repository;
using System.Web.Http;
using Unity.WebApi;

namespace refactor_me
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IProductsRepository, ProductsRepository>();
            container.RegisterType<IProductsService, ProductsService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}