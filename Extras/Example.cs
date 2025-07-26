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
        [SerializeField] private FlowKitEngine _titleFK;
        [SerializeField] private FlowKitEngine _optionsFK;
        [SerializeField] private FlowKitEngine _disclaimerFK;

        [Header("Example Controls")]
        [SerializeField] private bool useQueue = false;

        void Start()
        {
            _optionsFK.Visibility.SetPanelVisibility(true);

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
            _titleFK.Transition.FromTop(AnimationTarget.Text, 1, 650, EasingType.EaseIn, 2f);
            _disclaimerFK.Text.TypeWriteWithDuration(1, 3f);

            _optionsFK.Queue(new AnimationStep[]
            {
                // Set Button & Button text visibility to invisible instantly
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 1, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 1, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 2, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 2, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 3, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 3, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 4, false)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 4, false)),
                AnimationStep.Wait(0.5f),

                // Start transitioning in the buttons and make them visible
                AnimationStep.Call(() => _optionsFK.Transition.FromLeft(AnimationTarget.Button, 1, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 1, true)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 1, true)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Alternate direction for visual interest
                AnimationStep.Call(() => _optionsFK.Transition.FromRight(AnimationTarget.Button, 2, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 2, true)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 2, true)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Revert back to left transition
                AnimationStep.Call(() => _optionsFK.Transition.FromLeft(AnimationTarget.Button, 3, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 3, true)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 3, true)),
                // Delay before animating the next button
                AnimationStep.Wait(1.5f),

                // Alternate direction for visual interest
                AnimationStep.Call(() => _optionsFK.Transition.FromRight(AnimationTarget.Button, 4, 650, EasingType.EaseIn, 2f)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, 4, true)),
                AnimationStep.Call(() => _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, 4, true)),
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
            _titleFK.Transition.FromTop(AnimationTarget.Text, 1, 650, EasingType.EaseIn, 2f);
            _disclaimerFK.Text.TypeWriteWithDuration(1, 3f);

            StartCoroutine(TweenRoutine());
        }

        private IEnumerator TweenRoutine()
        {
            // Set Button & Button text visibility to invisible instantly
            for (int i = 1; i <= 4; i++)
            {
                _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, i, false);
                _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, i, false);
            }

            // Slight delay before starting the animation
            yield return new WaitForSeconds(0.5f);

            // Start transitioning in the buttons and make them visible
            for (int i = 1; i <= 4; i++)
            {
                // Alternate direction for visual interest
                if (i % 2 == 0)
                {
                    _optionsFK.Transition.FromRight(AnimationTarget.Button, i, 1000f, EasingType.EaseIn, 2f);
                }
                else
                {
                    _optionsFK.Transition.FromLeft(AnimationTarget.Button, i, 1000f, EasingType.EaseIn, 2f);
                }

                // Set Button & Button text visibility to visible instantly
                _optionsFK.Visibility.SetVisibility(AnimationTarget.Button, i, true);
                _optionsFK.Visibility.SetVisibility(AnimationTarget.Text, i, true);

                // Delay between each button transition
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}