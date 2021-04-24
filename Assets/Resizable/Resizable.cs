using System.Collections;
using UnityEngine;

namespace Resizable
{
    public class Resizable : MonoBehaviour, IResizable
    {
        public void Resize(float sizeDelta)
        {
            transform.localScale += Vector3.one * sizeDelta;

            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null) {
                rb.mass += sizeDelta * sizeDelta * sizeDelta;
            }
        }
    }
}