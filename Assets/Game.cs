using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Player player;
    private CullingController cullingController = new CullingController();
    public MirrorTransitionController mirrorTransitionController = new MirrorTransitionController();
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
        
        cullingController.HandleMirrorTraverse();
        Camera.main.cullingMask = visibleLayers;
    }

    public static Game Current()
    {
        return current;
    }

    public void MirrorSwap(LayerMask layersToSwitch)
    {
        visibleLayers ^= layersToSwitch;

        cullingController.HandleMirrorTraverse();
    }
}
