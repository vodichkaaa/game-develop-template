using System.Collections.Generic;
using Game.Scripts.Game.UI.Elements.MultiGraphicButton.GraphicTargets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI.Elements.MultiGraphicButton
{
    [RequireComponent(typeof(RectTransform))]
    public class MultiGraphicButton : Button
    {
        public enum ButtonState
        {
            Normal = 0,
            Highlighted = 1,
            Pressed = 2,
            Selected = 3,
            Disabled = 4
        }

        [Header("Graphic Targets")]
        [SerializeReference]
        [SerializeField] private List<BaseGraphicTarget> graphicTargets = new();

        [Header("Transition Settings")]
        [SerializeField] private float transitionDuration = 0.1f;

        private ButtonState currentButtonState = ButtonState.Normal;

        protected override void Awake()
        {
            base.Awake();
            InitializeGraphicTargets();
        }

        private void InitializeGraphicTargets()
        {
            if (graphicTargets == null || graphicTargets.Count == 0)
            {
                return;
            }

            foreach (var target in graphicTargets)
            {
                // IMPORTANT: Check if the reference survived serialization
                if (target != null) 
                {
                    target.Initialize();
                }
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            ButtonState buttonState = ConvertToButtonState(state);
            currentButtonState = buttonState;
        
            float duration = instant ? 0 : transitionDuration;

            foreach (var target in graphicTargets)
            {
                target.ApplyState(buttonState, duration);
            }
        }

        private ButtonState ConvertToButtonState(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    return ButtonState.Normal;
                case SelectionState.Highlighted:
                    return ButtonState.Highlighted;
                case SelectionState.Pressed:
                    return ButtonState.Pressed;
                case SelectionState.Selected:
                    return ButtonState.Selected;
                case SelectionState.Disabled:
                    return ButtonState.Disabled;
                default:
                    return ButtonState.Normal;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        
            if (IsInteractable())
            {
                PlayPunchEffects();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        
            if (IsInteractable())
            {
                PlayShakeEffects();
            }
        }

        private void PlayPunchEffects()
        {
            foreach (var target in graphicTargets)
            {
                target.PlayPunchEffect();
            }
        }

        private void PlayShakeEffects()
        {
            foreach (var target in graphicTargets)
            {
                target.PlayShakeEffect();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            KillAllTweens();
        }

        private void KillAllTweens()
        {
            foreach (var target in graphicTargets)
            {
                target.KillAllTweens();
            }
        }
    }
}