using UnityEngine;
using System.Collections.Generic;

namespace RailGeneration
{
    class MeshManager
    {
        private List<Vector3> rightRailVertices = new List<Vector3>();
        private List<Vector3> leftRailVertices = new List<Vector3>();
        private List<int> triangles = new List<int>();

        public void AddNewRailSegment(
            Vector3 position,
            Vector3 direction,
            Vector3 up,
            float railwayWidth,
            float railRadius
        ) {
            Vector3 right = Vector3.Cross(up, direction).normalized;

            Vector3 rightRailPosition = position + right * railwayWidth / 2;
            Vector3 leftRailPosition = position - right * railwayWidth / 2;

            Vector3 upVectorForRailVertices = up.normalized * railRadius;
            Vector3 rightVectorForRailVertices = right * railRadius;

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
        }

        private void CalculateTriangles()
        {

        }

        public void GenerateMesh(MeshRenderer renderer)
        {
            List<Vector3> totalVertices = new List<Vector3>();
            totalVertices.AddRange(rightRailVertices);
            totalVertices.AddRange(leftRailVertices);

            Vector3[] totalVerticesArray = totalVertices.ToArray();

            //int[] triangles = new int[];
        }
    }
}
