using GiftAidCalculator.Domain.Promoters.Entities;
using System;

namespace GiftAidCalculator.Domain.Repository
{
    public class EventRepository :IEventRepository
    {
        public decimal GetSupplementByEventType(Promoters.Entities.EventType eventType)
        {
            //throw new NotImplementedException("Connect to some repository for getting supplement by event type");
            switch (eventType)
            {
                case EventType.Running:
                    return 5m;
                case EventType.Swimming:
                    return 3m;
                case EventType.Other:
                    return 0m;
                default:
                    throw new ArgumentOutOfRangeException("eventType");
            }
        }
    }
}
