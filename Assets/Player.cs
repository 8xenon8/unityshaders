using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody ball;
    public Camera cam;

    public Vector3 forward;

    public float speed = 1f;

    public MirrorPlane crossingMirror;
    public bool mirrorSide;

    private Resizable.ResizablePlayer resizablePlayer;

    // Start is called before the first frame update
    void Awake()
    {
        ball = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        resizablePlayer = GetComponent<Resizable.ResizablePlayer>();
    }

    public bool BehindMirror()
    {
        return crossingMirror && crossingMirror.plane.GetSide(transform.position) == false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow camFollow = cam.GetComponent<CameraFollow>();

        Vector3 forward = cam.transform.forward;
        Vector3 right   = cam.transform.right;

        if (camFollow.isLookingThroughMirror) {
            forward = Vector3.Reflect(forward, camFollow.currentMirror.plane.normal);
            right = Vector3.Reflect(right, camFollow.currentMirror.plane.normal) * -1;
        };

        camFollow.SetCameraByAngle();

        Vector3 direction = Vector3.zero;

        int doFlip = Game.Current().mirrorTransitionController.playerBehindMirror ? -1 : 1;

        if (Input.GetKey("w"))
        {
            direction += forward;
        }

        if (Input.GetKey("s"))
        {
            direction += forward * -1;
        }

        if (Input.GetKey("d"))
        {
            direction += right * doFlip;
        }

        if (Input.GetKey("a"))
        {
            direction += right * -1 * doFlip;
        }

        if (direction != Vector3.zero)
        {
            direction = Vector3.ProjectOnPlane(direction, DimensionHelper.up).normalized;
            ball.AddForce(direction * speed * Time.deltaTime * resizablePlayer.currentScale, ForceMode.Force);
            forward = direction;
        }

        Debug.DrawLine(transform.position, transform.position + direction * speed * Time.deltaTime * 10, Color.yellow);
    }
}
