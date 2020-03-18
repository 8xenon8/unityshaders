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

        for (int i = 0; i < vs.Length; i += 6)
        {
            //if (i % root != 0 && i % root != root - 1 && i <= vs.Length - root && i >= root)
            //{

            float amp = .5f;

            vs[i].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i].y = Mathf.Sin(Time.time * speed[i]) * amp;

            vs[i + 1].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i + 1].y = Mathf.Sin(Time.time * speed[i]) * amp;

            vs[i + 2].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i + 2].y = Mathf.Sin(Time.time * speed[i]) * amp;

            vs[i + 3].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i + 3].y = Mathf.Sin(Time.time * speed[i]) * amp;

            vs[i + 4].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i + 4].y = Mathf.Sin(Time.time * speed[i]) * amp;

            vs[i + 5].y = Mathf.Sin(Time.time * speed[i]) * amp;
            cvs[i + 5].y = Mathf.Sin(Time.time * speed[i]) * amp;

            //Debug.DrawLine(transform.position + vs[i], transform.position + vs[i] + m.normals[i]);
            //Debug.DrawLine(transform.position + vs[i + 1], transform.position + vs[i + 1] + m.normals[i + 1]);
            //Debug.DrawLine(transform.position + vs[i + 2], transform.position + vs[i + 2] + m.normals[i + 2]);
            //Debug.DrawLine(transform.position + vs[i + 3], transform.position + vs[i + 3] + m.normals[i + 3]);
            //Debug.DrawLine(transform.position + vs[i + 4], transform.position + vs[i + 4] + m.normals[i + 4]);
            //Debug.DrawLine(transform.position + vs[i + 5], transform.position + vs[i + 5] + m.normals[i + 5]);

            //cvs[i].y = 0f;

            //} else
            //{
            //    vs[i].y = 1f;
            //    cvs[i].y = 1f;
            //}
        }

        m.vertices = vs;
        c.vertices = cvs;

        m.RecalculateNormals();
        //c.RecalculateNormals();

        c = GetComponent<MeshCollider>().sharedMesh = c;
    }
}
