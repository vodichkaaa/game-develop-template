using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Sounds;
using UnityEngine;

namespace Game.Scripts.Services.StaticData.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string SoundDataPath = "StaticData/SoundData";
        private const string GameConfigurationPath = "StaticData/GameSettings";
        
        public SoundData LoadSoundData() => Resources.Load<SoundData>(SoundDataPath);
        public GameSettings LoadGameConfiguration() => Resources.Load<GameSettings>(GameConfigurationPath);
    }
}