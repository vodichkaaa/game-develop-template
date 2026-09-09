using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Game.DeviceAdaptation.Layout
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class DeviceLayoutAdapter : DeviceSelection
    {
        [Header("iPhone Layout Data")]
        [SerializeField] private Vector2 _iphoneMin;
        [SerializeField] private Vector2 _iphoneMax;

        [Header("iPad Layout Data")]
        [SerializeField] private Vector2 _ipadMin;
        [SerializeField] private Vector2 _ipadMax;

        [SerializeField] private RectTransform _rect;

        private void OnEnable()
        {
            UILayoutManager.OnLayoutChanged += ApplyLayout;
        }
        
        private void OnDisable()
        {
            UILayoutManager.OnLayoutChanged -= ApplyLayout;
        }

        private void ApplyLayout()
        {
            bool isiPad = IsIpad();
            Vector2 targetMin = isiPad ? _ipadMin : _iphoneMin;
            Vector2 targetMax = isiPad ? _ipadMax : _iphoneMax;
            
            if (targetMin != Vector2.zero || targetMax != Vector2.zero)
            {
                _rect.anchorMin = targetMin;
                _rect.anchorMax = targetMax;
                _rect.offsetMin = Vector2.zero;
                _rect.offsetMax = Vector2.zero;
            }
        }

    #region Editor Tools
#if UNITY_EDITOR
        [CustomEditor(typeof(DeviceLayoutAdapter))]
        public class DeviceLayoutAdapterEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                DeviceLayoutAdapter script = (DeviceLayoutAdapter)target;

                GUILayout.Space(10);
                if (GUILayout.Button("Capture Current as iPhone"))
                {
                    script._iphoneMin = script._rect.anchorMin;
                    script._iphoneMax = script._rect.anchorMax;
                    EditorUtility.SetDirty(script);
                }

                if (GUILayout.Button("Capture Current as iPad"))
                {
                    script._ipadMin = script._rect.anchorMin;
                    script._ipadMax = script._rect.anchorMax;
                    EditorUtility.SetDirty(script);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (transform.parent == null) return;
            
            DrawLayoutPreview(_iphoneMin, _iphoneMax, Color.cyan, "iPhone");
            DrawLayoutPreview(_ipadMin, _ipadMax, Color.yellow, "iPad");
        }

        private void DrawLayoutPreview(Vector2 min, Vector2 max, Color color, string label)
        {
            RectTransform parent = transform.parent.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            parent.GetWorldCorners(corners);

            float w = corners[2].x - corners[0].x;
            float h = corners[2].y - corners[0].y;

            Vector3 posMin = corners[0] + new Vector3(min.x * w, min.y * h, 0);
            Vector3 posMax = corners[0] + new Vector3(max.x * w, max.y * h, 0);

            Gizmos.color = color;
            Gizmos.DrawWireCube((posMin + posMax) / 2, posMax - posMin);
        }
#endif
    #endregion
    }
}