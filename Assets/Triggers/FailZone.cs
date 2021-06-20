using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailZone : MonoBehaviour
{
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        if (collider == null) {
            throw new UnityException($"Object FailZone cannot load it's dependency collider");
            Application.Quit(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
