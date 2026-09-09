using Cysharp.Threading.Tasks;
using Game.Scripts.Services.Assets;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Services.Factories.BaseFactory
{
    public abstract class BaseFactory : IBaseFactory
    {
        protected readonly IAssets _assets;
        protected readonly IObjectResolver _resolver;

        protected BaseFactory(IAssets assets, IObjectResolver resolver)
        {
            _assets = assets;
            _resolver = resolver;
        }

        public async UniTask<T> InstantiateAsRegistered<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Component
        {
            // Use the string address (typeof(T).Name) to get the component directly
            T component = await _assets.Instantiate<T>(typeof(T).Name, at, rotation, parent);
            
            return FinalizeInstance(component);
        }

        public async UniTask<T> InstantiateAsRegistered<T>(Transform parent = null) where T : Component
        {
            T component = await _assets.Instantiate<T>(typeof(T).Name, parent);
            
            return FinalizeInstance(component);
        }

        public async UniTask<T> Instantiate<T>(string name, Transform parent = null) where T : Component
        {
            T component = await _assets.Instantiate<T>(name, parent);
            
            // Note: If you don't want this registered, call _resolver.Inject only.
            return FinalizeInstance(component);
        }

        public async UniTask<T> Instantiate<T>(Transform parent = null) where T : Component
        {
            return await Instantiate<T>(typeof(T).Name, parent);
        }

        private T FinalizeInstance<T>(T component) where T : Component
        {
            if (component == null)
            {
                Debug.LogError($"Factory failed to instantiate component of type {typeof(T).Name}");
                return null;
            }

            //_registry.Register<T>(component);
            _resolver.Inject(component); 
            return component;
        }
    }
}