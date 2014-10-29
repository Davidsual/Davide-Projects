using GiftAidCalculator.Domain.Management.Entities;

namespace GiftAidCalculator.Domain.Repository
{
    public interface ISettingRepository
    {
        SettingDto Get();
        SettingDto UpdateSettings(SettingDto setting);
    }
}
