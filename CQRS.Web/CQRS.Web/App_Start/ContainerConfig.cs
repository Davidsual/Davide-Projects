using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CQRS.Web.Infrastructure.DependencyInjection;

namespace CQRS.Web
{
    public class ContainerConfig
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer BootstrapContainer()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            var actionInvoker = new WindsorActionInvoker(_container);
            var controllerFactory = new WindsorControllerFactory(_container.Kernel, actionInvoker);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            return _container;
        }

        public static void DisposeContainer()
        {
            _container.Dispose();
        }
    }
}