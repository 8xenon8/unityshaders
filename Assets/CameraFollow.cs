using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Player player;

    float angleX = 0f;
    float angleY = 0;

    float angleYMin = -0.9f;
    float angleYMax = 0.95f;

    float mouseSpeedX = 3f;
    float mouseSpeedY = 0.1f;

    float zoom = 3f;
    float zoomMin = 1.5f;
    float zoomMax = 10f;

    bool isLookingThroughMirror = false;

    public MirrorPlane currentMirror;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {

        zoom += Input.mouseScrollDelta.y;

        zoom = Mathf.Min(Mathf.Max(zoom, zoomMin), zoomMax);

        float offsetX = Input.GetAxis("Mouse X");
        float offsetY = Input.GetAxis("Mouse Y");

        angleX += offsetX * mouseSpeedX;

        if (angleX < 0)
        {
            angleX = 360f + angleX;
        }

        if (angleX >= 360)
        {
            angleX = (angleX % 360);
        }

        angleY += offsetY * mouseSpeedY;
        angleY = Mathf.Min(Mathf.Max(angleY, angleYMin), angleYMax);

        SetCameraPosition();

        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 10;
        } else
        {
            Camera.main.fieldOfView = 60;
        }
    }

    private void SetCameraPosition()
    {
        float angleXRad = angleX * Mathf.Deg2Rad;
        float x = Game.Current().isFlipped ? Mathf.Cos(angleXRad) : Mathf.Sin(angleXRad);
        float z = Game.Current().isFlipped ? Mathf.Sin(angleXRad) : Mathf.Cos(angleXRad);

        Vector3 vec = new Vector3(x, angleY, z);
        vec.Normalize();

        RaycastHit hit;
        MirrorPlane mirror;
        Vector3 lookAt = player.transform.position;
        bool isLookingThroughMirrorCurrentFrame = false;

        Physics.Raycast(player.transform.position, vec, out hit, zoom, Camera.main.cullingMask);
        
        if (hit.collider)
        {
            if (hit.collider.gameObject.TryGetComponent(out mirror))
            {
                isLookingThroughMirrorCurrentFrame = true;
                Vector3 playerToMirrorVector = hit.point - player.transform.position;
                transform.position = hit.point + Vector3.Reflect(vec, mirror.plane.normal) * (zoom - playerToMirrorVector.magnitude);
                lookAt = hit.point;
            }
            else
            {
                transform.position = hit.point;
            }
        } else
        {
            transform.position = player.transform.position + vec * zoom;
        }

        transform.LookAt(lookAt);


        if (isLookingThroughMirror != isLookingThroughMirrorCurrentFrame)
        {
            isLookingThroughMirror = isLookingThroughMirrorCurrentFrame;

            foreach (Camera camera in Camera.allCameras)
            {
                Vector3 scale = new Vector3(-1, 1, 1);
                camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
            }
        }
    }

    public void FlipCamera(MirrorPlane mirror)
    {
        Vector3 cameraToPlayer = (mirror.transform.position - player.transform.position) * 2;
        angleX = ((Game.Current().mirrorTransitionController.playerBehindMirror ^ IsLookingThroughTheMirror()) ? Mathf.Atan2(cameraToPlayer.z, cameraToPlayer.x) : Mathf.Atan2(cameraToPlayer.x, cameraToPlayer.z)) * Mathf.Rad2Deg;
    }

    void OnPreCull()
    {
        //Camera camera = gameObject.GetComponent<Camera>();
        //foreach (Camera camera in Camera.allCameras)
        //{
        //    camera.ResetWorldToCameraMatrix();
        //    camera.ResetProjectionMatrix();
        //    Vector3 scale = new Vector3(IsLookingThroughTheMirror() ? -1 : 1, 1, 1);
        //    camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
        //}
    }

    private void OnPreRender()
    {
        GL.invertCulling = IsLookingThroughTheMirror();
    }


    private void OnPostRender()
    {
        GL.invertCulling = false;
    }

    public bool IsLookingThroughTheMirror()
    {
        float angleXRad = angleX * Mathf.Deg2Rad;
        float x = Game.Current().isFlipped ? Mathf.Cos(angleXRad) : Mathf.Sin(angleXRad);
        float z = Game.Current().isFlipped ? Mathf.Sin(angleXRad) : Mathf.Cos(angleXRad);

        Vector3 vec = new Vector3(x, angleY, z);
        vec.Normalize();

        RaycastHit hit;
        MirrorPlane mirror;
        Vector3 lookAt = player.transform.position;

        Physics.Raycast(player.transform.position, vec, out hit, zoom, Camera.main.cullingMask);

        if (hit.collider && hit.collider.gameObject.TryGetComponent(out mirror))
        {
            return true;
        }

        return false;
    }
}
