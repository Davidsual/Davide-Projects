namespace CQRS.Web.Infrastructure.Bus
{
    public interface ICommandHandler<in TMessage>
    {
        void Handle(TMessage message);
    }

    public interface ICommandHandler<in TMessage, out TResponse>
    {
        TResponse Handle(TMessage message);
    }
}