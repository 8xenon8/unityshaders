using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviour
{
    Camera cam1;
    Camera cam2;
    Camera cam3;

    GameObject q1;
    GameObject q2;
    GameObject q3;

    public List<Camera> cameras;

    // Start is called before the first frame update
    void Start()
    {
        cam1 = GameObject.Find("Cam1").GetComponent<Camera>();
        cam2 = GameObject.Find("Cam2").GetComponent<Camera>();
        cam3 = GameObject.Find("Cam3").GetComponent<Camera>();

        q1 = GameObject.Find("Q1");
        q2 = GameObject.Find("Q2");
        q3 = GameObject.Find("Q3");

        cam1.targetTexture = new RenderTexture(Screen.width, Screen.height, 0);
        cam2.targetTexture = new RenderTexture(Screen.width, Screen.height, 0);
        cam3.targetTexture = new RenderTexture(Screen.width, Screen.height, 0);

        q1.GetComponent<Renderer>().material.SetTexture("_MainTex", cam2.targetTexture);
        q2.GetComponent<Renderer>().material.SetTexture("_MainTex", cam3.targetTexture);
        //q3.GetComponent<Renderer>().material.SetTexture("_MainTex", cam3.targetTexture);

        cameras = new List<Camera>();
    }

    // Update is called once per frame
    void OnPreCull()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            ReflCam c;
            if (cam.gameObject.TryGetComponent(out c))
            {
                c.CustomRender();
            }
        }
    }
}
