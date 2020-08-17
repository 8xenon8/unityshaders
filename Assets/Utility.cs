﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void DrawCross(Vector3 point, Color color)
    {
        Debug.DrawLine(point - Vector3.left, point + Vector3.left, color, 0, false);
        Debug.DrawLine(point - Vector3.forward, point + Vector3.forward, color, 0, false);
        Debug.DrawLine(point - Vector3.up, point + Vector3.up, color, 0, false);
    }
}
