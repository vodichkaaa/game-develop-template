using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.DeviceAdaptation
{
    public class ResolutionCanvasScaler: DeviceSelection
    {
        [SerializeField] private CanvasScaler _canvasScaler;

        [SerializeField] private float _iphoneScreenMatch = 0.5f;
        [SerializeField] private float _ipadScreenMatch = 0.25f;

        private void Awake()
        {
            _canvasScaler.matchWidthOrHeight = IsIpad() ? _ipadScreenMatch : _iphoneScreenMatch;
        }
    }
}