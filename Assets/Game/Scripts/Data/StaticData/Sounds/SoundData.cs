using UnityEngine;

namespace Game.Scripts.Data.StaticData.Sounds
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "StaticData/SoundData")]
    public class SoundData : ScriptableObject
    {
        public AudioClipData[] AudioEffectClips;
        public AudioClip[] BackgroundMusic;
    }
}