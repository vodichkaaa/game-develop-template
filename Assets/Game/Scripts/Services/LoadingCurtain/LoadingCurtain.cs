using System.Collections;
using UnityEngine;

namespace Game.Scripts.Services.LoadingCurtain
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private CanvasGroup _curtain;

        private Coroutine _fadeRoutine;

        public void Show()
        {
            if (gameObject.activeSelf)
                return;

            StopFade();
            gameObject.SetActive(true);
            _curtain.alpha = 1f;
        }

        public void Hide()
        {
            if (!gameObject.activeSelf)
                return;

            StopFade();
            _fadeRoutine = StartCoroutine(DoFadeOut());
        }

        private IEnumerator DoFadeOut()
        {
            while (_curtain != null && _curtain.alpha > 0f)
            {
                _curtain.alpha -= Time.deltaTime * 3f;
                yield return null;
            }

            if (this != null && gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        private void StopFade()
        {
            if (_fadeRoutine != null)
            {
                StopCoroutine(_fadeRoutine);
                _fadeRoutine = null;
            }
        }
    }
}