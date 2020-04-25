using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Player player;
    private CullingController cullingController;
    private static Game current;
    public bool isFlipped = false;
    public LayerMask visibleLayers;

    void Start()
    {
        if (current)
        {
            Destroy(this);
        }

        current = this;

        player = GameObject.Find("Player").GetComponent<Player>();

        cullingController = new CullingController();
        cullingController.HandleMirrorTraverse();
        Camera.main.cullingMask = visibleLayers;
        //foreach (Camera cam in Camera.main.gameObject.GetComponent<MainCamera>().camerasToRender)
        //{
        //    cam.cullingMask =
        //}
    }

    public static Game Current()
    {
        return current;
    }

    public void MirrorSwap(LayerMask layersToSwitch)
    {
        isFlipped = !isFlipped;
        visibleLayers ^= layersToSwitch;
        foreach (Camera cam in Camera.allCameras)
        {
            cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }

        cullingController.HandleMirrorTraverse();
    }
}
