using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainCamera : MonoBehaviour
{
    public List<Camera> camerasToRender;

    private bool isInverted          = false;
    private bool invertCullingGlobal = false;

    // Start is called before the first frame update
    void Start()
    {
        camerasToRender = new List<Camera>();
    }

    private void OnPreRender()
    {
        GL.invertCulling = isInverted;
    }

    private void OnPreCull()
    {
        foreach (MirrorPlane mirror in MirrorPlane.GetActiveMirrors())
        {
            mirror.Render();
        }
    }

    private void OnPostRender()
    {
        GL.invertCulling = invertCullingGlobal;
    }

    public void Invert()
    {
        Camera cam = GetComponent<Camera>();

        cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        isInverted            = !isInverted;

        foreach (MirrorPlane mirror in MirrorPlane.GetActiveMirrors())
        {
            Renderer r = mirror.GetComponent<Renderer>();

            r.material.SetInt("_Inverse", r.material.GetInt("_Inverse") == 0 ? 1 : 0);
        }
    }

    void Update()
    {
        //Camera.main.nearClipPlane = 0.001f;
    }
}
