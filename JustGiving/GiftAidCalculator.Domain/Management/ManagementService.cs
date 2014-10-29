using GiftAidCalculator.Domain.Infrastructure.Validator;
using GiftAidCalculator.Domain.Management.Entities;
using GiftAidCalculator.Domain.Repository;
using System;

namespace GiftAidCalculator.Domain.Management
{
    public interface IManagementService
    {
        SettingDto GetSettings();
        SettingDto UpdateSettings(SettingDto settings);
    }

    public class ManagementService : IManagementService
    {
        private readonly ISettingRepository _settingRepository;

        public ManagementService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public SettingDto GetSettings()
        {
            return _settingRepository.Get();
        }

        public SettingDto UpdateSettings(SettingDto settings)
        {
            var validationResult = ValidatorHelper.IsValidEntity<SettingDto>(settings);

            if (!validationResult.IsValid)            
                throw new ArgumentException(validationResult.Errors);
            
            return _settingRepository.UpdateSettings(settings);
        }
    }
}
