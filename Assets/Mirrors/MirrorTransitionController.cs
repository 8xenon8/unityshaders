using UnityEngine;

public class MirrorTransitionController
{
    public bool playerBehindMirror = false;

    public void TransferPlayer(MirrorPlane mirror)
    {

        Game.Current().player.GetComponent<Rigidbody>().velocity = Vector3.Reflect(Game.Current().player.GetComponent<Rigidbody>().velocity, mirror.plane.normal);

        Game.Current().player.cam.GetComponent<MainCamera>().Invert();

        Game.Current().MirrorSwap(mirror.layersToSwitch);

        playerBehindMirror = !playerBehindMirror;

        DimensionHelper.right *= -1;

        //Debug.Log(Camera.main.transform.position - mirror.source.transform);

        Game.Current().player.cam.GetComponent<CameraFollow>().FlipCamera(mirror);

        //if (playerBehindMirror)
        //{
        //    Game.Current().player.cam.GetComponent<CameraFollow>().FlipCamera(mirror);
        //    DimensionHelper.right *= -1;
        //} else
        //{
        //    DimensionHelper.right *= -1;
        //    Game.Current().player.cam.GetComponent<CameraFollow>().FlipCamera(mirror);
        //}
    }
}