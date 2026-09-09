using System;

namespace Game.Scripts.Data.Progress
{
    [Serializable]
    public class UserProgress
    {
        private int _currentBalance;
        private int _currentLevel;
        
        public UserProgress()
        {
            Settings = new SettingsData();

            CurrentBalance = 1000;
            CurrentLevel = 1;
        }
        public SettingsData Settings { get; set; }

        public int CurrentBalance
        {
            get => _currentBalance;
            set
            {
                _currentBalance = value;
                OnPropertyChanged?.Invoke();
            }
        }

        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                OnPropertyChanged?.Invoke();
            }
        }
        public event Action OnPropertyChanged;

        public void Prepare()
        {
            Settings.OnPropertyChanged += SendPropertyChanged;
        }

        public void SendPropertyChanged()
        {
            OnPropertyChanged?.Invoke();
        }
    }
}