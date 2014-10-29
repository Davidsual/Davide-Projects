using System.Runtime.Serialization;

namespace GiftAidCalculator.Domain.Donor.Entities
{
    [DataContract]
    public class GiftAidDto
    {
        [DataMember]
        public decimal DonationAmount { get; set; }
    }
}
