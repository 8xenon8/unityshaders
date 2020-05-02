using UnityEngine;

public class MirrorTransitionController
{
    public bool playerBehindMirror = false;

    public void TransferPlayer(MirrorPlane mirror)
    {

        Game.Current().player.GetComponent<Rigidbody>().velocity = Vector3.Reflect(Game.Current().player.GetComponent<Rigidbody>().velocity, mirror.plane.normal);

        //foreach (Camera cam in Camera.allCameras)
        //{
        Game.Current().player.cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        //}

        //Game.Current().player.cam.gameObject.GetComponent<CameraFollow>().FlipCamera(mirror);

        playerBehindMirror = !playerBehindMirror;
    }
}