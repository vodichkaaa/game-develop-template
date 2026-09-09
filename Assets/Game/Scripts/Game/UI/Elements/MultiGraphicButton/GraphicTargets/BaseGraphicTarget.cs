using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.Elements.MultiGraphicButton.GraphicTargets
{
    [System.Serializable]
    public abstract class BaseGraphicTarget
    {
        public enum GraphicType
        {
            Image = 0,
            TextMeshPro = 1,
        }

        [HideInInspector]
        public string targetName = "New Target";
        
        public Graphic targetGraphic;
        
        [HideInInspector]
        public GraphicType targetType;
        
        [Header("Color Transitions")]
        public bool useColorTransition = true;
        public Color normalColor = Color.white;
        public Color highlightedColor = Color.white;
        public Color pressedColor = Color.white;
        public Color selectedColor = Color.white;
        public Color disabledColor = new Color(1f, 1f, 1f, 0.5f);

        [Header("Scale Transitions")]
        public bool useScaleTransition = false;
        public float normalScale = 1f;
        public float highlightedScale = 1.1f;
        public float pressedScale = 0.95f;
        public float selectedScale = 1f;
        public float disabledScale = 1f;
        public Ease scaleEase = Ease.OutQuad;

        [Header("Material Swap")]
        public bool useMaterialSwap = false;
        public Material normalMaterial;
        public Material highlightedMaterial;
        public Material pressedMaterial;
        public Material selectedMaterial;
        public Material disabledMaterial;

        [Header("Fade Transitions")]
        public bool useFadeTransition = false;
        public float normalAlpha = 1f;
        public float highlightedAlpha = 1f;
        public float pressedAlpha = 1f;
        public float selectedAlpha = 1f;
        public float disabledAlpha = 0.5f;

        [Header("Rotation Transitions")]
        public bool useRotationTransition = false;
        public Vector3 normalRotation = Vector3.zero;
        public Vector3 highlightedRotation = Vector3.zero;
        public Vector3 pressedRotation = Vector3.zero;
        public Vector3 selectedRotation = Vector3.zero;
        public Vector3 disabledRotation = Vector3.zero;
        public Ease rotationEase = Ease.OutQuad;

        [Header("Position Offset")]
        public bool usePositionOffset = false;
        public Vector2 normalOffset = Vector2.zero;
        public Vector2 highlightedOffset = Vector2.zero;
        public Vector2 pressedOffset = Vector2.zero;
        public Vector2 selectedOffset = Vector2.zero;
        public Vector2 disabledOffset = Vector2.zero;
        public Ease positionEase = Ease.OutQuad;

        [Header("Punch Effects (On Click)")]
        public bool usePunchScale = false;
        public Vector3 punchScaleStrength = new Vector3(0.2f, 0.2f, 0);
        public float punchDuration = 0.3f;
        public int punchVibrato = 10;
        public float punchElasticity = 1f;

        public bool usePunchRotation = false;
        public Vector3 punchRotationStrength = new Vector3(0, 0, 15f);

        public bool usePunchPosition = false;
        public Vector2 punchPositionStrength = new Vector2(5f, 5f);

        [Header("Shake Effects")]
        public bool useShakeOnHover = false;
        public float shakeDuration = 0.3f;
        public Vector3 shakeStrength = new Vector3(2f, 2f, 0);
        public int shakeVibrato = 10;

        [SerializeField]
        protected RectTransform _rectTransform;
        [SerializeField]
        protected CanvasGroup _canvasGroup;
        
        protected Vector3 originalScale;
        protected Vector2 originalPosition;
        protected Tweener scaleTweener;
        protected Tweener colorTweener;
        protected Tweener textColorTweener;
        protected Tweener fadeTweener;
        protected Tweener rotationTweener;
        protected Tweener positionTweener;
        protected Tweener punchTweener;
        protected Tweener shakeTweener;

        public void UpdateTargetType()
        {
            if (targetGraphic == null)
            {
                targetType = GraphicType.Image;
                targetName = "Empty Target";
                return;
            }

            if (targetGraphic is TextMeshProUGUI)
            {
                targetType = GraphicType.TextMeshPro;
                targetName = targetGraphic.name + " (TMP)";
            }
            else if (targetGraphic is Image)
            {
                targetType = GraphicType.Image;
                targetName = targetGraphic.name + " (Image)";
            }
            else
            {
                targetType = GraphicType.Image;
                targetName = targetGraphic.name + " (Graphic)";
            }
        }

        public virtual void Initialize()
        {
            if (_rectTransform != null)
            {
                originalScale = Vector3.one * normalScale;
                originalPosition = _rectTransform.anchoredPosition;
            }
        }

        public void ApplyState(MultiGraphicButton.ButtonState state, float transitionDuration)
        {
            if (targetGraphic == null) return;

            switch (state)
            {
                case MultiGraphicButton.ButtonState.Normal:
                    ApplyNormalState(transitionDuration);
                    break;
                case MultiGraphicButton.ButtonState.Highlighted:
                    ApplyHighlightedState(transitionDuration);
                    break;
                case MultiGraphicButton.ButtonState.Pressed:
                    ApplyPressedState(transitionDuration);
                    break;
                case MultiGraphicButton.ButtonState.Selected:
                    ApplySelectedState(transitionDuration);
                    break;
                case MultiGraphicButton.ButtonState.Disabled:
                    ApplyDisabledState(transitionDuration);
                    break;
            }
        }

        protected virtual void ApplyNormalState(float duration)
        {
            ApplyGraphicState(normalColor, normalScale, normalMaterial, 
                normalAlpha, normalRotation, normalOffset, duration);
        }

        protected virtual void ApplyHighlightedState(float duration)
        {
            ApplyGraphicState(highlightedColor, highlightedScale, highlightedMaterial, 
                highlightedAlpha, highlightedRotation, highlightedOffset, duration);
        }

        protected virtual void ApplyPressedState(float duration)
        {
            ApplyGraphicState(pressedColor, pressedScale, pressedMaterial, 
                pressedAlpha, pressedRotation, pressedOffset, duration);
        }

        protected virtual void ApplySelectedState(float duration)
        {
            ApplyGraphicState(selectedColor, selectedScale, selectedMaterial, 
                selectedAlpha, selectedRotation, selectedOffset, duration);
        }

        protected virtual void ApplyDisabledState(float duration)
        {
            ApplyGraphicState(disabledColor, disabledScale, disabledMaterial, 
                disabledAlpha, disabledRotation, disabledOffset, duration);
        }

        protected virtual void ApplyGraphicState(Color color, float scale, Material material, 
            float alpha, Vector3 rotation, Vector2 positionOffset, float duration)
        {
            // Color Transition
            if (useColorTransition)
            {
                colorTweener?.Kill();
                if (duration > 0)
                {
                    colorTweener = targetGraphic.DOColor(color, duration);
                }
                else
                {
                    targetGraphic.color = color;
                }
            }

            // Scale Transition
            if (useScaleTransition && _rectTransform != null)
            {
                Vector3 targetScale = originalScale * scale;
                scaleTweener?.Kill();
                
                if (duration > 0)
                {
                    scaleTweener = _rectTransform.DOScale(targetScale, duration).SetEase(scaleEase);
                }
                else
                {
                    _rectTransform.localScale = targetScale;
                }
            }

            // Material Swap
            if (useMaterialSwap && material != null)
            {
                targetGraphic.material = material;
            }

            // Fade Transition
            if (useFadeTransition && _canvasGroup != null)
            {
                fadeTweener?.Kill();
                
                if (duration > 0)
                {
                    fadeTweener = _canvasGroup.DOFade(alpha, duration);
                }
                else
                {
                    _canvasGroup.alpha = alpha;
                }
            }

            // Rotation Transition
            if (useRotationTransition && _rectTransform != null)
            {
                rotationTweener?.Kill();
                
                if (duration > 0)
                {
                    rotationTweener = _rectTransform.DOLocalRotate(rotation, duration).SetEase(rotationEase);
                }
                else
                {
                    _rectTransform.localEulerAngles = rotation;
                }
            }

            // Position Offset Transition
            if (usePositionOffset && _rectTransform != null)
            {
                Vector2 targetPosition = originalPosition + positionOffset;
                positionTweener?.Kill();
                
                if (duration > 0)
                {
                    positionTweener = _rectTransform.DOAnchorPos(targetPosition, duration).SetEase(positionEase);
                }
                else
                {
                    _rectTransform.anchoredPosition = targetPosition;
                }
            }
        }

        public void PlayPunchEffect()
        {
            if (_rectTransform == null) return;
            
            var originalScale = Vector3.one * normalScale;

            if (usePunchScale)
            {
                punchTweener?.Kill();
                punchTweener = _rectTransform.DOPunchScale(punchScaleStrength, punchDuration, punchVibrato, punchElasticity).OnComplete(() => _rectTransform.localScale = originalScale);
            }

            if (usePunchRotation)
            {
                _rectTransform.DOPunchRotation(punchRotationStrength, punchDuration, punchVibrato, punchElasticity);
            }

            if (usePunchPosition)
            {
                _rectTransform.DOPunchAnchorPos(punchPositionStrength, punchDuration, punchVibrato, punchElasticity);
            }
        }

        public void PlayShakeEffect()
        {
            if (useShakeOnHover && _rectTransform != null)
            {
                shakeTweener?.Kill();
                shakeTweener = _rectTransform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
            }
        }

        public void KillAllTweens()
        {
            scaleTweener?.Kill();
            colorTweener?.Kill();
            textColorTweener?.Kill();
            fadeTweener?.Kill();
            rotationTweener?.Kill();
            positionTweener?.Kill();
            punchTweener?.Kill();
            shakeTweener?.Kill();
        }
    }
}