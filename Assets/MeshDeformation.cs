using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                Debug.Log(localPosition);
            }
        }
    }
}
