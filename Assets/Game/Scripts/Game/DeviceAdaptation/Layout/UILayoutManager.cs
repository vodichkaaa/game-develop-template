using System;
using UnityEngine;

namespace Game.Scripts.Game.DeviceAdaptation.Layout
{
    [ExecuteAlways]
    public class UILayoutManager : MonoBehaviour
    {
        public static event Action OnLayoutChanged;
    
        private Vector2 _lastResolution;

        private void Start()
        {
            _lastResolution = new Vector2(Screen.width, Screen.height);
            OnLayoutChanged?.Invoke();
        }

        private void OnEnable()
        {
            _lastResolution = new Vector2(Screen.width, Screen.height);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
            {
                CheckResolutionChange();
            }
        }
#endif

        private void CheckResolutionChange()
        {
            if (!Mathf.Approximately(Screen.width, _lastResolution.x) || 
                !Mathf.Approximately(Screen.height, _lastResolution.y))
            {
                _lastResolution = new Vector2(Screen.width, Screen.height);
                OnLayoutChanged?.Invoke();
            }
        }
    }
}