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

        MeshGeneration g = GetComponent<MeshGeneration>();

        int w = g.width;
        int h = g.height;

        int counter = 0;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Vector3 offset = Vector3.zero;

        if (hit.collider)
        {
            GameObject target = hit.collider.gameObject;
            offset = target.transform.worldToLocalMatrix.MultiplyVector(hit.point - target.transform.position);

            // uncomment for 0.0...1.0 values
            //offset.x /= (float)w;
            //offset.y /= (float)h;
        }

        Debug.Log(offset);

        float r = 2f;

        for (int i = 0; i < vs.Length; i += 6)
        {
            //if (i % root != 0 && i % root != root - 1 && i <= vs.Length - root && i >= root)
            //{

            float amp = .5f;

            counter = i / 6;

            // uncomment for 0.0...1.0 values
            //float x = (float)(counter % (w + 1)) / (w + 1);
            //float y = (float)(counter / (w + 1)) / (w + 1);

            float x = (float)(counter % (w + 1));
            float y = (float)(counter / (w + 1));

            float diffX = Mathf.Abs(x - offset.x);
            float diffY = Mathf.Abs(y - offset.y);

            float d = Mathf.Sqrt(diffX * diffX + diffY * diffY);

            float offsetZ = 0f;

            if (d < r)
            {
                offsetZ = 1f - (float)d / (float)r;
            }

            //vs[i].y = cvs[i].y = vs[i + 1].y = cvs[i + 1].y = vs[i + 2].y = cvs[i + 2].y = vs[i + 3].y = cvs[i + 3].y = vs[i + 4].y = cvs[i + 4].y = vs[i + 5].y = cvs[i + 5].y = Mathf.Sin(30 * Mathf.Sqrt(x * x + y * y) + Time.time) * 5f;
            vs[i].z = cvs[i].z = vs[i + 1].z = cvs[i + 1].z = vs[i + 2].z = cvs[i + 2].z = vs[i + 3].z = cvs[i + 3].z = vs[i + 4].z = cvs[i + 4].z = vs[i + 5].z = cvs[i + 5].z =  offsetZ * offsetZ * 3;

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
