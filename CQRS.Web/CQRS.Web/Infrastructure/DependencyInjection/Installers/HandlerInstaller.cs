using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRS.Web.Infrastructure.Bus;


namespace CQRS.Web.Infrastructure.DependencyInjection.Installers
{   
    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBus>().Instance(new CQRS.Web.Infrastructure.Bus.Bus(container)));

            container.Register(
                Types.FromThisAssembly()
                     .BasedOn(typeof (ICommandHandler<>))
                     .WithService
                     .Base());

            container.Register(
                Types.FromThisAssembly()
                     .BasedOn(typeof(ICommandHandler<,>))
                     .WithService
                     .Base());

            container.Register(
                Types.FromThisAssembly()
                     .BasedOn(typeof (IQueryHandler<,>))
                     .WithService
                     .Base());
        }
    }
}