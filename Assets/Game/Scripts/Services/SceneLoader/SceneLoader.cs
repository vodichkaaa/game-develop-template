using System;
using Game.Scripts.Services.Assets;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IAssets _assets;

        public SceneLoader(IAssets assets)
        {
            _assets = assets;
        }

        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            var loadSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
            loadSceneAsyncOperation!.completed += operation =>
            {
                _assets.CleanUp();
                onLoaded?.Invoke();
            };
        }
    }
}