using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Sounds;
using Game.Scripts.Services.StaticData.StaticDataProvider;

namespace Game.Scripts.Services.StaticData
{
    public class StaticData : IStaticData
    {
        private readonly IStaticDataProvider _staticDataProvider;

        public StaticData(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
            LoadStaticData();
        }
        public SoundData SoundData { get; private set; }
        public GameSettings GameSettings { get; private set; }

        public void LoadStaticData()
        {
            SoundData = _staticDataProvider.LoadSoundData();
            GameSettings = _staticDataProvider.LoadGameConfiguration();
        }
    }
}