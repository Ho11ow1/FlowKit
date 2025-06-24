using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Introduces the core Tweening functionality and Queue system
using FlowKit.Core;
// Introduces common tweening types and utilities
using FlowKit.Common;

namespace FlowKit.Extras
{
    [AddComponentMenu("")]
    public class TweenExample : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] GameObject optionsPanel;
        [SerializeField] GameObject titlePanel;
        private FlowKitEngine _optionsFK;
        private FlowKitEngine _titleFK;

        [Header("Example Controls")]
        [SerializeField] private bool useQueueExample = false;

        void Awake()
        {
            _optionsFK = optionsPanel.GetComponent<FlowKitEngine>();
            _titleFK = titlePanel.GetComponent<FlowKitEngine>();
        }

        void Start()
        {
            _optionsFK.SetPanelVisibility(true);

            if (useQueueExample)
            {
                OptionsQueue();
            }
            else
            {
                OptionsTween();
            }
        }

        // ----------------------------------------------------- QUEUE EXAMPLE -----------------------------------------------------
        /*
         * Queue System - Best for complex sequences on single elements:
         * - Popup that scales in, bounces, then shows typewriter text
         * - Loading screen with multiple animation phases
         * - Tutorial callouts with complex timing
         */
        private void OptionsQueue()
        {
            _optionsFK.Queue(new AnimationStep[]
            {
                // Set button & Button text visibility to invisible instantly
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 4, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 4, 0f)),
                AnimationStep.Wait(0.5f),
                // Start transitioning in the buttons from the left and make them visible
                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 1, 650, EasingType.Cubic, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 1, 0f)),
                AnimationStep.Wait(1.5f), // Delay between each button transition

                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 2, 650, EasingType.Cubic, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 2, 0f)),
                AnimationStep.Wait(1.5f), // Delay between each button transition

                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 3, 650, EasingType.Cubic, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 3, 0f)),
                AnimationStep.Wait(1.5f), // Delay between each button transition

                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 4, 650, EasingType.Cubic, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 4, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 4, 0f)),
                AnimationStep.Wait(1.5f), // Delay between each button transition
            },
            name: "MenuLoad");

            _optionsFK.StartQueue("MenuLoad");
        }

        // ----------------------------------------------------- TWEEN EXAMPLE -----------------------------------------------------
        /*
         * Standard API + Coroutines - Best for group operations:
         * - Fading in a set of menu buttons with staggered timing
         * - Animating a grid of inventory slots
         * - Any repetitive pattern across multiple elements
         */
        private void OptionsTween()
        {
            StartCoroutine(TweenRoutine());
        }

        private IEnumerator TweenRoutine()
        {
            // Set button & Button text visibility to invisible instantly
            for (int i = 1; i <= 4; i++)
            {
                _optionsFK.FadeOut(AnimationTarget.Button, i, 0f);
                _optionsFK.FadeOut(AnimationTarget.Text, i, 0f);
            }

            yield return new WaitForSeconds(0.5f);

            // Start transitioning in the buttons from the left and make them visible
            for (int i = 1; i <= 4; i++)
            {
                _optionsFK.TransitionFromLeft(AnimationTarget.Button, i, 650, EasingType.Cubic, 2f);
                _optionsFK.FadeIn(AnimationTarget.Button, i, 0f);
                _optionsFK.FadeIn(AnimationTarget.Text, i, 0f);
                yield return new WaitForSeconds(1.5f); // Delay between each button transition
            }
        }
    }
}