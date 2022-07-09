using UnityEngine;

namespace AvatarBA.Debugging
{    
    // Custom Debug class to encapsulate the Unity Debug logs so it only works if we are
    // working on the editor
    public static class GameDebug
    {
        public static void Log(string message)
        {
            #if UNITY_EDITOR
            Debug.Log(message);
            #endif
        }

        public static void LogWarning(string message)
        {
            #if UNITY_EDITOR
            Debug.LogWarning(message);
            #endif
        }

        public static void LogError(string message)
        {
            #if UNITY_EDITOR
            Debug.LogError(message);
            #endif
        }
    }
}
