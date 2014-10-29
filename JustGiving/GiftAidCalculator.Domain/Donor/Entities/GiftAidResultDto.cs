using System.Runtime.Serialization;

namespace GiftAidCalculator.Domain.Donor.Entities
{
    [DataContract]    
    public class GiftAidResultDto
    {
        [DataMember]
        public decimal DonationAmount { get; set; }
        [DataMember]
        public decimal TaxRate { get; set; }
        [DataMember]
        public decimal TaxedAmount { get; set; }
        [DataMember]
        public decimal CharityAmount { get; set; }
    }
}
