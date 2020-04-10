using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{

    public int width;
    public int height;
    public float ratioX = 1;
    public float ratioY = 1;

    public PhysicMaterial physmat;

    public Material material;

    public List<int>[,] verticesAccess;
    public Triangle[] triangleAccess;

    public struct Triangle
    {
        public Vector2 coords;
        public int[] verticeIndexes;
        public Vector3[] verticeCoords;
    }

    void Start()
    {
        Generate();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * width, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * height, Color.blue);
    }

    public void Generate()
    {
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.material = physmat;
        collider.convex = false;
        Mesh mesh = filter.mesh;

        renderer.material = material;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        verticesAccess = new List<int>[width + 1, height + 1];
        triangleAccess = new Triangle[width * height * 2];

        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < height + 1; i++)
        {
            for (int j = 0; j < width + 1; j++)
            {
                verticesAccess[j, i] = new List<int>();
            }
        }

        int verticeCount = 0;
        int triangleCount = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3[] triangle1 = new Vector3[3]
                {
                    new Vector3(j, 0, (i + 1)),
                    new Vector3((j + 1), 0, i),
                    new Vector3(j, 0, i),
                };

                Vector3[] triangle2 = new Vector3[3]
                {
                    new Vector3((j + 1), 0, (i + 1)),
                    new Vector3((j + 1) , 0, i),
                    new Vector3(j, 0, (i +1)),
                };
                
                vertices.Add(triangle1[0]);
                vertices.Add(triangle1[1]);
                vertices.Add(triangle1[2]);
                vertices.Add(triangle2[0]);
                vertices.Add(triangle2[1]);
                vertices.Add(triangle2[2]);

                uv.Add(new Vector2(triangle1[0].x / width, triangle1[0].z / height));
                uv.Add(new Vector2(triangle1[1].x / width, triangle1[1].z / height));
                uv.Add(new Vector2(triangle1[2].x / width, triangle1[2].z / height));
                uv.Add(new Vector2(triangle2[0].x / width, triangle2[0].z / height));
                uv.Add(new Vector2(triangle2[1].x / width, triangle2[1].z / height));
                uv.Add(new Vector2(triangle2[2].x / width, triangle2[2].z / height));

                Triangle t1 = new Triangle();
                t1.coords = new Vector2(j, i);
                t1.verticeCoords = new Vector3[3]
                {
                    new Vector2(j, i + 1),
                    new Vector2(j + 1, i),
                    new Vector2(j, i),
                };

                Triangle t2 = new Triangle();
                t2.coords = new Vector2(j, i);
                t2.verticeCoords = new Vector3[3]
                {
                    new Vector2(j + 1, i + 1),
                    new Vector2(j + 1, i),
                    new Vector2(j, i + 1),
                };

                t1.verticeIndexes = new int[3];
                t2.verticeIndexes = new int[3];

                t1.verticeIndexes[0] = verticeCount;
                verticesAccess[(int)triangle1[0].x, (int)triangle1[0].z].Add(verticeCount);
                triangles.Add(verticeCount++);
                t1.verticeIndexes[1] = verticeCount;
                verticesAccess[(int)triangle1[1].x, (int)triangle1[1].z].Add(verticeCount);
                triangles.Add(verticeCount++);
                t1.verticeIndexes[2] = verticeCount;
                verticesAccess[(int)triangle1[2].x, (int)triangle1[2].z].Add(verticeCount);
                triangles.Add(verticeCount++);

                t2.verticeIndexes[0] = verticeCount;
                verticesAccess[(int)triangle2[0].x, (int)triangle2[0].z].Add(verticeCount);
                triangles.Add(verticeCount++);
                t2.verticeIndexes[1] = verticeCount;
                verticesAccess[(int)triangle2[1].x, (int)triangle2[1].z].Add(verticeCount);
                triangles.Add(verticeCount++);
                t2.verticeIndexes[2] = verticeCount;
                verticesAccess[(int)triangle2[2].x, (int)triangle2[2].z].Add(verticeCount);
                triangles.Add(verticeCount++);

                triangleAccess[triangleCount++] = t1;
                triangleAccess[triangleCount++] = t2;
            }
        }

        Vector3[] verticesAr = vertices.ToArray();
        int[] trianglesAr = triangles.ToArray();

        mesh.vertices = verticesAr;
        mesh.triangles = trianglesAr;
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        collider.sharedMesh = mesh;
        filter.mesh = mesh;
    }
}