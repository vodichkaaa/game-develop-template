#if UNITY_EDITOR || UNITY_ANDROID
using UnityEngine;
using UnityEngine.Scripting;

namespace NativeGalleryNamespace
{
    public class NGMediaReceiveCallbackAndroid : AndroidJavaProxy
    {
        private readonly NativeGallery.MediaPickCallback callback;

        private readonly NGCallbackHelper callbackHelper;
        private readonly NativeGallery.MediaPickMultipleCallback callbackMultiple;

        public NGMediaReceiveCallbackAndroid(NativeGallery.MediaPickCallback callback, NativeGallery.MediaPickMultipleCallback callbackMultiple) : base("com.yasirkula.unity.NativeGalleryMediaReceiver")
        {
            this.callback = callback;
            this.callbackMultiple = callbackMultiple;
            callbackHelper = new GameObject("NGCallbackHelper").AddComponent<NGCallbackHelper>();
        }

        [Preserve]
        public void OnMediaReceived(string path)
        {
            callbackHelper.CallOnMainThread(() => callback(!string.IsNullOrEmpty(path) ? path : null));
        }

        [Preserve]
        public void OnMultipleMediaReceived(string paths)
        {
            string[] result = null;
            if (!string.IsNullOrEmpty(paths))
            {
                var pathsSplit = paths.Split('>');

                var validPathCount = 0;
                for (var i = 0; i < pathsSplit.Length; i++)
                {
                    if (!string.IsNullOrEmpty(pathsSplit[i]))
                        validPathCount++;
                }

                if (validPathCount == 0)
                    pathsSplit = new string[0];
                else if (validPathCount != pathsSplit.Length)
                {
                    var validPaths = new string[validPathCount];
                    for (int i = 0, j = 0; i < pathsSplit.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(pathsSplit[i]))
                            validPaths[j++] = pathsSplit[i];
                    }

                    pathsSplit = validPaths;
                }

                result = pathsSplit;
            }

            callbackHelper.CallOnMainThread(() => callbackMultiple(result != null && result.Length > 0 ? result : null));
        }
    }
}
#endif