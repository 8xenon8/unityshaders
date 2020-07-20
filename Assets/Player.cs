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

    // Start is called before the first frame update
    void Awake()
    {
        ball = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public bool BehindMirror()
    {
        return crossingMirror && crossingMirror.plane.GetSide(transform.position) == false;
    }

    // Update is called once per frame
    void Update()
    {

        cam.GetComponent<CameraFollow>().SetCameraByAngle();

        Vector3 direction = Vector3.zero;

        int doFlip = Game.Current().mirrorTransitionController.playerBehindMirror ? -1 : 1;

        if (Input.GetKey("w"))
        {
            direction += cam.transform.forward;
        }

        if (Input.GetKey("s"))
        {
            direction += cam.transform.forward * -1;
        }

        if (Input.GetKey("d"))
        {
            direction += cam.transform.right * doFlip;
        }

        if (Input.GetKey("a"))
        {
            direction += cam.transform.right * -1 * doFlip;
        }

        if (direction != Vector3.zero)
        {
            direction = Vector3.ProjectOnPlane(direction, DimensionHelper.up).normalized;
            ball.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
            forward = direction;
        }

        Debug.DrawLine(transform.position, transform.position + direction * speed * Time.deltaTime * 10, Color.yellow);
    }
}
