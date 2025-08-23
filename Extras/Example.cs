using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Introduces the core Animation functionality and Queue system
using FlowKit;
// Introduces common tweening types and utilities
using FlowKit.Common;

namespace FlowKit.Extras
{
    [AddComponentMenu("")]
    public class Example : MonoBehaviour
    {
        [Header("UI Panel References")]
        [SerializeField] private FlowKitEngine titleFlowKit;
        [SerializeField] private FlowKitEngine buttonsFlowKit;
        [SerializeField] private FlowKitEngine disclaimerFlowKit;

        [Header("Example Controls")]
        [SerializeField] private bool useQueue = false;

        void Start()
        {
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
         * Queue System - Best for complex sequences on single elements (So not something like this):
         * - Popup that scales in, bounces, then shows typewriter text
         * - Loading screen with multiple animation phases
         * - Tutorial callouts with complex timing
         */
        private void OptionsQueue()
        {
            titleFlowKit.Text.TypeWriteWithDuration(1);
            disclaimerFlowKit.Text.TypeWriteWithDuration(1);

            bool fading = false;

            buttonsFlowKit.Queue(new AnimationStep[]
            {
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 1, fading)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 2, fading)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 3, fading)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 4, fading))
            },
            "Button FadeOut", true);

            buttonsFlowKit.Queue(new AnimationStep[]
            {                
                AnimationStep.Wait(0.5f),
                AnimationStep.Call(() => buttonsFlowKit.Movement.MoveFromLeft(AnimationTarget.Button, 1, 1000, 0.75f, EasingType.EaseOut)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 1, true)),
                AnimationStep.Wait(0.3f),
                AnimationStep.Call(() => buttonsFlowKit.Movement.MoveFromRight(AnimationTarget.Button, 2, 1000, 0.75f, EasingType.EaseOut)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 2, true)),
                AnimationStep.Wait(0.3f),
                AnimationStep.Call(() => buttonsFlowKit.Movement.MoveFromLeft(AnimationTarget.Button, 3, 1000, 0.75f, EasingType.EaseOut)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 3, true)),
                AnimationStep.Wait(0.3f),
                AnimationStep.Call(() => buttonsFlowKit.Movement.MoveFromRight(AnimationTarget.Button, 4, 1000, 0.75f, EasingType.EaseOut)),
                AnimationStep.Call(() => buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, 4, true)),
                AnimationStep.Wait(0.3f)
            },
            "Button Transition", true);

        }

        // ----------------------------------------------------- TWEEN EXAMPLE -----------------------------------------------------
        /*
         * Standard API + Coroutines - Best for group operations:
         * - Fading in a set of menu ButtonsArr with staggered timing
         * - Animating a grid of inventory slots
         * - Any repetitive pattern across multiple elements
         */
        private void OptionsTween()
        {
            titleFlowKit.Text.TypeWriteWithDuration(1);
            disclaimerFlowKit.Text.TypeWriteWithDuration(1);

            StartCoroutine(TweenRoutine());
        }

        private IEnumerator TweenRoutine()
        {
            for (int i = 1; i <= 4; i++)
            {
                buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, i, false);
            }

            yield return new WaitForSeconds(0.5f);
            
            for (int i = 1; i <= 4; i++)
            {
                if (i % 2 == 0)
                {
                    buttonsFlowKit.Movement.MoveFromRight(AnimationTarget.Button, i, 1000, 0.75f, EasingType.EaseOut);
                }
                else
                {
                    buttonsFlowKit.Movement.MoveFromLeft(AnimationTarget.Button, i, 1000, 0.75f, EasingType.EaseOut);
                }
                buttonsFlowKit.Visibility.SetVisibility(AnimationTarget.Button, i, true);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
