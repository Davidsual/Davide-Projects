using System;

namespace CQRS.Web.Infrastructure.Bus
{
    public class Response<TResponseData> : Response
    {
        public virtual TResponseData Model { get; set; }
    }

    public class Response
    {
        public virtual Exception Exception { get; set; }

        public virtual bool HasException()
        {
            return Exception != null;
        }
    }
}