using GiftAidCalculator.Domain.Promoters.Entities;
using GiftAidCalculator.Domain.Repository;

namespace GiftAidCalculator.Domain.Promoters
{
    public interface IPromoterService
    {
        EventDto GetTemplateEvent(EventType eventType);
    }

    public class PromoterService : IPromoterService
    {
        private readonly IEventRepository _eventRepository;

        public PromoterService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public EventDto GetTemplateEvent(EventType eventType)
        {
            decimal supplement = _eventRepository.GetSupplementByEventType(eventType);

            return new EventDto
            {
                EventType = eventType,
                EventAdditionalSupplement = supplement
            };
        }
    }
}
