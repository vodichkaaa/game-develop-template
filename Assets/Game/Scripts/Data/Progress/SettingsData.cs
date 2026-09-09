using System;

namespace Game.Scripts.Data.Progress
{
    [Serializable]
    public class SettingsData
    {
        private bool _isSoundMuted;

        public SettingsData()
        {
            IsSoundMuted = false;
        }

        public bool IsSoundMuted
        {
            get => _isSoundMuted;
            set
            {
                _isSoundMuted = value;
                OnPropertyChanged?.Invoke();
            }
        }
        public event Action OnPropertyChanged;
    }
}