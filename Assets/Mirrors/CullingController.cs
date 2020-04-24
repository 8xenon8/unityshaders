using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingController
{
    public void HandleMirrorTraverse()
    {
        for (int i = 0; i < 32; i++)
        {
            bool doCollide;

            int layerMask = (int)Mathf.Pow(2, i);
            int playerLayer = LayerMask.NameToLayer("Player");
            
            Physics.IgnoreLayerCollision(i, playerLayer, !((Game.Current().visibleLayers & layerMask) == layerMask));

        }

        //foreach (Camera cam in Camera.allCameras)
        //{
        //    cam.cullingMask = Game.Current().visibleLayers;
        //}
    }
}
