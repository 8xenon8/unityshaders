using System.Collections;
using UnityEngine;

namespace Resizable
{
    public class ResizablePlayer : MonoBehaviour, IResizable
    {
        private Camera camera;

        public float minScale = 0.1f;
        public float maxScale = 10f;
        public float currentScale = 1f;

        private Rigidbody rigidbody;

        private Vector3 initialScale;
        private float initialMass;

        void Start()
        {
            camera = GetComponent<Camera>();

            rigidbody = GetComponent<Rigidbody>();

            initialScale = Vector3.zero + transform.localScale;
            initialMass = rigidbody.mass;
        }

        public void Resize(float sizeDelta)
        {
            currentScale += sizeDelta;

            currentScale = Mathf.Max(Mathf.Min(currentScale, maxScale), minScale);

            transform.localScale = initialScale * currentScale;
            rigidbody.mass = initialMass * currentScale;
        }

        void Update()
        {
            if (Input.GetKey("r"))
            {
                float multiplier = 1f;
                Resize(Time.deltaTime * multiplier);
            }
            if (Input.GetKey("t"))
            {
                float multiplier = 1f;
                Resize(Time.deltaTime * multiplier * -1);
            }
        }
    }
}