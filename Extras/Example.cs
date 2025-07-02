using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Introduces the core Animation functionality and Queue system
using FlowKit.Core;
// Introduces common tweening types and utilities
using FlowKit.Common;

namespace FlowKit.Extras
{
    [AddComponentMenu("")]
    public class Example : MonoBehaviour
    {
        [Header("UI Panel References")]
        [SerializeField] GameObject optionsPanel;
        [SerializeField] GameObject titlePanel;
        [SerializeField] GameObject disclaimerPanel;
        
        private FlowKitEngine _optionsFK;
        private FlowKitEngine _titleFK;
        private FlowKitEngine _disclaimerFK;

        [Header("Example Controls")]
        [SerializeField] private bool useQueue = false;

        void Awake()
        {
            _optionsFK = optionsPanel.GetComponent<FlowKitEngine>();
            _titleFK = titlePanel.GetComponent<FlowKitEngine>();
            _disclaimerFK = disclaimerPanel.GetComponent<FlowKitEngine>();
        }

        void Start()
        {
            _optionsFK.SetPanelVisibility(true);

            if (useQueue)
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
            // No need for a seperate function here since these are simple animations
            _titleFK.TransitionFromTop(AnimationTarget.Text, 1, 650, EasingType.EaseIn, 2f);
            _disclaimerFK.TypeWriteWithDuration(1, 3f);

            _optionsFK.Queue(new AnimationStep[]
            {
                // Set Button & Button text visibility to invisible instantly
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Button, 4, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeOut(AnimationTarget.Text, 4, 0f)),
                AnimationStep.Wait(0.5f),

                // Start transitioning in the buttons and make them visible
                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 1, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 1, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 1, 0f)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Alternate direction for visual interest
                AnimationStep.Call(() => _optionsFK.TransitionFromRight(AnimationTarget.Button, 2, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 2, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 2, 0f)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Revert back to left transition
                AnimationStep.Call(() => _optionsFK.TransitionFromLeft(AnimationTarget.Button, 3, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 3, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 3, 0f)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Alternate direction for visual interest
                AnimationStep.Call(() => _optionsFK.TransitionFromRight(AnimationTarget.Button, 4, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Button, 4, 0f)),
                AnimationStep.Call(() => _optionsFK.FadeIn(AnimationTarget.Text, 4, 0f)),
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
            // No need for a coroutine here since these are simple animations
            _titleFK.TransitionFromTop(AnimationTarget.Text, 1, 650, EasingType.EaseIn, 2f);
            _disclaimerFK.TypeWriteWithDuration(1, 3f);

            StartCoroutine(TweenRoutine());
        }

        private IEnumerator TweenRoutine()
        {
            // Set Button & Button text visibility to invisible instantly
            for (int i = 1; i <= 4; i++)
            {
                _optionsFK.FadeOut(AnimationTarget.Button, i, 0f);
                _optionsFK.FadeOut(AnimationTarget.Text, i, 0f);
            }

            // Slight delay before starting the animation
            yield return new WaitForSeconds(0.5f);

            // Start transitioning in the buttons and make them visible
            for (int i = 1; i <= 4; i++)
            {
                // Alternate direction for visual interest
                if (i % 2 == 0)
                {
                    _optionsFK.TransitionFromRight(AnimationTarget.Button, i, 1000f, EasingType.EaseIn, 2f);
                }
                else
                {
                    _optionsFK.TransitionFromLeft(AnimationTarget.Button, i, 1000f, EasingType.EaseIn, 2f);
                }

                // Set Button & Button text visibility to visible instantly
                _optionsFK.FadeIn(AnimationTarget.Button, i, 0f);
                _optionsFK.FadeIn(AnimationTarget.Text, i, 0f);

                // Delay between each button transition
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}