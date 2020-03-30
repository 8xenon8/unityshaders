using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProjection : MonoBehaviour
{
    public Texture image;

    void Start()
    {
        int width = 2;
        int height = 2;

        MeshGeneration generator = gameObject.AddComponent<MeshGeneration>();
        generator.width = width;
        generator.height = height;

        float ratio = (float)image.height / (float)image.width;

        generator.ratio = ratio;

        generator.Generate();

        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = image;
        material.mainTextureScale = new Vector2(1, 2);

        GetComponent<MeshRenderer>().material = material;

        transform.position += new Vector3(width / -2f, height / -2f);
    }

    // Update is called once per frame
    void Update1()
    {
        Player p = GameObject.Find("Player").GetComponent<Player>();

        int h = 3;
        int w = 3;

        Vector2 rectStart = new Vector2(0.1f, 0.1f);
        Vector2 rectEnd = new Vector2(0.9f, 0.9f);

        Vector3[,] points = new Vector3[w, h];

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                Vector2 currentPoint = new Vector2(rectStart.x + (rectEnd.x - rectStart.x) / (w - 1) * j, rectStart.y + (rectEnd.y - rectStart.y) / (h - 1) * i);

                currentPoint = new Vector2(Screen.width * currentPoint.x, Screen.height * currentPoint.y);

                Ray r = Camera.main.ScreenPointToRay(currentPoint);

                RaycastHit hit;
                if (Physics.Raycast(r, out hit))
                {
                    if (hit.collider.gameObject == GameObject.Find("Player").gameObject)
                    {
                        float magnitude = 15f + Mathf.PerlinNoise(currentPoint.x, currentPoint.y) * 15;
                        //float magnitude = 10f;
                        Vector3 endpoint = (hit.point + Vector3.Reflect(r.direction, hit.normal) * magnitude);
                        points[j, i] = endpoint;
                        //Debug.DrawLine(hit.point, endpoint);
                        //Debug.DrawLine(hit.point, r.origin);
                    }
                }
            }
        }

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(points[j,i]);

                GetComponent<MeshGeneration>().SetVertices(j, i, pos);
            }
        }
    }

    void ArrangeForSphere(Vector3 sphereCenter)
    {

    }
}
