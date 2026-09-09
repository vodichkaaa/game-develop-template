using UnityEngine;

namespace Game.Scripts.Game.DeviceAdaptation
{
    public class DynamicRatio : BaseResolutionManager
    {
        protected override void Start()
        {
            base.Start();
            AdjustResolution();
        }

        public override void AdjustResolution()
        {
            _isPortraitOrientation = Screen.height > Screen.width;
            ConfigureAspectRatioFitter(IsIpad());
        }
    }
}
