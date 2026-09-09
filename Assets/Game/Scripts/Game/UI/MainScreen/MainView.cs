using Game.Scripts.Game.Logic.Main;
using Game.Scripts.Game.UI.Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.MainScreen
{
    public class MainView : BaseView<MainModel>
    {
        public UnityEvent OnPlayClick => _playButton.onClick;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _balanceText;
        
        public override void SubscribeView()
        {
            // Override to bind view events that should be (re)subscribed each entry.
        }

        public override void UnsubscribeView()
        {
            // Override to unbind what SubscribeView subscribed.
        }

        public void UpdateBalance(int balance)
        {
            if (_balanceText == null) return;
            _balanceText.text = $"Balance: {balance}";
        }
    }
}