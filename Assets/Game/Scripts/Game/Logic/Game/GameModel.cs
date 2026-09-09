using Game.Scripts.Core.EventBus;
using Game.Scripts.Game.UI.Architecture;

namespace Game.Scripts.Game.Logic.Game
{
    public class GameModel : BaseModel
    {
        private int _score;
        private int _currentLevel;
        private int _balance;

        public int Score
        {
            get => _score;
            set { _score = value; eventBus.Publish(new ScoreChangedEvent(_score)); }
        }

        public int CurrentLevel
        {
            get => _currentLevel;
            set { _currentLevel = value; eventBus.Publish(new LevelChangedEvent(_currentLevel)); }
        }

        public int Balance
        {
            get => _balance;
            set { _balance = value; eventBus.Publish(new BalanceChangedEvent(_balance)); }
        }

        /// <summary>
        /// Bulk-load persisted values without notifying subscribers. Use during state setup so the
        /// model does not publish change events for every field on entry.
        /// </summary>
        public void LoadValues(int score, int currentLevel, int balance)
        {
            _score = score;
            _currentLevel = currentLevel;
            _balance = balance;
        }
    }
}