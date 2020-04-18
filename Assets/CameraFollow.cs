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

    public bool flipHorizontal = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {

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
            angleX = 360f - angleX;
        }

        if (angleX >= 360)
        {
            angleX = (angleX % 360);
        }

        angleY += offsetY * mouseSpeedY;
        angleY = Mathf.Min(Mathf.Max(angleY, angleYMin), angleYMax);

        float angleXRad = angleX * Mathf.Deg2Rad;

        transform.position = player.transform.position;

        transform.position += new Vector3(
            Mathf.Sin(angleXRad),
            0,
            Mathf.Cos(angleXRad)
        ) * zoom;

        float d = Vector3.Distance(transform.position, player.transform.position);

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, d - Mathf.Sqrt(1 - angleY * angleY) * zoom);
        transform.position += Vector3.up * angleY * zoom * -1;

        transform.LookAt(player.transform.position);

        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 10;
        } else
        {
            Camera.main.fieldOfView = 60;
        }
    }

    void OnPreCull()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        GL.invertCulling = flipHorizontal;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
