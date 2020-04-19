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
        Vector3 direction = Vector3.zero;
        float angle = Camera.main.transform.rotation.eulerAngles.y;

        int doFlip = Game.Current().isFlipped ? -1 : 1;

        if (Input.GetKey("w"))
        {
            direction += Camera.main.transform.forward;
        }

        if (Input.GetKey("s"))
        {
            direction += Camera.main.transform.forward * -1;
        }

        if (Input.GetKey("d"))
        {
            direction += Camera.main.transform.right * doFlip;
        }

        if (Input.GetKey("a"))
        {
            direction += Camera.main.transform.right * -1 * doFlip;
        }

        direction.y = 0;
        direction.Normalize();

        ball.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
    }
}
