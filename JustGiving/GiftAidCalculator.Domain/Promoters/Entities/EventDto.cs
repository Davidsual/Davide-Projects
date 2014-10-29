using System.Runtime.Serialization;

namespace GiftAidCalculator.Domain.Promoters.Entities
{
    [DataContract]
    public enum EventType
    {
        [EnumMember]
        Running = 0,
        [EnumMember]
        Swimming,
        [EnumMember]
        Other
    }
    [DataContract]
    public class EventDto
    {
        [DataMember]
        public EventType EventType { get; set; }
        [DataMember]
        public decimal EventAdditionalSupplement { get; set; }
    }
}
