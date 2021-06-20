using UnityEngine;
using System.Collections;

public static class DimensionHelper
{
    public static Vector3 forward = Vector3.forward;
    public static Vector3 up = Vector3.up;
    public static Vector3 right = Vector3.right;

    public static void SetDimensions(Vector3 up, Vector3 forward)
    {
        DimensionHelper.up = up.normalized;
        DimensionHelper.forward = forward.normalized;
        DimensionHelper.right = Vector3.Cross(DimensionHelper.up, DimensionHelper.forward).normalized * -1;
    }

    public static void SetDimensions(Vector3 up)
    {
        DimensionHelper.up = Vector3.ProjectOnPlane(up.normalized, DimensionHelper.up).normalized;
        DimensionHelper.right = Vector3.Cross(DimensionHelper.up, DimensionHelper.forward).normalized;
    }
}
