using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Dashboards_BusinessLayer;
using Dashboards_BusinessLayer.Interfaces;
using Dashboards_BusinessLayer.Handlers;
using Dashboards_BusinessLayer.Handlers.Interfaces;

namespace Dashboards_Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IReadMetricDataHandler, ReadMetricDataHandler>();

            container.RegisterType<IICMDataHandler, ICMDataHandler>();
            container.RegisterType<ITaskHandler, TaskHandler>();
            container.RegisterType<ISfbHandler, SfbHandler>();
            container.RegisterType<IMsodsHandler, MSOdsHandler>();
            container.RegisterType<IBvdHandler, BvdHandler>();
            container.RegisterType<IIc3EdgeHandler, IC3EdgeHandler>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}