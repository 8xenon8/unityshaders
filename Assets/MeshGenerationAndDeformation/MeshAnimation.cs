using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshAnimation : MonoBehaviour
{

    public float r;

    Mesh m;
    Mesh c;

    float[] speed;

    // Start is called before the first frame update
    void Awake()
    {
        m = GetComponent<MeshFilter>().mesh;
        c = gameObject.GetComponent<MeshCollider>().sharedMesh;

        speed = new float[m.vertices.Length];

        for (int i = 0; i < m.vertices.Length; i++)
        {
            speed[i] = Random.Range(0f, 2.5f);
        }
    }

    void FixedUpdate()
    {
        Vector3[] vs = m.vertices;
        Vector3[] cvs = c.vertices;

        float root = Mathf.Sqrt(vs.Length);

        //MeshGeneration g = GetComponent<MeshGeneration>();

        int w = 30;
        int h = 30;

        int counter = 0;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Vector3 offset = Vector3.zero;

        if (hit.collider && hit.collider.gameObject == gameObject)
        {
            GameObject target = hit.collider.gameObject;
            offset = target.transform.worldToLocalMatrix.MultiplyVector(hit.point - target.transform.position);

            // uncomment for 0.0...1.0 values
            offset.x /= (float)w;
            offset.y /= (float)h;
        }

        float r = 2f;

        for (int i = 0; i < vs.Length; i+=6)
        {
            //if (i % root != 0 && i % root != root - 1 && i <= vs.Length - root && i >= root)
            //{

            counter = i / 6;

            // uncomment for 0.0...1.0 values
            float x = (float)(counter % (w + 1)) / (w + 1);
            float y = (float)(counter / (w + 1)) / (w + 1);

            //float x = (float)(counter % (w + 1));
            //float y = (float)(counter / (w + 1));

            float diffX = Mathf.Abs(x - offset.x);
            float diffY = Mathf.Abs(y - offset.y);

            float d = Mathf.Sqrt(diffX * diffX + diffY * diffY);

            float offsetZ = 0f;

            if (d < r)
            {
                offsetZ = 1f - (float)d / (float)r;
            }

            //vs[i].y = cvs[i].y = Mathf.Sin(30 * Mathf.Sqrt(x * x + y * y) + Time.time) * 5f;
            //vs[i].z = vs[i + 1].z = vs[i + 2].z = vs[i + 3].z = vs[i + 4].z = vs[i + 5].z = Mathf.Sin(30 * Mathf.Sqrt(x * x + y * y) + Time.time) * 5f;
            //vs[i].z = cvs[i].z = vs[i + 1].z = cvs[i + 1].z = vs[i + 2].z = cvs[i + 2].z = vs[i + 3].z = cvs[i + 3].z = vs[i + 4].z = cvs[i + 4].z = vs[i + 5].z = cvs[i + 5].z = Random.value * 2;
            vs[i].z = cvs[i].z = vs[i + 1].z = cvs[i + 1].z = vs[i + 2].z = cvs[i + 2].z = vs[i + 3].z = cvs[i + 3].z = vs[i + 4].z = cvs[i + 4].z = vs[i + 5].z = cvs[i + 5].z = offsetZ;

            //cvs[i].y = 0f;

            //} else
            //{
            //    vs[i].y = 1f;
            //    cvs[i].y = 1f;
            //}
        }

        m.vertices = vs;
        //c.vertices = cvs;

        m.RecalculateNormals();
        //c.RecalculateNormals();

        //c = GetComponent<MeshCollider>().sharedMesh = c;
        if (Input.GetKeyDown("q"))
        {
            //    MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            //    Object m = AssetDatabase.LoadAssetAtPath<Object>("Assets/mesh");
            //    Debug.Log(m);
            //    //filter.mesh = m;
            PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/Resources/Mesh.prefab");
        }

        if (Input.GetKeyDown("e"))
        {
            //    MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            //    Object m = AssetDatabase.LoadAssetAtPath<Object>("Assets/mesh");
            //    Debug.Log(m);
            //    //filter.mesh = m;
            //o.SetActive(true);
            Instantiate(Resources.Load<GameObject>("Mesh"));
        }
    }
}
