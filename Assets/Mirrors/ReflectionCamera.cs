using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCamera : MonoBehaviour
{
    private void OnPreCull()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(Game.Current().isFlipped ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }

    void OnPreRender()
    {
        GL.invertCulling = Game.Current().isFlipped;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
