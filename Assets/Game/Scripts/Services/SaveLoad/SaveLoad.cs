using Game.Scripts.Data.Progress;
using Game.Scripts.Extensions;
using UnityEngine;

namespace Game.Scripts.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        /// <summary>
        /// Bump when the persisted save schema changes. Used to detect and reset saves that are
        /// incompatible with the current <see cref="UserProgress"/> shape instead of crashing.
        /// </summary>
        private const int SaveVersion = 1;
        private const string VersionKey = "ProgressVersion";
        private const string ProgressKey = "Progress";

        public UserProgress Progress { get; set; }

        public void Load()
        {
            int savedVersion = PlayerPrefs.GetInt(VersionKey, 0);
            if (savedVersion != SaveVersion)
            {
                ResetProgress();
                return;
            }

            try
            {
                string progressJson = PlayerPrefs.GetString(ProgressKey);
                Progress = progressJson.ToDeserialized<UserProgress>() ?? new UserProgress();
                Progress.Prepare();
                Progress.OnPropertyChanged += SaveProgress;
                Debug.Log($"Load Progress - {Progress.ToJson()}");
            }
            catch (System.Exception ex)
            {
                // Corrupted / incompatible save must not crash the load flow; start fresh.
                Debug.LogError($"Load Progress failed, resetting save: {ex.Message}");
                ResetProgress();
            }
        }

        private void ResetProgress()
        {
            Progress = new UserProgress();
            Progress.Prepare();
            Progress.OnPropertyChanged += SaveProgress;
            PlayerPrefs.SetInt(VersionKey, SaveVersion);
            SaveProgress();
            Debug.Log($"Reset Progress - {Progress.ToJson()}");
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetString(ProgressKey, Progress.ToJson());
        }
    }
}