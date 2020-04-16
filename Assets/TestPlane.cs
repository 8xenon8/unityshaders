using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlane : MonoBehaviour
{
    Camera source;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        source = GameObject.Find("MirrorCamera").GetComponent<Camera>();
        RenderTexture t = new RenderTexture(Screen.width, Screen.height, 0);
        source.targetTexture = t;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", t);
    }
}
