using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.DeviceAdaptation.Layout
{
    public class LayoutResolution : DeviceSelection, IDeviceAdjustable
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        [Header("IPhone")] 
        [SerializeField] private Vector2 _iPhoneCellSize = new Vector2(125, 192);
        [SerializeField] private Vector2 _iPhoneSpacing = new Vector2(30, 55);
        [Header("IPad")] 
        [SerializeField] private Vector2 _iPadCellSize = new Vector2(100, 154);
        [SerializeField] private Vector2 _iPadSpacing = new Vector2(40, 17);

        private void Start()
        {
            SetGridLayoutSize(IsIpad());
        }

        public void SetGridLayoutSize(bool isIpad)
        {
            _gridLayoutGroup.cellSize = isIpad ? _iPadCellSize : _iPhoneCellSize;
            _gridLayoutGroup.spacing = isIpad ? _iPadSpacing : _iPhoneSpacing;
        }

        public void AdjustResolution()
        {
        
        }
    }
}
