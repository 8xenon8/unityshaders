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

        Vector3 pos = transform.position;
        Quaternion rotation = Camera.main.transform.rotation;

        //Vector2 direction = new Vector2(transform.position Camera.main.transform);

        float angle = Camera.main.transform.rotation.eulerAngles.y;

        Vector3 direction;

        if (Input.GetKey("w"))
        {
            direction = Camera.main.transform.forward;
            direction.y = 0;
            direction = direction.normalized;
            ball.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
        }

        if (Input.GetKey("s"))
        {
            direction = Camera.main.transform.forward;
            direction.y = 0;
            direction = direction.normalized;
            ball.AddForce(direction * speed * Time.deltaTime * -1, ForceMode.Force);
        }

        if (Input.GetKey("d"))
        {
            direction = Camera.main.transform.right;
            direction.y = 0;
            direction = direction.normalized;
            ball.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
        }

        if (Input.GetKey("a"))
        {
            direction = Camera.main.transform.right;
            direction.y = 0;
            direction = direction.normalized;
            ball.AddForce(direction * speed * Time.deltaTime * -1, ForceMode.Force);
        }

        ball.velocity = new Vector3(
            ball.velocity.x * 0.95f,
            0,
            ball.velocity.z * 0.95f
        );
    }
}
