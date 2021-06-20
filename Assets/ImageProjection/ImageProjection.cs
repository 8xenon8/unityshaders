using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProjection : MonoBehaviour
{
    public bool isSimulated;

    public Texture image;

    public int width;
    public int height;

    float[] randoms;

    void Start()
    {
        ProceduralMesh generator = gameObject.AddComponent<ProceduralMesh>();
        generator.width = width;
        generator.height = height;

        float ratio = (float)image.height / (float)image.width;

        generator.ratioX = ratio;

        generator.Generate();

        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = image;
        material.mainTextureScale = new Vector2(1, 2);

        GetComponent<MeshRenderer>().material = material;

        transform.position += new Vector3(width / -2f, height / -2f);

        randoms = new float[width * height * 2 + 1];

        for (int i = 1; i < randoms.Length; i++)
        {
            randoms[i] = 3 * i;
            //randoms[i] = Random.Range(50f, 100f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Player p = GameObject.Find("Player").GetComponent<Player>();

        int w = width + 1;
        int h = height + 1;

        Vector2 rectStart = new Vector2(0.25f, 0.25f);
        Vector2 rectEnd = new Vector2(0.75f, 0.75f);

        Vector3[,] points = new Vector3[w, h];

        Mesh m = gameObject.GetComponent<MeshFilter>().mesh;

        if (isSimulated)
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Vector2 currentPoint = new Vector2(rectStart.x + (rectEnd.x - rectStart.x) / (w - 1) * j, rectStart.y + (rectEnd.y - rectStart.y) / (h - 1) * i);

                    //currentPoint = new Vector2(Screen.width * currentPoint.x, Screen.height * currentPoint.y);

                    int[] triangles = GetAdjancentTriangles(i, j);

                    float magnitude = 30f;
                    //Ray r = Camera.main.ScreenPointToRay(currentPoint);

                    Vector3 p1 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[0]]);
                    Vector3 p2 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[1]]);
                    Vector3 p3 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[2]]);
                    Vector3 p4 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[3]]);
                    Vector3 p5 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[4]]);
                    Vector3 p6 = GetReflectionAtCameraPoint(currentPoint, magnitude + randoms[triangles[5]]);

                    Vector3[] posAr = new Vector3[6]
                    {
                        p1, p2, p3, p4, p5, p6
                    };
                    //GetComponent<MeshGeneration>().SetVertices(j, i, posAr);
                }
            }
        }
    }

    Vector3 GetReflectionAtCameraPoint(Vector2 currentPoint, float magnitude)
    {
        currentPoint = new Vector2(Screen.width * currentPoint.x, Screen.height * currentPoint.y);

        Ray r = Camera.main.ScreenPointToRay(currentPoint);

        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.gameObject == GameObject.Find("Player").gameObject)
            {
                return transform.worldToLocalMatrix.MultiplyPoint(hit.point + Vector3.Reflect(r.direction, hit.normal) * magnitude);
            }
        }

        return r.origin + r.direction * magnitude;
    }

    int[] GetAdjancentTriangles(int x, int y)
    {
        int t1 = width * (y - 1) * 2 + x * 2; // 1
        int t2 = width * (y - 1) * 2 + x * 2 + 1; // 2
        int t3 = width * (y - 1) * 2 + x * 2 + 2; // 3
        int t4 = width * y * 2 + x * 2 - 1; // 4
        int t5 = width * y * 2 + x * 2; // 5
        int t6 = width * y * 2 + x * 2 + 1; // 6

        List<int> triangleIndiced = new List<int>
        { 
            t1, t2, t3, t4, t5, t6
        };

        if (x == 0)
        {
            triangleIndiced[0] = 0;
            triangleIndiced[3] = 0;
            triangleIndiced[4] = 0;
        }

        if (y == 0)
        {
            triangleIndiced[0] = 0;
            triangleIndiced[1] = 0;
            triangleIndiced[2] = 0;
        }

        if (x == width)
        {
            triangleIndiced[1] = 0;
            triangleIndiced[2] = 0;
            triangleIndiced[5] = 0;
        }

        if (y == height)
        {
            triangleIndiced[3] = 0;
            triangleIndiced[4] = 0;
            triangleIndiced[5] = 0;
        }

        return triangleIndiced.ToArray();
    }
}
