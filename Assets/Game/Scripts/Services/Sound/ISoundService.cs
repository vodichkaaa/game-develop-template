using Game.Scripts.Data.Enums;
using Game.Scripts.Data.StaticData.Sounds;
using Game.Scripts.Services.SaveLoad;

namespace Game.Scripts.Services.Sound
{
    public interface ISoundService
    {
        bool IsSoundMuted { get; set; }
        void Construct(ISaveLoad saveLoad, SoundData soundData);
        void PlayBackgroundMusic();
        void PlayEffectSound(SoundId soundId);
        public void MuteSound();
    }
}