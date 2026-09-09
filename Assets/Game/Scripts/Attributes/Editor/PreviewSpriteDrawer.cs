#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(PreviewSpriteAttribute))]
    public class PreviewSpriteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference || 
                property.objectReferenceValue == null || 
                !(property.objectReferenceValue is Sprite))
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            PreviewSpriteAttribute previewAttribute = (PreviewSpriteAttribute)attribute;
            float maxHeight = previewAttribute.Height;
            
            Sprite sprite = property.objectReferenceValue as Sprite;
            
            float aspectRatio = sprite.rect.width / sprite.rect.height;
            float previewWidth = maxHeight * aspectRatio;
            float previewHeight = maxHeight;
            
            if (previewWidth > maxHeight * 1.5f) 
            {
                previewWidth = maxHeight * 1.5f;
                previewHeight = previewWidth / aspectRatio;
            }
            
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);
            
            Rect fieldRect = new Rect(
                position.x + EditorGUIUtility.labelWidth, 
                position.y, 
                position.width - EditorGUIUtility.labelWidth - previewWidth - 5, 
                EditorGUIUtility.singleLineHeight
            );
            EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
            
            if (sprite != null)
            {
                Rect previewRect = new Rect(
                    position.x + position.width - previewWidth, 
                    position.y + (maxHeight - previewHeight) / 2, 
                    previewWidth, 
                    previewHeight
                );
                
                EditorGUI.DrawRect(previewRect, new Color(0.2f, 0.2f, 0.2f, 1f));
                
                Texture2D texture = sprite.texture;
                Rect textureRect = sprite.textureRect;
                Rect uv = new Rect(
                    textureRect.x / texture.width,
                    textureRect.y / texture.height,
                    textureRect.width / texture.width,
                    textureRect.height / texture.height
                );
                
                GUI.DrawTextureWithTexCoords(previewRect, texture, uv);
                
                EditorGUI.DrawRect(new Rect(previewRect.x, previewRect.y, previewRect.width, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(previewRect.x, previewRect.yMax - 1, previewRect.width, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(previewRect.x, previewRect.y, 1, previewRect.height), Color.gray);
                EditorGUI.DrawRect(new Rect(previewRect.xMax - 1, previewRect.y, 1, previewRect.height), Color.gray);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference || 
                property.objectReferenceValue == null || 
                !(property.objectReferenceValue is Sprite))
            {
                return EditorGUIUtility.singleLineHeight;
            }
            
            PreviewSpriteAttribute previewAttribute = (PreviewSpriteAttribute)attribute;
            return Mathf.Max(previewAttribute.Height + 4, EditorGUIUtility.singleLineHeight); 
        }
    }
}
#endif