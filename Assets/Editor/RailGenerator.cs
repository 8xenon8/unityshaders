using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rail))]
[ExecuteInEditMode]
public class RailGenerator : Editor
{
    private Rail rail;
    private Transform handleTransform;

    private Vector3 newPoint = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneGUI()
    {
        rail = target as Rail;
        handleTransform = rail.transform;

        Vector3 tmpPoint = handleTransform.TransformPoint(newPoint);

        EditorGUI.BeginChangeCheck();

        tmpPoint = Handles.DoPositionHandle(tmpPoint, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(rail, "Move Point");
            //EditorUtility.SetDirty(rail);
            newPoint = handleTransform.InverseTransformPoint(tmpPoint);
        }

        List<Vector3> points = rail.CalculateRouteToPoint(newPoint);

        Handles.color = Color.red;
        Handles.DrawLine(handleTransform.position + handleTransform.right, handleTransform.position - handleTransform.right);
        Handles.DrawLine(handleTransform.position, handleTransform.position + handleTransform.forward);

        for (int i = 1; i < points.Count; i++) {
            Handles.color = Color.green;
            Handles.DrawLine(rail.transform.TransformPoint(points[i - 1]), rail.transform.TransformPoint(points[i]));
        }
        Handles.DrawLine(rail.transform.TransformPoint(points[points.Count - 1]), rail.transform.TransformPoint(points[1]));

        Handles.DrawWireArc(handleTransform.TransformPoint(points[1]), Vector3.up, Vector3.forward, 360f, rail.railWidth / 2f + rail.rotationSpacing);
    }
}
