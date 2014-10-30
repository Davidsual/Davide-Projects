using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel;

namespace CQRS.Web.Infrastructure.DependencyInjection
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;
        private readonly IActionInvoker _actionInvoker;

        public WindsorControllerFactory(IKernel kernel, IActionInvoker actionInvoker)
        {
            _kernel = kernel;
            _actionInvoker = actionInvoker;
        }

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException((int)HttpStatusCode.NotFound, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));

            var controller = _kernel.Resolve(controllerType) as Controller;
            if (controller != null) 
                controller.ActionInvoker = _actionInvoker;

            return controller;
        }
    }
}