using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Sounds;

namespace Game.Scripts.Services.StaticData
{
    public interface IStaticData : IGlobalService
    {
        SoundData SoundData { get; }
        GameSettings GameSettings { get; }
    }
}