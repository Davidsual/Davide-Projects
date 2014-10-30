namespace CQRS.Web.Infrastructure.Bus
{
    public interface IBus
    {
        Response<TResponseData> Request<TResponseData>(IQuery<TResponseData> query);
        Response<TResponseData> Send<TMessage, TResponseData>(TMessage message);
        void Send<TMessage>(TMessage message);
    }
}
