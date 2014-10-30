using Castle.Windsor;
using System.Linq;

namespace CQRS.Web.Infrastructure.Bus
{
    public class Bus : IBus
    {
        private readonly IWindsorContainer _kernel;

        public Bus(IWindsorContainer kernel)
        {
            _kernel = kernel;
        }

        public virtual Response<TResponseData> Request<TResponseData>(IQuery<TResponseData> query)
        {
            var response = new Response<TResponseData>();
            var handler = GetHandler(query);
            response.Model = ProcessQueryWithHandler(query, handler);
            
            return response;
        }

        private object GetHandler<TResponseData>(IQuery<TResponseData> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponseData));
            var handler = _kernel.Resolve(handlerType);
            return handler;
        }

        static TResponseData ProcessQueryWithHandler<TResponseData>(IQuery<TResponseData> query, object handler)
        {
            var method = handler
                .GetType()
                .GetMethods()
                .Single(x => x.Name == "Handle" 
                          && x.GetParameters().Any(p => p.ParameterType == query.GetType()));

            return (TResponseData)method.Invoke(handler, new object[] { query });
        }

        public Response<TResponseData> Send<TMessage, TResponseData>(TMessage message)
        {
            var handler = (ICommandHandler<TMessage, TResponseData>)_kernel.Resolve(typeof(ICommandHandler<TMessage, TResponseData>));

            var response = new Response<TResponseData>();
            response.Model = handler.Handle(message);

            return response;
        }

        public virtual void Send<TMessage>(TMessage message)
        {   
            var handler = (ICommandHandler<TMessage>)_kernel.Resolve(typeof(ICommandHandler<TMessage>));
            handler.Handle(message);   
        }
    }
}