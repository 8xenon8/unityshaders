using UnityEngine;
using System.Collections.Generic;
using System;

namespace RailGeneration
{
    public class MeshManager
    {
        public Dictionary<string, List<Vector3>> ConvertSegmentToMeshVertices(
            RouteCalculator.Segment segment,
            float railwayWidth,
            float railRadius
        ) {
            Vector3 right = Vector3.Cross(segment.up, segment.direction).normalized;

            Vector3 rightRailPosition = segment.position + right * railwayWidth / 2;
            Vector3 leftRailPosition = segment.position - right * railwayWidth / 2;

            Vector3 upVectorForRailVertices = segment.up.normalized * railRadius;
            Vector3 rightVectorForRailVertices = right * railRadius;

            List<Vector3> rightRailVertices = new List<Vector3>();
            List<Vector3> leftRailVertices = new List<Vector3>();

            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI / 6));
            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI / 2) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI / 2));
            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 5 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 5 / 6));
            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 7 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 7 / 6));
            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 3 / 2) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 3 / 2));
            rightRailVertices.Add(rightRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 11 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 11 / 6));

            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI / 6));
            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI / 2) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI / 2));
            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 5 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 5 / 6));
            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 7 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 7 / 6));
            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 3 / 2) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 3 / 2));
            leftRailVertices.Add(leftRailPosition + upVectorForRailVertices * Mathf.Sin(Mathf.PI * 11 / 6) + rightVectorForRailVertices * Mathf.Cos(Mathf.PI * 11 / 6));

            return new Dictionary<string, List<Vector3>>()
            {
                { "left", leftRailVertices },
                { "right", rightRailVertices }
            };
        }

        public void CalculateMesh(
            List<RouteCalculator.Segment> routeSegments,
            Transform transform,
            float railWidthFrom,
            float railWidthTo,
            float railRadius
        ) {
            Mesh m = new Mesh();

            int facesCount = 6; // @TODO: Change int constant to variable to make it possible to set the number of faces of rail
            Vector3[] vertices =  new Vector3[routeSegments.Count * facesCount * 2];
            int arrayCounter = 0;

            for (int i = 0; i < routeSegments.Count; i++) {
                RouteCalculator.Segment segment = routeSegments[i];
                Dictionary<string, List<Vector3>> verticesTotal = ConvertSegmentToMeshVertices(
                    segment,
                    railWidthFrom + (railWidthTo - railWidthFrom) * (i / routeSegments.Count),
                    railRadius
                );

                for (int j = 0; j < verticesTotal["right"].Count; j++) {
                    vertices[arrayCounter] = transform.InverseTransformPoint(transform.parent.TransformPoint(verticesTotal["right"][j]));
                    vertices[routeSegments.Count * facesCount + arrayCounter] = transform.InverseTransformPoint(transform.parent.TransformPoint(verticesTotal["left"][j]));
                    arrayCounter++;
                }
            }

            List<int> triangles = new List<int>();
            for (int k = 0; k < routeSegments.Count * 2 - 1; k++) {

                if (k == routeSegments.Count - 1)
                    continue;

                int i = k * facesCount;
                triangles.Add(i);
                triangles.Add(i + 7);
                triangles.Add(i + 6);

                triangles.Add(i + 1);
                triangles.Add(i + 7);
                triangles.Add(i);

                triangles.Add(i + 1);
                triangles.Add(i + 8);
                triangles.Add(i + 7);

                triangles.Add(i + 2);
                triangles.Add(i + 8);
                triangles.Add(i + 1);

                triangles.Add(i + 2);
                triangles.Add(i + 9);
                triangles.Add(i + 8);

                triangles.Add(i + 3);
                triangles.Add(i + 9);
                triangles.Add(i + 2);

                triangles.Add(i + 3);
                triangles.Add(i + 10);
                triangles.Add(i + 9);

                triangles.Add(i + 4);
                triangles.Add(i + 10);
                triangles.Add(i + 3);

                triangles.Add(i + 4);
                triangles.Add(i + 11);
                triangles.Add(i + 10);

                triangles.Add(i + 5);
                triangles.Add(i + 11);
                triangles.Add(i + 4);

                triangles.Add(i + 5);
                triangles.Add(i + 6);
                triangles.Add(i + 11);

                triangles.Add(i);
                triangles.Add(i + 6);
                triangles.Add(i + 5);
            }

            //m.vertices[m.vertices.Length - 4] = new Vector3(routeSegments[0].position);

            m.SetVertices(vertices);
            m.triangles = triangles.ToArray();

            m.RecalculateTangents();
            m.RecalculateBounds();
            m.RecalculateNormals();

            transform.gameObject.GetComponent<MeshFilter>().sharedMesh = m;
            transform.gameObject.GetComponent<MeshCollider>().sharedMesh = m;
        }
    }
}
