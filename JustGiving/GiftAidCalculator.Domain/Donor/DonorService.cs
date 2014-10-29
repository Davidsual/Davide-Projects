using GiftAidCalculator.Domain.Donor.Entities;
using GiftAidCalculator.Domain.Infrastructure.Validator;
using GiftAidCalculator.Domain.Management.Entities;
using GiftAidCalculator.Domain.Repository;
using System;

namespace GiftAidCalculator.Domain.Donor
{
    public interface IDonorService
    {
        GiftAidResultDto GetGiftAidAmount(GiftAidDto giftAid);
    }
    public class DonorService : IDonorService
    {
        private readonly ISettingRepository _settingRepository;

        public DonorService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public GiftAidResultDto GetGiftAidAmount(GiftAidDto giftAid)
        {
            var validationResult = ValidatorHelper.IsValidEntity<GiftAidDto>(giftAid);

            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors);

            var settings = _settingRepository.Get();

            var giftAidResult = new GiftAidResultDto();

            giftAidResult.DonationAmount = giftAid.DonationAmount;
            giftAidResult.TaxRate = settings.TaxRatePercentage;
            giftAidResult.TaxedAmount = GetTaxedAmount(giftAid.DonationAmount, settings);
            giftAidResult.CharityAmount = GetCharityAmount(giftAid.DonationAmount, giftAidResult.TaxedAmount, settings);

            return giftAidResult;

        }

        private static decimal GetTaxedAmount(decimal donationAmount, SettingDto settingDto)
        {
            return Math.Round((donationAmount * (settingDto.TaxRatePercentage / (100 - settingDto.TaxRatePercentage))),settingDto.NumberDecimalRounded);
        }

        private static decimal GetCharityAmount(decimal donationAmount, decimal taxedAmount, SettingDto settingDto)
        {
            return Math.Round((donationAmount - taxedAmount), settingDto.NumberDecimalRounded);
        }
    }
}
