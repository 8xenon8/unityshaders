using UnityEngine;
using System.Collections;

public static class GravityHelper
{
    private static Vector3 previousVector = Physics.gravity;

    //public static Vector3 up = Vector3.up;
    //public static Vector3 forward = Vector3.forward;
    //public static Vector3 right = Vector3.right;
    public static void SetGravity(Vector3 vector)
    {
        Quaternion rotation = Quaternion.FromToRotation(previousVector, vector);
        DimensionHelper.up = -vector.normalized;
        //forward = Vector3.ProjectOnPlane(Game.Current().player.forward, up).normalized;
        DimensionHelper.forward = rotation * DimensionHelper.forward;
        DimensionHelper.right = rotation * DimensionHelper.right;

        //right = Vector3.Cross(up, forward).normalized;

        previousVector = Physics.gravity;
        Physics.gravity = vector * 100;
    }
}
