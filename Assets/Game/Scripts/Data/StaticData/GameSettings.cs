using UnityEngine;

namespace Game.Scripts.Data.StaticData
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "StaticData/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Performance")]
        [Tooltip("Applied in LoadDataState.PrepareGame via Application.targetFrameRate.")]
        [SerializeField] private int _targetFrameRate = 60;

        [Tooltip("0 = not synced (vSyncCount), 1 = sync to refresh rate.")]
        [SerializeField, Range(0, 4)] private int _vSyncCount = 0;

        public int TargetFrameRate => _targetFrameRate;
        public int VSyncCount => _vSyncCount;
    }
}