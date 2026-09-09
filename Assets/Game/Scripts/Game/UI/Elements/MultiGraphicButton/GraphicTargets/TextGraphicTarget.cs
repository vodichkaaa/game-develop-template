using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Game.UI.Elements.MultiGraphicButton.GraphicTargets
{
    [Serializable]
    public class TextGraphicTarget : BaseGraphicTarget
    {
        [Header("Text Changes (TMP Only)")]
        public bool useTextColorChange = false;
        public Color normalTextColor = Color.white;
        public Color highlightedTextColor = Color.white;
        public Color pressedTextColor = Color.white;
        public Color selectedTextColor = Color.white;
        public Color disabledTextColor = new Color(1f, 1f, 1f, 0.5f);
        
        protected override void ApplyNormalState(float duration)
        {
            ApplyGraphicState(normalColor, normalScale, normalMaterial, 
                normalTextColor, normalAlpha, normalRotation, normalOffset, duration);
        }

        protected override void ApplyHighlightedState(float duration)
        {
            ApplyGraphicState(highlightedColor, highlightedScale, highlightedMaterial, 
                highlightedTextColor, highlightedAlpha, highlightedRotation, highlightedOffset, duration);
        }

        protected override void ApplyPressedState(float duration)
        {
            ApplyGraphicState(pressedColor, pressedScale, pressedMaterial, 
                pressedTextColor, pressedAlpha, pressedRotation, pressedOffset, duration);
        }

        protected override void ApplySelectedState(float duration)
        {
            ApplyGraphicState(selectedColor, selectedScale, selectedMaterial, 
                selectedTextColor, selectedAlpha, selectedRotation, selectedOffset, duration);
        }

        protected override void ApplyDisabledState(float duration)
        {
            ApplyGraphicState(disabledColor, disabledScale, disabledMaterial, 
                disabledTextColor, disabledAlpha, disabledRotation, disabledOffset, duration);
        }
        
        private void ApplyGraphicState(Color color, float scale, Material material, 
            Color textColor, float alpha, Vector3 rotation, Vector2 positionOffset, float duration)
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

            // Text Color Change
            if (useTextColorChange && targetGraphic is TextMeshProUGUI tmp)
            {
                textColorTweener?.Kill();
                
                if (duration > 0)
                {
                    textColorTweener = tmp.DOColor(textColor, duration);
                }
                else
                {
                    tmp.color = textColor;
                }
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
    }
}