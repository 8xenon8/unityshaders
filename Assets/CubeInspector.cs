using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInspector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh m = gameObject.GetComponent<MeshFilter>().mesh;
        Debug.Log(m.vertices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
