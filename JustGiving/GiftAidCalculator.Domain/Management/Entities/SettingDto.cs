using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GiftAidCalculator.Domain.Management.Entities
{
    [DataContract]
    public class SettingDto
    {
        public SettingDto()
        {
            TaxRatePercentage = 20; 
            NumberDecimalRounded = 2;
        }

        [DataMember]
        [Required]
        public decimal TaxRatePercentage { get; set; }
        [DataMember]
        [Required]
        public int NumberDecimalRounded { get; set; }

    }
}
