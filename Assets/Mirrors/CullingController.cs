using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingController
{
    public void HandleMirrorTraverse()
    {
        for (int i = 0; i < 32; i++)
        {
            int layerMask = (int)Mathf.Pow(2, i);
            int playerLayer = LayerMask.NameToLayer("Player");

            bool doCollide = ((Game.Current().visibleLayers & layerMask) == layerMask);

            Physics.IgnoreLayerCollision(i, playerLayer, !doCollide);
        }
    }
}
