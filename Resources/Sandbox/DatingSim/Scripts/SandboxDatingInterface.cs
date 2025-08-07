using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlowKit;

public class SandboxDatingInterface : MonoBehaviour
{
    private FlowKitEngine flowKitEngine;

    void Awake()
    {
        flowKitEngine = GetComponent<FlowKitEngine>();    
    }

    void OnEnable()
    {
        if (gameObject.activeSelf) { return; } // For testing | allows the interface to be initially enabled without causing an error
        flowKitEngine.Transition.FromBottom(FlowKit.Common.AnimationTarget.Panel, 1, 2000, FlowKit.Common.EasingType.EaseOut, 0.3f);
        flowKitEngine.Visibility.FadeIn(FlowKit.Common.AnimationTarget.Panel, 1, 0.2f);
    }
}
