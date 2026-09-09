using Game.Scripts.Animations;
using Game.Scripts.Game.Logic.Game;
using Game.Scripts.Game.UI.Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.GameScreen
{
    public class GameView : BaseView<GameModel>
    {
        public UnityEvent OnBackClick => _backButton.onClick;
        
        [Header("Buttons")] 
        [SerializeField] private Button _backButton;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _balanceText;

        [Header("Popup")]
        [SerializeField] private PopupAnimation _resultPopup;

        public override void SubscribeView()
        {
            // Override to bind view events that should be (re)subscribed each entry.
        }

        public override void UnsubscribeView()
        {
            // Override to unbind what SubscribeView subscribed.
        }
        
        public void UpdateScore(int score)
        {
            if (_scoreText == null) return;
            _scoreText.text = $"Score: {score}";
        }
        
        public void UpdateLevel(int level)
        {
            if (_levelText == null) return;
            _levelText.text = $"Level: {level}";
        }
        
        public void UpdateBalance(int balance)
        {
            if (_balanceText == null) return;
            _balanceText.text = $"Balance: {balance}";
        }
    }
}