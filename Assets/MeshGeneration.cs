using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{

    public int width;
    public int height;
    public float ratio = 1;

    public Material material;

    public Vector3[][,] verticesAccess;

    public void Generate()
    {
        int vsSize = (width + 1) * (height + 1);

        Mesh m = new Mesh();
        m.vertices = new Vector3[vsSize * 6];
        int[] triangles = new int[vsSize * 6];
        Vector3[] vertices = new Vector3[vsSize * 6];

        for (int h = 0; h <= height; h++)
        {
            for (int w = 0; w <= width; w++)
            {
                int l = width * h + w;
                vertices[((width + 1) * h + w) * 6]     = new Vector3(w, h * ratio, 0);
                vertices[((width + 1) * h + w) * 6 + 1] = new Vector3(w, h * ratio, 0);
                vertices[((width + 1) * h + w) * 6 + 2] = new Vector3(w, h * ratio, 0);
                vertices[((width + 1) * h + w) * 6 + 3] = new Vector3(w, h * ratio, 0);
                vertices[((width + 1) * h + w) * 6 + 4] = new Vector3(w, h * ratio, 0);
                vertices[((width + 1) * h + w) * 6 + 5] = new Vector3(w, h * ratio, 0);
            }
        }

        m.vertices = vertices;

        int triangleCount = 0;

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                int v1 = h * (width + 1) * 6 + w * 6;
                int v2 = h * (width + 1) * 6 + 6 + w * 6 + 1;
                int v3 = (h + 1) * (width + 1) * 6 + w * 6 + 2;
                
                int v4 = (h + 1) * (width + 1) * 6 + w * 6 + 3;
                int v5 = h * (width + 1) * 6 + 6 + w * 6 + 4;
                int v6 = (h + 1) * (width + 1) * 6 + 6 + w * 6 + 5;

                triangles[triangleCount++] = v1;
                triangles[triangleCount++] = v2;
                triangles[triangleCount++] = v3;

                triangles[triangleCount++] = v4;
                triangles[triangleCount++] = v5;
                triangles[triangleCount++] = v6;
            }
        }
        m.triangles = triangles;

        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = m;
        MeshRenderer r = gameObject.AddComponent<MeshRenderer>();

        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            float x, y;
            x = vertices[i].x / height;
            y = vertices[i].y / width;
            uv[i] = new Vector2(x, y);
        }

        m.uv = uv;

        m.RecalculateNormals();
        m.RecalculateBounds();

        MeshCollider c = gameObject.AddComponent<MeshCollider>();
        c.sharedMesh.vertices = m.vertices;

        GetComponent<MeshRenderer>().material = material;

        m.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
    }

    public Vector3[] GetVertices(int x, int y)
    {
        Mesh m = GetComponent<MeshFilter>().mesh;
        
        return new Vector3[]
        {
            m.vertices[((width + 1) * y + x) * 6],
            m.vertices[((width + 1) * y + x) * 6 + 1],
            m.vertices[((width + 1) * y + x) * 6 + 2],
            m.vertices[((width + 1) * y + x) * 6 + 3],
            m.vertices[((width + 1) * y + x) * 6 + 4],
            m.vertices[((width + 1) * y + x) * 6 + 5],
        };
    }

    public void SetVertices(int x, int y, Vector3 pos)
    {
        Mesh m = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] vs = m.vertices;

        vs[((width + 1) * y + x) * 6] = pos;
        vs[((width + 1) * y + x) * 6 + 1] = pos;
        vs[((width + 1) * y + x) * 6 + 2] = pos;
        vs[((width + 1) * y + x) * 6 + 3] = pos;
        vs[((width + 1) * y + x) * 6 + 4] = pos;
        vs[((width + 1) * y + x) * 6 + 5] = pos;

        gameObject.GetComponent<MeshFilter>().mesh.vertices = vs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
