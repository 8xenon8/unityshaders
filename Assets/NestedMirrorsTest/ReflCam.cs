using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflCam : MonoBehaviour
{
    public GameObject target;

    public bool doCopy = false;

    public void CustomRender()
    {
        Camera cam = gameObject.GetComponent<Camera>();
        target.GetComponent<Renderer>().material.SetTexture("_MainTex", cam.targetTexture);
        cam.Render();

        if (doCopy)
        {
            GameObject.Find("Cam1").GetComponent<ReflCam>().CustomRender();
        }
    }
}
