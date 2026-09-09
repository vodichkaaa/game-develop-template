using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Custom.Editor
{
    public class TweenEasePreviewWindow : EditorWindow
    {
        private enum AnimationType { Move, Scale, Rotate, MoveAndScale }
    
        private Sprite testSprite;
        private AnimationType animationType = AnimationType.Move;
    
        // Move settings
        private Vector2 startPosition = new Vector2(0, 0);
        private Vector2 endPosition = new Vector2(300, 0);
    
        // Scale settings
        private float startScale = 0.5f;
        private float endScale = 1.5f;
    
        // Rotate settings
        private float startRotation = 0f;
        private float endRotation = 360f;
    
        private float duration = 1f;
        private Vector2 scrollPosition;
    
        // Animation state
        private Dictionary<Ease, float> animationProgress = new Dictionary<Ease, float>();
        private double lastUpdateTime;
    
        private Ease[] easesToShow = new Ease[]
        {
            Ease.Linear,
            Ease.InSine, Ease.OutSine, Ease.InOutSine,
            Ease.InQuad, Ease.OutQuad, Ease.InOutQuad,
            Ease.InCubic, Ease.OutCubic, Ease.InOutCubic,
            Ease.InQuart, Ease.OutQuart, Ease.InOutQuart,
            Ease.InQuint, Ease.OutQuint, Ease.InOutQuint,
            Ease.InExpo, Ease.OutExpo, Ease.InOutExpo,
            Ease.InCirc, Ease.OutCirc, Ease.InOutCirc,
            Ease.InElastic, Ease.OutElastic, Ease.InOutElastic,
            Ease.InBack, Ease.OutBack, Ease.InOutBack,
            Ease.InBounce, Ease.OutBounce, Ease.InOutBounce
        };
    
        // Colors
        private Color backgroundColor = new Color(0.15f, 0.15f, 0.15f);
        private Color panelColor = new Color(0.2f, 0.2f, 0.2f);
        private Color accentColor = new Color(0.3f, 0.5f, 0.8f);
    
        [MenuItem("Tools/Tween Ease Preview")]
        public static void ShowWindow()
        {
            var window = GetWindow<TweenEasePreviewWindow>("Tween Ease Preview");
            window.maxSize = new Vector2(4000, 4000);
            window.minSize = new Vector2(800, 600);
        }
    
        private void OnEnable()
        {
            foreach (Ease ease in easesToShow)
            {
                animationProgress[ease] = 0f;
            }
            lastUpdateTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += OnEditorUpdate;
        }
    
        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
    
        private void OnEditorUpdate()
        {
            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastUpdateTime);
            lastUpdateTime = currentTime;
        
            foreach (Ease ease in easesToShow)
            {
                animationProgress[ease] += deltaTime / duration;
                if (animationProgress[ease] > 1f)
                {
                    animationProgress[ease] = 0f;
                }
            }
        
            Repaint();
        }
    
        private void OnGUI()
        {
            DrawHeader();
            DrawControls();
            DrawPreviews();
        }
    
        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(10);
        
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = accentColor }
            };
        
            EditorGUILayout.LabelField("🎬 Tween Ease Preview", titleStyle, GUILayout.Height(35));
        
            GUIStyle subtitleStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.gray }
            };
        
            EditorGUILayout.LabelField("Live animation preview for all DOTween easing functions", subtitleStyle);
        
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }
    
        private void DrawControls()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(10);
        
            // Sprite selection
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Sprite", EditorStyles.boldLabel, GUILayout.Width(80));
            testSprite = (Sprite)EditorGUILayout.ObjectField(testSprite, typeof(Sprite), false, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        
            GUILayout.Space(10);
        
            // Animation type selection
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fixedHeight = 30,
                fixedWidth = 120
            };
        
            if (GUILayout.Button(animationType == AnimationType.Move ? "▶ Move" : "Move", buttonStyle))
                animationType = AnimationType.Move;
        
            if (GUILayout.Button(animationType == AnimationType.Scale ? "▶ Scale" : "Scale", buttonStyle))
                animationType = AnimationType.Scale;
        
            if (GUILayout.Button(animationType == AnimationType.Rotate ? "▶ Rotate" : "Rotate", buttonStyle))
                animationType = AnimationType.Rotate;
        
            if (GUILayout.Button(animationType == AnimationType.MoveAndScale ? "▶ Move + Scale" : "Move + Scale", buttonStyle))
                animationType = AnimationType.MoveAndScale;
        
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        
            GUILayout.Space(15);
        
            // Animation-specific settings
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(600));
        
            switch (animationType)
            {
                case AnimationType.Move:
                    EditorGUILayout.LabelField("Move Settings", EditorStyles.boldLabel);
                    startPosition = EditorGUILayout.Vector2Field("Start Position", startPosition);
                    endPosition = EditorGUILayout.Vector2Field("End Position", endPosition);
                    break;
                
                case AnimationType.Scale:
                    EditorGUILayout.LabelField("Scale Settings", EditorStyles.boldLabel);
                    startScale = EditorGUILayout.Slider("Start Scale", startScale, 0.1f, 3f);
                    endScale = EditorGUILayout.Slider("End Scale", endScale, 0.1f, 3f);
                    break;
                
                case AnimationType.Rotate:
                    EditorGUILayout.LabelField("Rotation Settings", EditorStyles.boldLabel);
                    startRotation = EditorGUILayout.Slider("Start Rotation", startRotation, -360f, 360f);
                    endRotation = EditorGUILayout.Slider("End Rotation", endRotation, -360f, 360f);
                    break;
                
                case AnimationType.MoveAndScale:
                    EditorGUILayout.LabelField("Move + Scale Settings", EditorStyles.boldLabel);
                    startPosition = EditorGUILayout.Vector2Field("Start Position", startPosition);
                    endPosition = EditorGUILayout.Vector2Field("End Position", endPosition);
                    startScale = EditorGUILayout.Slider("Start Scale", startScale, 0.1f, 3f);
                    endScale = EditorGUILayout.Slider("End Scale", endScale, 0.1f, 3f);
                    break;
            }
        
            GUILayout.Space(5);
            duration = EditorGUILayout.Slider("Duration", duration, 0.1f, 3f);
        
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }
    
        private void DrawPreviews()
        {
            GUILayout.Space(10);
        
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
            int columns = Mathf.Max(3, Mathf.FloorToInt(position.width / 280f));
            int currentColumn = 0;
        
            EditorGUILayout.BeginHorizontal();
        
            foreach (Ease ease in easesToShow)
            {
                if (currentColumn >= columns)
                {
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.BeginHorizontal();
                    currentColumn = 0;
                }
            
                DrawEasePreview(ease);
                GUILayout.Space(10);
                currentColumn++;
            }
        
            // Fill remaining space
            GUILayout.FlexibleSpace();
        
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    
        private void DrawEasePreview(Ease ease)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(250), GUILayout.Height(220));
        
            // Title
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13,
                normal = { textColor = accentColor }
            };
        
            EditorGUILayout.LabelField(ease.ToString(), titleStyle, GUILayout.Height(25));
        
            // Animation preview area
            Rect previewRect = GUILayoutUtility.GetRect(230, 150);
        
            // Draw background
            EditorGUI.DrawRect(previewRect, backgroundColor);
        
            // Draw animated preview
            DrawAnimatedPreview(previewRect, ease);
        
            GUILayout.Space(5);
        
            // Progress bar
            float progress = animationProgress[ease];
            Rect progressRect = GUILayoutUtility.GetRect(230, 3);
            EditorGUI.DrawRect(progressRect, new Color(0.3f, 0.3f, 0.3f));
            Rect progressFillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * progress, progressRect.height);
            EditorGUI.DrawRect(progressFillRect, accentColor);
        
            GUILayout.Space(5);
        
            EditorGUILayout.EndVertical();
        }
    
        private void DrawAnimatedPreview(Rect rect, Ease ease)
        {
            float t = animationProgress[ease];
            
            // Calculate current transform
            Vector2 position = Vector2.zero;
            float scale = 1f;
            float rotation = 0f;
            
            switch (animationType)
            {
                case AnimationType.Move:
                    // DOTween DoMove uses direct value easing - NO CLAMPING for overshoots!
                    float easedT_Move = DOVirtual.EasedValue(0, 1, t, ease);
                    // Allow values outside 0-1 range for back/elastic eases
                    position = startPosition + (endPosition - startPosition) * easedT_Move;
                    break;
                    
                case AnimationType.Scale:
                    // DOTween DoScale uses direct value easing
                    float easedT_Scale = DOVirtual.EasedValue(0, 1, t, ease);
                    // Allow negative/overshooting scale values
                    scale = startScale + (endScale - startScale) * easedT_Scale;
                    break;
                    
                case AnimationType.Rotate:
                    // DOTween DoRotate has special handling for angles
                    float easedT_Rotate = DOVirtual.EasedValue(0, 1, t, ease);
                    rotation = startRotation + (endRotation - startRotation) * easedT_Rotate;
                    break;
                    
                case AnimationType.MoveAndScale:
                    // Both animations use the same eased time value
                    float easedT_Combined = DOVirtual.EasedValue(0, 1, t, ease);
                    position = startPosition + (endPosition - startPosition) * easedT_Combined;
                    scale = startScale + (endScale - startScale) * easedT_Combined;
                    break;
            }
            
            // Draw easing curve graph
            DrawEasingCurveGraph(rect, ease, t);
            
            // Draw coordinate grid and axes
            DrawCoordinateSystem(rect);
            
            // Draw path
            DrawPath(rect, ease);
            
            // Draw start and end markers
            DrawStartEndMarkers(rect);
            
            // Draw sprite/circle at current position
            DrawSpriteAtPosition(rect, position, scale, rotation);
            
            // Draw value labels
            DrawValueLabels(rect, position, scale, rotation, t);
        }

        private void DrawEasingCurveGraph(Rect rect, Ease ease, float currentTime)
        {
            // Define graph area (top-left corner of preview)
            float graphSize = 60f;
            Rect graphRect = new Rect(rect.x + 5, rect.y + 25, graphSize, graphSize);
            
            // Draw graph background
            EditorGUI.DrawRect(graphRect, new Color(0.1f, 0.1f, 0.1f, 0.8f));
            
            // Draw graph border
            Handles.BeginGUI();
            Handles.color = new Color(0.4f, 0.4f, 0.4f, 0.8f);
            Handles.DrawPolyLine(
                new Vector3(graphRect.x, graphRect.y, 0),
                new Vector3(graphRect.x + graphRect.width, graphRect.y, 0),
                new Vector3(graphRect.x + graphRect.width, graphRect.y + graphRect.height, 0),
                new Vector3(graphRect.x, graphRect.y + graphRect.height, 0),
                new Vector3(graphRect.x, graphRect.y, 0)
            );
            
            // Draw easing curve
            int steps = 50;
            Vector3[] curvePoints = new Vector3[steps];
            
            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)(steps - 1);
                float easedValue = DOVirtual.EasedValue(0, 1, t, ease);
                
                // Clamp for visualization (some eases go beyond 0-1)
                float visualValue = easedValue;
                
                curvePoints[i] = new Vector3(
                    graphRect.x + t * graphRect.width,
                    graphRect.y + graphRect.height - (visualValue * graphRect.height),
                    0
                );
            }
            
            // Draw the curve line
            Handles.color = new Color(1f, 0.6f, 0.2f, 1f); // Orange color
            Handles.DrawAAPolyLine(2f, curvePoints);
            
            // Draw linear reference line (dashed)
            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
            Vector3 linearStart = new Vector3(graphRect.x, graphRect.y + graphRect.height, 0);
            Vector3 linearEnd = new Vector3(graphRect.x + graphRect.width, graphRect.y, 0);
            Handles.DrawDottedLine(linearStart, linearEnd, 2f);
            
            // Draw current position indicator
            float currentEasedValue = DOVirtual.EasedValue(0, 1, currentTime, ease);
            Vector3 currentPoint = new Vector3(
                graphRect.x + currentTime * graphRect.width,
                graphRect.y + graphRect.height - (currentEasedValue * graphRect.height),
                0
            );
            
            // Draw vertical line showing current time
            Handles.color = new Color(0.3f, 0.5f, 0.8f, 0.6f);
            Handles.DrawLine(
                new Vector3(currentPoint.x, graphRect.y, 0),
                new Vector3(currentPoint.x, graphRect.y + graphRect.height, 0)
            );
            
            // Draw current point on curve
            Handles.color = accentColor;
            Handles.DrawSolidDisc(currentPoint, Vector3.forward, 3f);
            
            Handles.EndGUI();
            
            // Draw axis labels
            GUIStyle miniLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 8,
                normal = { textColor = new Color(0.6f, 0.6f, 0.6f, 0.8f) }
            };
            
            GUI.Label(new Rect(graphRect.x - 5, graphRect.y - 2, 20, 15), "1", miniLabelStyle);
            GUI.Label(new Rect(graphRect.x - 5, graphRect.y + graphRect.height - 10, 20, 15), "0", miniLabelStyle);
            GUI.Label(new Rect(graphRect.x - 5, graphRect.y + graphRect.height + 2, 20, 15), "0", miniLabelStyle);
            GUI.Label(new Rect(graphRect.x + graphRect.width - 8, graphRect.y + graphRect.height + 2, 20, 15), "1", miniLabelStyle);
        }

        private void DrawCoordinateSystem(Rect rect)
        {
            Handles.BeginGUI();
            
            Vector2 center = rect.center;
            
            // Draw grid lines
            Handles.color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
            
            // Vertical lines
            for (int i = -2; i <= 2; i++)
            {
                float x = center.x + i * 50f;
                Handles.DrawLine(
                    new Vector3(x, rect.y, 0),
                    new Vector3(x, rect.y + rect.height, 0)
                );
            }
            
            // Horizontal lines
            for (int i = -1; i <= 1; i++)
            {
                float y = center.y + i * 50f;
                Handles.DrawLine(
                    new Vector3(rect.x, y, 0),
                    new Vector3(rect.x + rect.width, y, 0)
                );
            }
            
            // Draw axes (brighter)
            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            
            // X axis
            Handles.DrawLine(
                new Vector3(rect.x, center.y, 0),
                new Vector3(rect.x + rect.width, center.y, 0)
            );
            
            // Y axis
            Handles.DrawLine(
                new Vector3(center.x, rect.y, 0),
                new Vector3(center.x, rect.y + rect.height, 0)
            );
            
            Handles.EndGUI();
            
            // Draw axis labels
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9,
                normal = { textColor = new Color(0.6f, 0.6f, 0.6f, 0.8f) }
            };
            
            GUI.Label(new Rect(rect.x + rect.width - 15, center.y + 2, 15, 15), "X", labelStyle);
            GUI.Label(new Rect(center.x + 2, rect.y + 2, 15, 15), "Y", labelStyle);
        }

        private void DrawStartEndMarkers(Rect rect)
        {
            Handles.BeginGUI();
            
            Vector2 center = rect.center;
            
            // Calculate screen positions based on animation type
            Vector2 startScreenPos = center;
            Vector2 endScreenPos = center;
            
            switch (animationType)
            {
                case AnimationType.Move:
                case AnimationType.MoveAndScale:
                    startScreenPos = new Vector2(
                        center.x + startPosition.x * 0.4f,
                        center.y - startPosition.y * 0.4f
                    );
                    endScreenPos = new Vector2(
                        center.x + endPosition.x * 0.4f,
                        center.y - endPosition.y * 0.4f
                    );
                    break;
                    
                case AnimationType.Scale:
                case AnimationType.Rotate:
                    startScreenPos = new Vector2(center.x - 100, center.y);
                    endScreenPos = new Vector2(center.x + 100, center.y);
                    break;
            }
            
            // Draw start marker (green)
            Handles.color = new Color(0.2f, 0.8f, 0.2f, 0.6f);
            Handles.DrawSolidDisc(new Vector3(startScreenPos.x, startScreenPos.y, 0), Vector3.forward, 5f);
            
            // Draw end marker (red)
            Handles.color = new Color(0.8f, 0.2f, 0.2f, 0.6f);
            Handles.DrawSolidDisc(new Vector3(endScreenPos.x, endScreenPos.y, 0), Vector3.forward, 5f);
            
            Handles.EndGUI();
        }

        private void DrawValueLabels(Rect rect, Vector2 position, float scale, float rotation, float t)
        {
            GUIStyle valueStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9,
                normal = { textColor = new Color(1f, 1f, 1f, 0.9f) },
                alignment = TextAnchor.MiddleLeft
            };
            
            GUIStyle valueStyleSmall = new GUIStyle(GUI.skin.label)
            {
                fontSize = 8,
                normal = { textColor = new Color(0.8f, 0.8f, 0.8f, 0.8f) },
                alignment = TextAnchor.MiddleLeft
            };
            
            float labelX = rect.x + 5;
            float labelY = rect.y + rect.height - 45;
            
            // Progress
            GUI.Label(new Rect(labelX, labelY, 200, 15), 
                $"t: {t:F3}", valueStyle);
            
            switch (animationType)
            {
                case AnimationType.Move:
                    GUI.Label(new Rect(labelX, labelY + 12, 200, 15), 
                        $"Pos: ({position.x:F1}, {position.y:F1})", valueStyleSmall);
                    break;
                    
                case AnimationType.Scale:
                    GUI.Label(new Rect(labelX, labelY + 12, 200, 15), 
                        $"Scale: {scale:F3}", valueStyleSmall);
                    break;
                    
                case AnimationType.Rotate:
                    GUI.Label(new Rect(labelX, labelY + 12, 200, 15), 
                        $"Rot: {rotation:F1}°", valueStyleSmall);
                    break;
                    
                case AnimationType.MoveAndScale:
                    GUI.Label(new Rect(labelX, labelY + 12, 200, 15), 
                        $"Pos: ({position.x:F1}, {position.y:F1})", valueStyleSmall);
                    GUI.Label(new Rect(labelX, labelY + 24, 200, 15), 
                        $"Scale: {scale:F3}", valueStyleSmall);
                    break;
            }
        }
    
        private void DrawPath(Rect rect, Ease ease)
        {
            Handles.BeginGUI();
    
            int steps = 50;
            Vector3[] pathPoints = new Vector3[steps];
    
            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)(steps - 1);
                float easedT = DOVirtual.EasedValue(0, 1, t, ease);
        
                Vector2 pos = Vector2.zero;
        
                switch (animationType)
                {
                    case AnimationType.Move:
                    case AnimationType.MoveAndScale:
                        // Allow overshoot - don't clamp easedT
                        pos = startPosition + (endPosition - startPosition) * easedT;
                        break;
                
                    case AnimationType.Scale:
                        pos = new Vector2(t * 200 - 100, 0);
                        break;
                
                    case AnimationType.Rotate:
                        pos = new Vector2(t * 200 - 100, 0);
                        break;
                }
        
                pathPoints[i] = new Vector3(
                    rect.center.x + pos.x * 0.4f,
                    rect.center.y - pos.y * 0.4f,
                    0
                );
            }
    
            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Handles.DrawAAPolyLine(2f, pathPoints);
    
            Handles.EndGUI();
        }
    
        private void DrawSpriteAtPosition(Rect rect, Vector2 position, float scale, float rotation)
        {
            Vector2 screenPos = new Vector2(
                rect.center.x + position.x * 0.4f,
                rect.center.y - position.y * 0.4f
            );
        
            if (testSprite != null)
            {
                // Draw sprite
                Texture2D texture = testSprite.texture;
                Rect spriteRect = testSprite.textureRect;
            
                float size = 30f * scale;
                Rect drawRect = new Rect(screenPos.x - size / 2, screenPos.y - size / 2, size, size);
            
                Matrix4x4 matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(rotation, screenPos);
            
                GUI.DrawTextureWithTexCoords(drawRect, texture, new Rect(
                    spriteRect.x / texture.width,
                    spriteRect.y / texture.height,
                    spriteRect.width / texture.width,
                    spriteRect.height / texture.height
                ));
            
                GUI.matrix = matrixBackup;
            }
            else
            {
                // Draw circle
                Handles.BeginGUI();
                Handles.color = accentColor;
                Handles.DrawSolidDisc(new Vector3(screenPos.x, screenPos.y, 0), Vector3.forward, 8f * scale);
                Handles.EndGUI();
            }
        }
    }
}