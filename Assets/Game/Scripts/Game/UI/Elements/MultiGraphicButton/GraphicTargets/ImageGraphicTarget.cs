using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.Elements.MultiGraphicButton.GraphicTargets
{
    [Serializable]
    public class ImageGraphicTarget : BaseGraphicTarget
    {
        [Header("Sprite Swap (Images Only)")]
        public bool useSpriteSwap = false;
        public Sprite normalSprite;
        public Sprite highlightedSprite;
        public Sprite pressedSprite;
        public Sprite selectedSprite;
        public Sprite disabledSprite;
        
        protected override void ApplyNormalState(float duration)
        {
            ApplyGraphicState(normalColor, normalScale, normalSprite, normalMaterial, 
                normalAlpha, normalRotation, normalOffset, duration);
        }

        protected override void ApplyHighlightedState(float duration)
        {
            ApplyGraphicState(highlightedColor, highlightedScale, highlightedSprite, highlightedMaterial, 
                highlightedAlpha, highlightedRotation, highlightedOffset, duration);
        }

        protected override void ApplyPressedState(float duration)
        {
            ApplyGraphicState(pressedColor, pressedScale, pressedSprite, pressedMaterial, 
                pressedAlpha, pressedRotation, pressedOffset, duration);
        }

        protected override void ApplySelectedState(float duration)
        {
            ApplyGraphicState(selectedColor, selectedScale, selectedSprite, selectedMaterial, 
                selectedAlpha, selectedRotation, selectedOffset, duration);
        }

        protected override void ApplyDisabledState(float duration)
        {
            ApplyGraphicState(disabledColor, disabledScale, disabledSprite, disabledMaterial, 
                disabledAlpha, disabledRotation, disabledOffset, duration);
        }
        
        private void ApplyGraphicState(Color color, float scale, Sprite sprite, Material material, 
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

            // Sprite Swap
            if (useSpriteSwap && targetGraphic is Image image && sprite != null)
            {
                image.sprite = sprite;
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
    }
}