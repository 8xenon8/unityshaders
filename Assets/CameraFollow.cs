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

    public bool disabled = false;

    public Matrix4x4 m;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        m = gameObject.GetComponent<Camera>().projectionMatrix;
    }
    void Update()
    {
        gameObject.GetComponent<Camera>().projectionMatrix = m;

        if (disabled)
        {
            return;
        }

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
        transform.LookAt(player.transform.position);

        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 10;
        }
        else
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
        Physics.Raycast(player.transform.position, vec, out hit, zoom, Camera.main.cullingMask);

        if (hit.collider)
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = player.transform.position + vec * zoom;
        }
    }

    public void FlipCamera(ReflectionCamera reflectionCamera)
    {
        int i = Game.Current().isFlipped ? 1 : -1;
        Vector3 cameraToPlayer = reflectionCamera.transform.position - player.transform.position;
        angleX = (Game.Current().isFlipped ? Mathf.Atan2(cameraToPlayer.z, cameraToPlayer.x) : Mathf.Atan2(cameraToPlayer.x, cameraToPlayer.z)) * Mathf.Rad2Deg;
    }

    void OnPreCull()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(Game.Current().isFlipped ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }

    void OnPreRender()
    {
        GL.invertCulling = Game.Current().isFlipped;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
