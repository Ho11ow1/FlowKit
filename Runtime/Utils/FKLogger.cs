using UnityEngine;

internal static class FKLogger
{
    public static void NullObject<T>(string method, string source)
    {
        #if UNITY_EDITOR
        Debug.LogError($"[{typeof(T).Name}] Null 'RectTransform' passed to '{method}' on '{source}'.");
        #endif
    }

    public static void MissingCanvasGroup<T>(string target)
    {
        #if UNITY_EDITOR
        Debug.LogWarning($"[{typeof(T).Name}] No 'CanvasGroup' found on '{target}'. Add one for alpha control.");
        #endif
    }

    public static void UnknownDirection<T>(string method, string direction, string source)
    {
        #if UNITY_EDITOR
        Debug.LogError($"[{typeof(T).Name}] Unknown 'Direction'({direction}) passed to '{method}' on '{source}'");
        #endif
    }

}
