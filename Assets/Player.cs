using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody ball;
    public Camera cam;

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

        Vector3 pos = transform.position;
        Vector3 direction = Vector3.zero;
        float angle = cam.transform.rotation.eulerAngles.y;

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

        direction.y = 0;
        direction.Normalize();

        Debug.DrawLine(transform.position, transform.position + cam.transform.forward * 5, Color.red);

        ball.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
    }
}
