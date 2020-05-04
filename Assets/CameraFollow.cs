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
    public void SetCameraByAngle()
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
        float x = Mathf.Sin(angleXRad);
        float z = Mathf.Cos(angleXRad);

        bool isLookingThroughMirrorCurrentFrame = false;

        Vector3 vec = new Vector3(x, angleY, z);
        vec.Normalize();

        if (player.BehindMirror())
        {
            return;
            Vector3 newPos = player.transform.position + vec * zoom;
            if (player.crossingMirror.plane.GetSide(newPos) == false)
            {
                transform.position = newPos + player.crossingMirror.plane.GetDistanceToPoint(newPos) * -2 * player.crossingMirror.plane.normal;
                isLookingThroughMirrorCurrentFrame = true;
            } else
            {
                transform.position = newPos;
            }
            return;
        }

        RaycastHit hit;
        MirrorPlane mirror;
        Vector3 lookAt = player.transform.position;

        Physics.Raycast(player.transform.position, vec, out hit, zoom, Camera.main.cullingMask);

        if (player.crossingMirror != null && player.crossingMirror.plane.GetSide(player.transform.position) == false)
        {
            if (player.crossingMirror.plane.GetSide(transform.position))
            {
                transform.position = player.transform.position + vec * zoom;
            } else
            {

            }
        }

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
        }
        //else if (player.crossingMirror && player.crossingMirror.plane.GetSide(player.transform.position) == false)
        //{
        //    Debug.Log("1");
        //    //Plane p = new Plane(player.crossingMirror.plane.normal, transform.position);
        //    Utility.DrawCross(Vector3.Reflect(vec, player.crossingMirror.plane.normal), Color.red);
        //    transform.position = Vector3.Reflect(vec, player.crossingMirror.plane.normal);
        //}
        else
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
        //Vector3 cameraToPlayer = (mirror.transform.position - player.transform.position) * 2;
        Vector3 playerToReflection = mirror.source.transform.position - player.gameObject.transform.position;
        angleX = Mathf.Atan2(playerToReflection.x, playerToReflection.z) * Mathf.Rad2Deg;
    }

    void OnPreCull()
    {
        SetCameraPosition();
    }

    public bool IsLookingThroughTheMirror()
    {
        return isLookingThroughMirror;

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
