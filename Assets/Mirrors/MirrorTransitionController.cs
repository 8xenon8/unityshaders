using UnityEngine;

public class MirrorTransitionController
{
    public bool playerBehindMirror = false;

    public void TransferPlayer(MirrorPlane mirror)
    {

        Game.Current().player.GetComponent<Rigidbody>().velocity = Vector3.Reflect(Game.Current().player.GetComponent<Rigidbody>().velocity, mirror.plane.normal);

        //if (Game.Current().player.cam.GetComponent<CameraFollow>().IsLookingThroughTheMirror() == false)
        //{
            Game.Current().player.cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        //}

        foreach (Camera cam in Camera.allCameras)
        {
            cam.cullingMask ^= mirror.layersToSwitch;
            if (cam != Game.Current().player.cam)
            {
                cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
            }
        }

        Game.Current().MirrorSwap(mirror.layersToSwitch);

        //Game.Current().player.cam.gameObject.GetComponent<CameraFollow>().FlipCamera(mirror);

        playerBehindMirror = !playerBehindMirror;

        DimensionHelper.right *= -1;

        //DimensionHelper.SetDimensions(mirror.transform.forward * -1, Vector3.Reflect(DimensionHelper.forward, mirror.plane.normal));

        //Game.Current().player.cam.GetComponent<CameraFollow>().FlipCamera(mirror);
    }
}