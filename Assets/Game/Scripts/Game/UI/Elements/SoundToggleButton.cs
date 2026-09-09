using Game.Scripts.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.Elements
{
    public class SoundToggleButton : MonoBehaviour
    {
        [SerializeField] private Sprite _soundOnSprite;
        [SerializeField] private Sprite _soundOffSprite;
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        
        private ISoundService _soundService;

        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
            _button.onClick.AddListener(ToggleSound);
        }
    
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ToggleSound);
        }

        public void SetImage()
        {
            UpdateImage(_soundService.IsSoundMuted);
        }
    
        public void ToggleSound()
        {
            _soundService.MuteSound();
            SetImage();
        }

        public void UpdateImage(bool isMuted)
        {
            _image.sprite = isMuted ? _soundOffSprite : _soundOnSprite;
        }
    }
}