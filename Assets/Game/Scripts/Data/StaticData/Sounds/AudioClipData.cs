using System;
using Game.Scripts.Data.Enums;
using UnityEngine;

namespace Game.Scripts.Data.StaticData.Sounds
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip Clip;
        public SoundId Id;
    }
}