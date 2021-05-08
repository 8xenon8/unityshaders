using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RailGeneration;

[CustomEditor(typeof(RouteCalculator))]
[ExecuteInEditMode]
public class RailGenerator : Editor
{
    private RouteCalculator rail;
    private Transform handleTransform;

    private Vector3 transformLocalPoint = Vector3.forward;

    private void OnSceneGUI()
    {
        rail = target as RouteCalculator;
        handleTransform = rail.transform;

        Vector3 tmpPoint = handleTransform.TransformPoint(transformLocalPoint);

        EditorGUI.BeginChangeCheck();

        tmpPoint = Handles.DoPositionHandle(tmpPoint, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(rail, "Move Point");
            //EditorUtility.SetDirty(rail);
            transformLocalPoint = handleTransform.InverseTransformPoint(tmpPoint);
        }

        List<Vector3>[] pointsAll = rail.CalculateRouteToPoint(transformLocalPoint);

        if (pointsAll.Length == 0) {
            return;
        }

        List<Vector3> points = pointsAll[0];

        for (int i = 1; i < points.Count; i++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(rail.transform.TransformPoint(points[i - 1]), rail.transform.TransformPoint(points[i]));
        }

        points = pointsAll[1];

        for (int i = 1; i < points.Count; i++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(rail.transform.TransformPoint(points[i - 1]), rail.transform.TransformPoint(points[i]));
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add segment")) {
            rail.CreateSegment(transformLocalPoint);
        }

        if (GUILayout.Button("Reset"))
        {
            rail.Reset();
            this.transformLocalPoint = Vector3.forward;
        }
    }
}
