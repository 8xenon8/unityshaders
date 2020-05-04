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
        GL.invertCulling = Game.Current().player.cam.gameObject.GetComponent<CameraFollow>().IsLookingThroughTheMirror() ^ Game.Current().mirrorTransitionController.playerBehindMirror;
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
        GL.invertCulling = false;
    }
}
