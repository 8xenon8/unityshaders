using System.Collections;
using UnityEngine;
using Resizable;

namespace Rays
{
    public class RaySource : MonoBehaviour
    {
        public float maxRayDistance = 10000f;
        [Range(-10, 10)]
        public float power = 0.05f;

        private GameObject rayOrigin;
        private LineRenderer lineRenderer;

        const string RAY_ORIGIN_NAME = "RayOrigin";
        // Use this for initialization
        void Start()
        {
            rayOrigin = transform.Find(RAY_ORIGIN_NAME).gameObject;
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(rayOrigin.transform.position, transform.forward, out hit, maxRayDistance);

            lineRenderer.SetPosition(0, rayOrigin.transform.position);

            if (hit.collider) {
                lineRenderer.SetPosition(1, hit.point);

                ProcessHit(hit.collider.gameObject);
            } else {
                lineRenderer.SetPosition(1, rayOrigin.transform.position + transform.forward * maxRayDistance);
            }
        }

        private void ProcessHit(GameObject target)
        {
            IResizable resizable = target.GetComponent<IResizable>();

            if (resizable != null) {
                resizable.Resize(power * Time.deltaTime);
            }
        }
    }
}