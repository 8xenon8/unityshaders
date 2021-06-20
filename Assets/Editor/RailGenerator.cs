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
        List<GameObject> points = rail.points;

        MeshManager meshManager = new MeshManager();

        foreach (GameObject point in points)
        {
            Vector3 position = point.transform.position;
            point.transform.position = Handles.DoPositionHandle(point.transform.position, point.transform.rotation);
        }

        List<RouteCalculator.Segment> segments = new List<RouteCalculator.Segment>();

        if (points.Count >= 2)
        {
            for (int i = 1; i < points.Count; i++)
            {
                List<RouteCalculator.Segment> routePoints = rail.CalculateRouteToPoint(
                    points[i - 1].transform,
                    rail.transform.InverseTransformPoint(points[i].transform.position)
                );

                //MeshFilter r = points[i - 1].GetComponent<MeshFilter>();

                meshManager.CalculateMesh(routePoints, points[i - 1].transform, rail.railWidth, rail.railWidth, rail.railRadius);

                //if (i > 1) {
                //    MeshFilter meshFilter = points[i - 1].gameObject.GetComponent<MeshFilter>();

                //    Mesh currentMesh = meshFilter.sharedMesh;
                //    Mesh previousMesh = points[i - 2].GetComponent<MeshFilter>().sharedMesh;
                //    Vector3[] vertices = currentMesh.vertices;

                //    for (int j = 12; j > 0; j--) {
                //        vertices[currentMesh.vertices.Length - j - 1] = previousMesh.vertices[j] + Vector3.one;
                //    }

                //    currentMesh.SetVertices(vertices);

                //    currentMesh.RecalculateNormals();
                //    currentMesh.RecalculateTangents();
                //    currentMesh.RecalculateBounds();

                //    meshFilter.sharedMesh = currentMesh;
                //}

                segments.AddRange(routePoints);

                points[i].transform.LookAt(
                    points[i].transform.position + routePoints[routePoints.Count - 1].direction,
                    Vector3.up
                );
            }

            for (int j = 1; j < segments.Count; j++)
            {
                Handles.color = Color.red;
                Handles.DrawLine(
                    rail.transform.TransformPoint(segments[j - 1].position),
                    rail.transform.TransformPoint(segments[j].position)
                );
                Handles.color = Color.white;
            }
        }

        Vector3 tmpPoint = handleTransform.TransformPoint(transformLocalPoint);

        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck())
        {
            transformLocalPoint = handleTransform.InverseTransformPoint(tmpPoint);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //if (GUILayout.Button("Add segment")) {
        //    rail.CreateSegment(transformLocalPoint);
        //}

        if (GUILayout.Button("Reset"))
        {
            rail.Reset();
            transformLocalPoint = Vector3.forward;
        }

        if (GUILayout.Button("Add segment"))
        {
            rail.AddSegment();
            transformLocalPoint = Vector3.forward;
        }
    }
}
