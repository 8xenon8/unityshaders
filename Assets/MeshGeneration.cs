using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{

    public int width;
    public int height;

    public float gridSize = 2f;

    //= new Vector3[]
    //{
    //    new Vector3(0, 0, 0),
    //    new Vector3(1, 0, 0),
    //    new Vector3(0, 1, 0),
    //    new Vector3(1, 1, 0),
    //    new Vector3(0, 0, 1),
    //    new Vector3(1, 0, 1),
    //    new Vector3(0, 1, 1),
    //    new Vector3(1, 1, 1),
    //};

    // Start is called before the first frame update
    void Start()
    {
        Mesh m = new Mesh();
        m.vertices = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[(width + 1) * (height + 1) * 6];

        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];

        for (int h = 0; h <= height; h++)
        {
            for (int w = 0; w <= width; w++)
            {
                int l = width * h + w;
                vertices[(width + 1) * h + w] = new Vector3(h, 0, w);
            }
        }

        m.vertices = vertices;

        int triangleCount = 0;

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                int v1 = h * (width + 1) + w;
                int v2 = h * (width + 1) + w + 1;
                int v3 = (h + 1) * (width + 1) + w;
                int v4 = (h + 1) * (width + 1) + w + 1;
                triangles[triangleCount++] = v1;
                triangles[triangleCount++] = v2;
                triangles[triangleCount++] = v3;
                triangles[triangleCount++] = v3;
                triangles[triangleCount++] = v2;
                triangles[triangleCount++] = v4;
            }
        }
        m.triangles = triangles;

        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = m;
        MeshRenderer r = gameObject.AddComponent<MeshRenderer>();

        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x / height, vertices[i].z / width);
        }

        m.uv = uv;

        m.RecalculateNormals();
        m.RecalculateBounds();

        MeshCollider c = gameObject.AddComponent<MeshCollider>();
        c.sharedMesh.vertices = m.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
