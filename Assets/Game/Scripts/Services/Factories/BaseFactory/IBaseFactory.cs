using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Services.Factories.BaseFactory
{
    public interface IBaseFactory
    {
        UniTask<T> InstantiateAsRegistered<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Component;
        UniTask<T> InstantiateAsRegistered<T>(Transform parent) where T : Component;
        UniTask<T> Instantiate<T>(Transform parent = null) where T : Component;
    }
}