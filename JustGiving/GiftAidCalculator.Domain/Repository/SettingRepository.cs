using GiftAidCalculator.Domain.Management.Entities;

namespace GiftAidCalculator.Domain.Repository
{
    public class SettingRepository : ISettingRepository
    {
        //Just mocking a database same instance of repository for same request
        private static SettingDto MockSetting { get; set; }

        public Management.Entities.SettingDto Get()
        {
            return MockSetting ?? (MockSetting = new SettingDto());
            //throw new NotImplementedException("Connect to some repository for getting settings");
        }

        public Management.Entities.SettingDto UpdateSettings(Management.Entities.SettingDto setting)
        {
            return MockSetting = setting;
        }
    }
}
