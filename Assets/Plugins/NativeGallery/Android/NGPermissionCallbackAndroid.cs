#if UNITY_EDITOR || UNITY_ANDROID
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

namespace NativeGalleryNamespace
{
    public class NGPermissionCallbackAndroid : AndroidJavaProxy
    {
        private readonly object threadLock;

        public NGPermissionCallbackAndroid(object threadLock) : base("com.yasirkula.unity.NativeGalleryPermissionReceiver")
        {
            Result = -1;
            this.threadLock = threadLock;
        }
        public int Result { get; private set; }

        [Preserve]
        public void OnPermissionResult(int result)
        {
            Result = result;

            lock (threadLock)
            {
                Monitor.Pulse(threadLock);
            }
        }
    }

    public class NGPermissionCallbackAsyncAndroid : AndroidJavaProxy
    {
        private readonly NativeGallery.PermissionCallback callback;
        private readonly NGCallbackHelper callbackHelper;

        public NGPermissionCallbackAsyncAndroid(NativeGallery.PermissionCallback callback) : base("com.yasirkula.unity.NativeGalleryPermissionReceiver")
        {
            this.callback = callback;
            callbackHelper = new GameObject("NGCallbackHelper").AddComponent<NGCallbackHelper>();
        }

        [Preserve]
        public void OnPermissionResult(int result)
        {
            callbackHelper.CallOnMainThread(() => callback((NativeGallery.Permission)result));
        }
    }
}
#endif