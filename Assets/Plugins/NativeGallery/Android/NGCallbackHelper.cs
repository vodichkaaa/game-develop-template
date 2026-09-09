#if UNITY_EDITOR || UNITY_ANDROID
using System;
using UnityEngine;

namespace NativeGalleryNamespace
{
    public class NGCallbackHelper : MonoBehaviour
    {
        private Action mainThreadAction;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (mainThreadAction != null)
            {
                try
                {
                    var temp = mainThreadAction;
                    mainThreadAction = null;
                    temp();
                }
                finally
                {
                    Destroy(gameObject);
                }
            }
        }

        public void CallOnMainThread(Action function)
        {
            mainThreadAction = function;
        }
    }
}
#endif