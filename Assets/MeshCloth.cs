using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCloth : MonoBehaviour
{

    Mesh m;
    Mesh c;

    public float t;

    float[] speed;
    Vector3[] offsetMap;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshFilter>().mesh;
        c = GetComponent<MeshCollider>().sharedMesh;

        speed = new float[m.vertices.Length];

        for (int i = 0; i < m.vertices.Length; i++)
        {
            speed[i] = Random.Range(0f, 2.5f);
        }
    }

    void Update()
    {
        Vector3[] vs = m.vertices;
        Vector3[] cvs = c.vertices;

        offsetMap = new Vector3[vs.Length];

        float root = Mathf.Sqrt(vs.Length);

        for (int i = 0; i < vs.Length; i++)
        {
            if (i % root != 0 && i % root != root - 1 && i <= vs.Length - root && i >= root)
            {
                Vector3[] neighboorVertices = new Vector3[4];
                neighboorVertices[0] = vs[i - 1];
                neighboorVertices[1] = vs[i + 1];
                neighboorVertices[2] = vs[i - (int)root];
                neighboorVertices[3] = vs[i + (int)root];

                //Vector3 offset = Physics.gravity;
                Vector3 offset = Vector3.down;

                foreach (Vector3 v in neighboorVertices)
                {
                    float d = Vector3.Distance(vs[i], v);

                    float tension = d * d * t;

                    offset += (v - vs[i]) * tension;
                }

                offsetMap[i] = offset / 5f;

            }
            else
            {
                vs[i].y = 1f;
                cvs[i].y = 1f;
                offsetMap[i] = Vector3.zero;
            }
        }

        for (int i = 0; i < offsetMap.Length; i++)
        {
            vs[i] += offsetMap[i] * Time.deltaTime * 10;
        }

        m.vertices = vs;
        c.vertices = cvs;

        m.RecalculateNormals();
        c.RecalculateNormals();

        c = GetComponent<MeshCollider>().sharedMesh = c;
    }
}
