using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Services.Assets
{
    /// <summary>
    /// Attached (via <see cref="AssetProvider.Instantiate{T}"/>) to objects created through
    /// Addressables.InstantiateAsync so the instance operation is released when the GameObject
    /// is destroyed. Without it every spawned view/entity would leak its Addressable over the
    /// session.
    /// </summary>
    public sealed class ReleaseOnDestroy : MonoBehaviour
    {
        public void OnDestroy()
        {
            if (gameObject != null)
                Addressables.ReleaseInstance(gameObject);
        }
    }
}