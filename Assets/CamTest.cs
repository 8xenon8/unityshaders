using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public Matrix4x4 m;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m = gameObject.GetComponent<Camera>().projectionMatrix;
    }


    private void OnPreRender()
    {
        if (Game.Current().mirrorTransitionController.playerBehindMirror)
        {
            //Matrix4x4 mx = gameObject.GetComponent<Camera>().projectionMatrix;
            //mx.m20 *= -1;
            //gameObject.GetComponent<Camera>().projectionMatrix = mx;
        }
    }
}
