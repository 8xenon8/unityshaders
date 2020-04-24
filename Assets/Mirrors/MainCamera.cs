using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public List<Camera> camerasToRender;
    public int depth;

    // Start is called before the first frame update
    void Start()
    {
        camerasToRender = new List<Camera>();
    }

    private void OnPreRender()
    {
        GL.invertCulling = Game.Current().isFlipped;
    }

    private void OnPreCull()
    {
        if (gameObject.GetComponent<Camera>() == Camera.main)
        {
            foreach (Camera cam in camerasToRender)
            {
                ReflectionCamera rCam;
                if (cam.gameObject.TryGetComponent(out rCam))
                {
                    rCam.SetPositionAndRotationByOriginCameraRecursively(gameObject.transform);
                    rCam.gameObject.GetComponent<Camera>().Render();
                }
            }
        }
    }

    private void OnPostRender()
    {
        GL.invertCulling = false;
    }

    void ClearCameras()
    {
        foreach (Camera cam in camerasToRender)
        {
            cam.gameObject.GetComponent<ReflectionCamera>().ClearCameras();
            Destroy(cam.gameObject);
        }
        camerasToRender.Clear();
    }
}
