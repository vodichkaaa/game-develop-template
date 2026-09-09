using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Sounds;

namespace Game.Scripts.Services.StaticData.StaticDataProvider
{
    public interface IStaticDataProvider : IGlobalService
    {
        SoundData LoadSoundData();
        GameSettings LoadGameConfiguration();
    }
}