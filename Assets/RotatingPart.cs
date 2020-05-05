using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate()
    {
        // @TODO: make layerIsActive check
        if ((gameObject.layer ^ Game.Current().visibleLayers) == Mathf.Abs(Game.Current().visibleLayers - gameObject.layer))
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}
