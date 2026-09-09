using Game.Scripts.Core.EventBus;
using Game.Scripts.Game.UI.Architecture;

namespace Game.Scripts.Game.Logic.Main
{
    public class MainModel : BaseModel
    {
        private int _balance;

        public int Balance
        {
            get => _balance;
            set => SetBalance(value, true);
        }

        /// <summary>
        /// Sets the balance without notifying subscribers. Use when loading persisted values so
        /// the model does not spam <see cref="BalanceChangedEvent"/> during state setup.
        /// </summary>
        public void SetBalanceWithoutNotify(int value) => SetBalance(value, false);

        private void SetBalance(int value, bool notify)
        {
            _balance = value;
            if (notify)
                eventBus.Publish(new BalanceChangedEvent(_balance));
        }
    }
}