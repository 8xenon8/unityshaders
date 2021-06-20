using UnityEngine;
using System.Collections;

public class GravityMirrorPlane : MirrorPlane
{
    private protected new void OnTriggerStay(Collider other)
    {
        if (canSwap && plane.GetSide(other.gameObject.transform.position) == true)
        {
            Game.Current().mirrorTransitionController.TransferPlayer(this);
            GravityHelper.SetGravity(plane.normal);
            canSwap = false;
        }
    }
}
