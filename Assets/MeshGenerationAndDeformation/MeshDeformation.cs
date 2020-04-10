using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
    public float radius;

    void Start()
    {

    }
    
    void Update()
    {

        int layermask = LayerMask.NameToLayer("Terrain");
        layermask = ~layermask;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, layermask);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                int direction = Input.GetMouseButton(0) ? 1 : -1;
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                ProceduralMesh mesh = gameObject.GetComponent<ProceduralMesh>();
                Vector3[] vs = gameObject.GetComponent<MeshFilter>().mesh.vertices;

                List<Vector3> vertexPositions = new List<Vector3>();

                for (int i = Mathf.FloorToInt(localPosition.z - radius); i < Mathf.CeilToInt(localPosition.z + radius); i++)
                {
                    for (int j = Mathf.FloorToInt(localPosition.x - radius); j < Mathf.CeilToInt(localPosition.x + radius); j++)
                    {
                        float distance = Vector2.Distance(
                            new Vector2(localPosition.x, localPosition.z),
                            new Vector2(j, i)
                        );
                        if (distance < radius)
                        {
                            vertexPositions.Add(new Vector3(j, (1 - distance / radius) * direction, i));
                        }
                    }
                }

                foreach (Vector3 vertex in vertexPositions)
                {
                    int x = (int)vertex.x;
                    int y = (int)vertex.z;
                    if (
                        x >= 0 && x < mesh.verticesAccess.GetLength(0) &&
                        y >= 0 && y < mesh.verticesAccess.GetLength(1)
                    )
                    {
                        foreach (int vIndex in mesh.verticesAccess[(int)vertex.x, (int)vertex.z])
                        {
                            vs[vIndex].y += vertex.y / 5;
                        }
                    }

                }
                gameObject.GetComponent<MeshFilter>().mesh.vertices = vs;
                gameObject.GetComponent<MeshCollider>().sharedMesh.vertices = vs;
                gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
                gameObject.GetComponent<MeshCollider>().sharedMesh.RecalculateNormals();
                gameObject.GetComponent<MeshCollider>().sharedMesh.RecalculateTangents();
                gameObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            radius += Input.mouseScrollDelta.y;
            if (radius < 1)
            {
                radius = 1f;
            }
        }
    }
}
