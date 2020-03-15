using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAnimation : MonoBehaviour
{

    Mesh m;
    Mesh c;

    float[] speed;

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

        float root = Mathf.Sqrt(vs.Length);

        for (int i = 0; i < vs.Length; i++)
        {
            if (i % root != 0 && i % root != root - 1 && i <= vs.Length - root && i >= root)
            {

                vs[i].y = Mathf.Sin(Time.time * speed[i]) * 0.25f;
                cvs[i].y = Mathf.Sin(Time.time * speed[i]) * 0.25f;
                //cvs[i].y = 0f;

            } else
            {
                vs[i].y = 1f;
                cvs[i].y = 1f;
            }
        }

        m.vertices = vs;
        c.vertices = cvs;

        m.RecalculateNormals();
        //c.RecalculateNormals();

        c = GetComponent<MeshCollider>().sharedMesh = c;
    }
}
