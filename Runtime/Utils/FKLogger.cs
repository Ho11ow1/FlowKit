using System;

using UnityEngine;

internal static class FKLogger
{
    public static void NullObject<T>(string method, string source)
    {
        #if UNITY_EDITOR
        Debug.LogError($"[{typeof(T).Name}] Null 'RectTransform' passed to '{method}' on '{source}'.");
        #endif
    }

    public static void MissingComponent<T>(Type component, string target)
    {
        #if UNITY_EDITOR
        Debug.LogWarning($"[{typeof(T).Name}] No '{component.Name}' found on '{target}'. Add one to enable {typeof(T).Name} functionality");
        #endif
    }

    public static void UnknownDirection<T>(string method, string direction, string source)
    {
        #if UNITY_EDITOR
        Debug.LogError($"[{typeof(T).Name}] Unknown 'Direction'({direction}) passed to '{method}' on '{source}'");
        #endif
    }

}
