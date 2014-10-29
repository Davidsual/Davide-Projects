using GiftAidCalculator.Domain.Promoters.Entities;

namespace GiftAidCalculator.Domain.Repository
{
    public interface IEventRepository
    {
        decimal GetSupplementByEventType(EventType eventType);
    }
}
