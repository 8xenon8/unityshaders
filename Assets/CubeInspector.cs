using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInspector : MonoBehaviour
{
    Mesh m;
    // Start is called before the first frame update
    void Start()
    {
        m = gameObject.GetComponent<MeshFilter>().mesh;
        Debug.Log(m.vertices);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m.normals.Length; i++)
        {
            Debug.DrawLine(transform.position + m.vertices[i], transform.position + m.normals[i]);
        }
    }
}
