using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody ball;

    public float speed = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        ball = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            ball.AddForce(Camera.main.transform.forward * speed * Time.deltaTime, ForceMode.Force);
        }

        if (Input.GetKey("s"))
        {
            ball.AddForce(Camera.main.transform.forward * speed * Time.deltaTime * -1, ForceMode.Force);
        }

        if (Input.GetKey("d"))
        {
            ball.AddForce(Camera.main.transform.right * speed * Time.deltaTime, ForceMode.Force);
        }

        if (Input.GetKey("a"))
        {
            ball.AddForce(Camera.main.transform.right * speed * Time.deltaTime * -1, ForceMode.Force);
        }

        ball.velocity = new Vector3(
            ball.velocity.x * 0.95f,
            ball.velocity.y,
            ball.velocity.z * 0.95f
        );
    }
}
