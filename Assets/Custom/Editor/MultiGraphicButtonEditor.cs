using System.Collections.Generic;
using Game.Scripts.Game.UI.Elements;
using Game.Scripts.Game.UI.Elements.MultiGraphicButton;
using Game.Scripts.Game.UI.Elements.MultiGraphicButton.GraphicTargets;
using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Custom.Editor
{
    [CustomEditor(typeof(MultiGraphicButton), true)]
    [CanEditMultipleObjects]
    public class MultiGraphicButtonEditor : ButtonEditor
    {
        private SerializedProperty graphicTargetsProp;
        private SerializedProperty transitionDurationProp;
    
        private bool showGraphicTargets = true;
        private Dictionary<int, bool> targetFoldouts = new Dictionary<int, bool>();
    
        private static GUIStyle headerStyle;
        private static GUIStyle boxStyle;
        private static Color headerColor = new Color(0.3f, 0.5f, 0.7f, 0.3f);

        protected override void OnEnable()
        {
            base.OnEnable();
        
            graphicTargetsProp = serializedObject.FindProperty("graphicTargets");
            transitionDurationProp = serializedObject.FindProperty("transitionDuration");
        }

        public override void OnInspectorGUI()
        {
            InitializeStyles();
            base.OnInspectorGUI();
        
            EditorGUILayout.Space(10);
            DrawSeparator();
        
            serializedObject.Update();
            
            DrawMultiGraphicButtonSettings();
        
            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.fontSize = 12;
                headerStyle.normal.textColor = Color.white;
            }

            if (boxStyle == null)
            {
                boxStyle = new GUIStyle(EditorStyles.helpBox);
                boxStyle.padding = new RectOffset(10, 10, 10, 10);
            }
        }

        private void DrawSeparator()
        {
            EditorGUILayout.Space(5);
            Rect rect = EditorGUILayout.GetControlRect(false, 2);
            rect.height = 2;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
            EditorGUILayout.Space(5);
        }

        private void DrawMultiGraphicButtonSettings()
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, 25);
            EditorGUI.DrawRect(headerRect, headerColor);
            EditorGUI.LabelField(headerRect, "  MULTI-GRAPHIC BUTTON SETTINGS", headerStyle);
        
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUILayout.PropertyField(transitionDurationProp, new GUIContent("Transition Duration", "Duration of state transitions"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            
            DrawGraphicTargetsSection();
        }

        private void DrawGraphicTargetsSection()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            EditorGUILayout.BeginHorizontal();
            showGraphicTargets = EditorGUILayout.Foldout(showGraphicTargets, 
                $"Graphic Targets ({graphicTargetsProp.arraySize})", true, EditorStyles.foldoutHeader);

            GUILayout.FlexibleSpace();
                
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Image"), false, AddTargetReference<ImageGraphicTarget>);
                menu.AddItem(new GUIContent("TextMeshProUGUI"), false, AddTargetReference<TextGraphicTarget>);
                
                menu.ShowAsContext();
            }
        
            GUI.enabled = graphicTargetsProp.arraySize > 0;
            if (GUILayout.Button("Clear All", GUILayout.Width(70)))
            {
                if (EditorUtility.DisplayDialog("Clear All Targets", 
                    "Are you sure you want to remove all graphic targets?", "Yes", "No"))
                {
                    graphicTargetsProp.ClearArray();
                    targetFoldouts.Clear();
                    serializedObject.ApplyModifiedProperties();
                }
            }
            GUI.enabled = true;
        
            EditorGUILayout.EndHorizontal();

            if (showGraphicTargets)
            {
                EditorGUILayout.Space(5);
            
                for (int i = 0; i < graphicTargetsProp.arraySize; i++)
                {
                    DrawGraphicTarget(i);
                    EditorGUILayout.Space(5);
                }

                if (graphicTargetsProp.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("No graphic targets added. Click '+' to add a new target.", MessageType.Info);
                }
            }
        
            EditorGUILayout.EndVertical();
        }
        
        private void AddTargetReference<T>() where T : BaseGraphicTarget, new()
        {
            serializedObject.Update();
            int index = graphicTargetsProp.arraySize;
            
            graphicTargetsProp.InsertArrayElementAtIndex(index);
            graphicTargetsProp.GetArrayElementAtIndex(index).managedReferenceValue = new T();
    
            serializedObject.ApplyModifiedProperties();
            InitializeNewTarget(index); 
        }

        private void InitializeNewTarget(int index)
        {
            SerializedProperty targetProp = graphicTargetsProp.GetArrayElementAtIndex(index);
            
            SetProp(targetProp, "targetName", "New Target");
            SetProp(targetProp, "useColorTransition", true);
            SetProp(targetProp, "normalColor", Color.white);
            SetProp(targetProp, "highlightedColor", Color.white);
            SetProp(targetProp, "pressedColor", Color.white);
            SetProp(targetProp, "selectedColor", Color.white);
            SetProp(targetProp, "disabledColor", new Color(1f, 1f, 1f, 0.5f));
            
            SetProp(targetProp, "useScaleTransition", false);
            SetProp(targetProp, "normalScale", 1f);
            SetProp(targetProp, "highlightedScale", 1.1f);
            SetProp(targetProp, "pressedScale", 0.95f);
            SetProp(targetProp, "selectedScale", 1f);
            SetProp(targetProp, "disabledScale", 1f);
            
            SetProp(targetProp, "useFadeTransition", false);
            SetProp(targetProp, "normalAlpha", 1f);
            SetProp(targetProp, "highlightedAlpha", 1f);
            SetProp(targetProp, "pressedAlpha", 1f);
            SetProp(targetProp, "selectedAlpha", 1f);
            SetProp(targetProp, "disabledAlpha", 0.5f);
            
            SetProp(targetProp, "useRotationTransition", false);
            SetProp(targetProp, "normalRotation", Vector3.zero);
            SetProp(targetProp, "highlightedRotation", Vector3.zero);
            SetProp(targetProp, "pressedRotation", Vector3.zero);
            SetProp(targetProp, "selectedRotation", Vector3.zero);
            SetProp(targetProp, "disabledRotation", Vector3.zero);
            
            SetProp(targetProp, "usePositionOffset", false);
            SetProp(targetProp, "normalOffset", Vector2.zero);
            SetProp(targetProp, "highlightedOffset", Vector2.zero);
            SetProp(targetProp, "pressedOffset", Vector2.zero);
            SetProp(targetProp, "selectedOffset", Vector2.zero);
            SetProp(targetProp, "disabledOffset", Vector2.zero);
            
            SetProp(targetProp, "usePunchScale", false);
            SetProp(targetProp, "punchScaleStrength", new Vector3(0.2f, 0.2f, 0));
            SetProp(targetProp, "punchDuration", 0.3f);
            SetProp(targetProp, "punchVibrato", 10);
            SetProp(targetProp, "punchElasticity", 1f);
            SetProp(targetProp, "usePunchRotation", false);
            SetProp(targetProp, "punchRotationStrength", new Vector3(0, 0, 15f));
            SetProp(targetProp, "usePunchPosition", false);
            SetProp(targetProp, "punchPositionStrength", new Vector2(5f, 5f));
            
            SetProp(targetProp, "useShakeOnHover", false);
            SetProp(targetProp, "shakeDuration", 0.3f);
            SetProp(targetProp, "shakeStrength", new Vector3(2f, 2f, 0));
            SetProp(targetProp, "shakeVibrato", 10);
            
            SetProp(targetProp, "useSpriteSwap", false);
            SetProp(targetProp, "useMaterialSwap", false);
            
            SetProp(targetProp, "useTextColorChange", false);
            SetProp(targetProp, "normalTextColor", Color.white);
            SetProp(targetProp, "highlightedTextColor", Color.white);
            SetProp(targetProp, "pressedTextColor", Color.white);
            SetProp(targetProp, "selectedTextColor", Color.white);
            SetProp(targetProp, "disabledTextColor", new Color(1f, 1f, 1f, 0.5f));

            serializedObject.ApplyModifiedProperties();
        }
        
        private void SetProp(SerializedProperty parent, string path, object value)
        {
            SerializedProperty prop = parent.FindPropertyRelative(path);
            if (prop == null) return; 

            switch (value)
            {
                case bool b: prop.boolValue = b; break;
                case Color c: prop.colorValue = c; break;
                case float f: prop.floatValue = f; break;
                case int i: prop.intValue = i; break;
                case Vector2 v2: prop.vector2Value = v2; break;
                case Vector3 v3: prop.vector3Value = v3; break;
                case string s: prop.stringValue = s; break;
                case UnityEngine.Object obj: prop.objectReferenceValue = obj; break;
            }
        }

        private void DrawGraphicTarget(int index)
        {
            SerializedProperty targetProp = graphicTargetsProp.GetArrayElementAtIndex(index);
            
            string typeName = targetProp.managedReferenceFullTypename;
            bool isImageTarget = typeName.Contains("ImageGraphicTarget");
            bool isTextTarget = typeName.Contains("TextGraphicTarget");
            
            SerializedProperty graphicProp = targetProp.FindPropertyRelative("targetGraphic");
            SerializedProperty targetNameProp = targetProp.FindPropertyRelative("targetName");
            
            if (!targetFoldouts.ContainsKey(index)) targetFoldouts[index] = false;
            
            Color boxColor = index % 2 == 0 ? new Color(0.2f, 0.2f, 0.2f, 0.3f) : new Color(0.25f, 0.25f, 0.25f, 0.3f);
            EditorGUILayout.BeginVertical(boxStyle);
            Rect targetHeaderRect = EditorGUILayout.GetControlRect(false, 20);
            EditorGUI.DrawRect(targetHeaderRect, boxColor);

            Rect foldoutRect = new Rect(targetHeaderRect.x + 5, targetHeaderRect.y, targetHeaderRect.width - 80, targetHeaderRect.height);
            Rect deleteRect = new Rect(targetHeaderRect.x + targetHeaderRect.width - 75, targetHeaderRect.y + 2, 70, 16);

            string headerLabel = graphicProp.objectReferenceValue != null 
                ? $"   {targetNameProp.stringValue}" 
                : $"   Target (Empty)";
            
            targetFoldouts[index] = EditorGUI.Foldout(foldoutRect, targetFoldouts[index], headerLabel, true, EditorStyles.boldLabel);

            if (GUI.Button(deleteRect, "Remove", EditorStyles.miniButton))
            {
                graphicTargetsProp.DeleteArrayElementAtIndex(index);
                targetFoldouts.Remove(index);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (targetFoldouts[index])
            {
                EditorGUILayout.Space(5);
                EditorGUI.BeginChangeCheck();
                
                UnityEngine.Object assignedObject = graphicProp.objectReferenceValue;

                if (isImageTarget)
                {
                    graphicProp.objectReferenceValue = EditorGUILayout.ObjectField("Target Image", assignedObject, typeof(Image), true);
                }
                else if (isTextTarget)
                {
                    graphicProp.objectReferenceValue = EditorGUILayout.ObjectField("Target Text (TMP)", assignedObject, typeof(TextMeshProUGUI), true);
                }
                else
                {
                    EditorGUILayout.PropertyField(graphicProp);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    if (graphicProp.objectReferenceValue != null)
                    {
                        targetNameProp.stringValue = graphicProp.objectReferenceValue.name;
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                
                if (graphicProp.objectReferenceValue != null)
                {
                    EditorGUILayout.Space(5);
                    
                    BaseGraphicTarget.GraphicType currentType = isTextTarget ? BaseGraphicTarget.GraphicType.TextMeshPro : BaseGraphicTarget.GraphicType.Image;
                    DrawTargetProperties(targetProp, currentType);
                }
                else
                {
                    string neededType = isImageTarget ? "Image" : "TextMeshProUGUI";
                    EditorGUILayout.HelpBox($"Please assign an {neededType} component.", MessageType.Warning);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawTargetProperties(SerializedProperty targetProp, BaseGraphicTarget.GraphicType graphicType)
        {
            EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("_rectTransform"));
            EditorGUILayout.Space(5f);
            
            DrawColorTransitions(targetProp);
            DrawScaleTransitions(targetProp);
            
            if (graphicType == BaseGraphicTarget.GraphicType.Image)
            {
                DrawSpriteSwap(targetProp);
            }
            
            DrawMaterialSwap(targetProp);
            
            if (graphicType == BaseGraphicTarget.GraphicType.TextMeshPro)
            {
                DrawTextColorChange(targetProp);
            }
            
            DrawFadeTransitions(targetProp);
            DrawRotationTransitions(targetProp);
            DrawPositionOffset(targetProp);
            DrawPunchEffects(targetProp);
            DrawShakeEffects(targetProp);
        }

        private void DrawColorTransitions(SerializedProperty targetProp)
        {
            SerializedProperty useColorProp = targetProp.FindPropertyRelative("useColorTransition");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useColorProp.boolValue = EditorGUILayout.ToggleLeft("Color Transitions", useColorProp.boolValue, EditorStyles.boldLabel);
        
            if (useColorProp.boolValue)
            {
                
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledColor"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawScaleTransitions(SerializedProperty targetProp)
        {
            SerializedProperty useScaleProp = targetProp.FindPropertyRelative("useScaleTransition");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useScaleProp.boolValue = EditorGUILayout.ToggleLeft("Scale Transitions", useScaleProp.boolValue, EditorStyles.boldLabel);
        
            if (useScaleProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalScale"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedScale"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedScale"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedScale"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledScale"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("scaleEase"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSpriteSwap(SerializedProperty targetProp)
        {
            SerializedProperty useSpriteProp = targetProp.FindPropertyRelative("useSpriteSwap");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useSpriteProp.boolValue = EditorGUILayout.ToggleLeft("Sprite Swap (Image Only)", useSpriteProp.boolValue, EditorStyles.boldLabel);
        
            if (useSpriteProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalSprite"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedSprite"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedSprite"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedSprite"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledSprite"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawMaterialSwap(SerializedProperty targetProp)
        {
            SerializedProperty useMaterialProp = targetProp.FindPropertyRelative("useMaterialSwap");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useMaterialProp.boolValue = EditorGUILayout.ToggleLeft("Material Swap", useMaterialProp.boolValue, EditorStyles.boldLabel);
        
            if (useMaterialProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalMaterial"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedMaterial"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedMaterial"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedMaterial"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledMaterial"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawTextColorChange(SerializedProperty targetProp)
        {
            SerializedProperty useTextColorProp = targetProp.FindPropertyRelative("useTextColorChange");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useTextColorProp.boolValue = EditorGUILayout.ToggleLeft("Text Color Change (TMP Only)", useTextColorProp.boolValue, EditorStyles.boldLabel);
        
            if (useTextColorProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalTextColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedTextColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedTextColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedTextColor"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledTextColor"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFadeTransitions(SerializedProperty targetProp)
        {
            SerializedProperty useFadeProp = targetProp.FindPropertyRelative("useFadeTransition");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useFadeProp.boolValue = EditorGUILayout.ToggleLeft("Fade Transitions", useFadeProp.boolValue, EditorStyles.boldLabel);
        
            if (useFadeProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("_canvasGroup"));
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalAlpha"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedAlpha"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedAlpha"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedAlpha"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledAlpha"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawRotationTransitions(SerializedProperty targetProp)
        {
            SerializedProperty useRotationProp = targetProp.FindPropertyRelative("useRotationTransition");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useRotationProp.boolValue = EditorGUILayout.ToggleLeft("Rotation Transitions", useRotationProp.boolValue, EditorStyles.boldLabel);
        
            if (useRotationProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalRotation"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedRotation"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedRotation"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedRotation"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledRotation"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("rotationEase"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPositionOffset(SerializedProperty targetProp)
        {
            SerializedProperty usePositionProp = targetProp.FindPropertyRelative("usePositionOffset");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            usePositionProp.boolValue = EditorGUILayout.ToggleLeft("Position Offset", usePositionProp.boolValue, EditorStyles.boldLabel);
        
            if (usePositionProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("normalOffset"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("highlightedOffset"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("pressedOffset"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("selectedOffset"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("disabledOffset"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("positionEase"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPunchEffects(SerializedProperty targetProp)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Punch Effects (On Click)", EditorStyles.boldLabel);
        
            EditorGUI.indentLevel++;
        
            SerializedProperty usePunchScaleProp = targetProp.FindPropertyRelative("usePunchScale");
            usePunchScaleProp.boolValue = EditorGUILayout.ToggleLeft("Use Punch Scale", usePunchScaleProp.boolValue);
            if (usePunchScaleProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchScaleStrength"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchDuration"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchVibrato"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchElasticity"));
                EditorGUI.indentLevel--;
            }

            SerializedProperty usePunchRotationProp = targetProp.FindPropertyRelative("usePunchRotation");
            usePunchRotationProp.boolValue = EditorGUILayout.ToggleLeft("Use Punch Rotation", usePunchRotationProp.boolValue);
            if (usePunchRotationProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchRotationStrength"));
                EditorGUI.indentLevel--;
            }

            SerializedProperty usePunchPositionProp = targetProp.FindPropertyRelative("usePunchPosition");
            usePunchPositionProp.boolValue = EditorGUILayout.ToggleLeft("Use Punch Position", usePunchPositionProp.boolValue);
            if (usePunchPositionProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("punchPositionStrength"));
                EditorGUI.indentLevel--;
            }
        
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DrawShakeEffects(SerializedProperty targetProp)
        {
            SerializedProperty useShakeProp = targetProp.FindPropertyRelative("useShakeOnHover");
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            useShakeProp.boolValue = EditorGUILayout.ToggleLeft("Shake on Hover", useShakeProp.boolValue, EditorStyles.boldLabel);
        
            if (useShakeProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("shakeDuration"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("shakeStrength"));
                EditorGUILayout.PropertyField(targetProp.FindPropertyRelative("shakeVibrato"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}