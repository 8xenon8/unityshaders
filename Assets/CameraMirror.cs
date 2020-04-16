using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMirror : MonoBehaviour
{
    // Start is called before the first frame update
    void OnPreCull()
    {
        foreach (MirrorPlane mirror in MirrorPlane.mirrors)
        {
            mirror.Render();
        }
    }
}
