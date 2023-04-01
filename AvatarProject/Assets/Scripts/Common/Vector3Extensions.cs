using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 TargetDirection(this Vector3 source, Vector3 target)
    {
        return target - source;
    }

    public static float Distance(this Vector3 source)
    {
        return source.x * source.x + source.z * source.z;
    }
}
