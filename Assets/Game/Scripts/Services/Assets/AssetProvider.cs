using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Scripts.Services.Assets
{
    public class AssetProvider : IAssets
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>(10);
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>(10);

        public AssetProvider() => Addressables.InitializeAsync().ToUniTask();

        public async UniTask<T> Instantiate<T>(string address, Transform parent = null) where T : Object
        {
            GameObject go = await Addressables.InstantiateAsync(address, parent).ToUniTask();
            AttachReleaseOnDestroy(go);
            return GetResult<T>(go, address);
        }

        public async UniTask<T> Instantiate<T>(string address, Vector3 at, Quaternion rotation, Transform parent = null) where T : Object
        {
            GameObject go = await Addressables.InstantiateAsync(address, at, rotation, parent).ToUniTask();
            AttachReleaseOnDestroy(go);
            return GetResult<T>(go, address);
        }

        public async UniTask<T> LoadPersistent<T>(string address) where T : class => await Addressables.LoadAssetAsync<T>(address);

        public async UniTask<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return ExtractResult<T>(completedHandle);

            // If T is a Component, we must load the Asset as a GameObject to avoid InvalidKeyException
            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
                GameObject prefab = await RunWithCacheOnComplete(address, handle);
                return prefab.GetComponent(typeof(T)) as T;
            }

            AsyncOperationHandle<T> standardHandle = Addressables.LoadAssetAsync<T>(address);
            return await RunWithCacheOnComplete(address, standardHandle);
        }

        public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
        {
            string key = assetReference.AssetGUID;
            if (_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
                return ExtractResult<T>(completedHandle);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            return await RunWithCacheOnComplete(key, handle);
        }

        public void CleanUp()
        {
            // Release all cached loads. Instantiated objects self-release via ReleaseOnDestroy when
            // their GameObject dies, so they are intentionally not released here (double-release risk).
            foreach (var handles in _handles.Values)
                foreach (var handle in handles)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
        }

        private void AttachReleaseOnDestroy(GameObject instance)
        {
            if (instance == null) return;

            // Auto-release the Addressable instance once the GameObject dies (view teardown).
            if (instance.GetComponent<ReleaseOnDestroy>() == null)
                instance.AddComponent<ReleaseOnDestroy>();
        }

        private T GetResult<T>(GameObject go, string address) where T : Object
        {
            if (go == null)
            {
                Debug.LogError($"AssetProvider: Failed to instantiate object at address '{address}'");
                return null;
            }

            if (typeof(Component).IsAssignableFrom(typeof(T)))
                return go.GetComponent(typeof(T)) as T;

            return go as T;
        }

        private T ExtractResult<T>(AsyncOperationHandle handle) where T : class
        {
            if (handle.Result is GameObject go && typeof(Component).IsAssignableFrom(typeof(T)))
                return go.GetComponent(typeof(T)) as T;
            return handle.Result as T;
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(string cacheKey, AsyncOperationHandle<T> handle) where T : class
        {
            handle.Completed += completeHandle => _completedCache[cacheKey] = completeHandle;
            AddHandle(cacheKey, handle);
            return await handle.ToUniTask();
        }

        private void AddHandle(string key, AsyncOperationHandle handle)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handles[key] = handles;
            }
            handles.Add(handle);
        }
    }
}