using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RailGeneration
{

    [ExecuteInEditMode]
    public class RouteCalculator : MonoBehaviour
    {
        public float railWidth = 1f;
        public float railRadius = 0.01f;
        public float rotationSpacing = 0.05f;

        public GameObject pointPrefab;

        public List<GameObject> points = new List<GameObject>();

        public struct Segment
        {
            public Vector3 position;
            public Vector3 direction;
            public Vector3 up;

            public Segment(Vector3 position, Vector3 direction, Vector3 up)
            {
                this.position = position;
                this.direction = direction;
                this.up = up;
            }
        }

        private LinkedList<Segment> segmentLinkedList = new LinkedList<Segment>();

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<Segment> CalculateRouteToPoint(Transform startFrom, Vector3 point)
        {
            List<Segment> segments = new List<Segment>();

            Vector3 lastSegmentPosition = startFrom.localPosition;
            Vector3 lastSegmentDirection = startFrom.parent.InverseTransformVector(startFrom.forward);
            Vector3 lastSegmentUp = startFrom.transform.parent.InverseTransformVector(startFrom.up);
            Vector3 rightLocal = Vector3.Cross(
                startFrom.up,
                lastSegmentDirection
            ).normalized;

            Plane horizontalPlaneLocal = new Plane(Vector3.up, lastSegmentPosition);
            Plane verticalPlaneLocal = new Plane(rightLocal, lastSegmentPosition);

            Plane forwardUpPlane = new Plane(rightLocal, lastSegmentPosition);
            Plane rightUpPlane = new Plane(lastSegmentDirection, lastSegmentPosition);
            Plane rightForwardPlane = new Plane(lastSegmentUp, lastSegmentPosition);
            Vector3 newPointPerpendicularVector = point - forwardUpPlane.ClosestPointOnPlane(point);
            Vector3 newPointPerpendicularDirection = newPointPerpendicularVector.normalized;
            float directionSign = Mathf.Sign(Vector3.Dot(rightLocal, newPointPerpendicularDirection));

            segments.Add(new Segment(lastSegmentPosition, lastSegmentDirection, lastSegmentUp));

            //rightTrack.AddRange(GetMeshVerticesForDirection(lastSegmentPosition - newPointPerpendicularDirection * railWidth / 2, lastSegmentDirection, lastSegmentUp));
            //leftTrack.AddRange(GetMeshVerticesForDirection(lastSegmentPosition + newPointPerpendicularDirection * railWidth / 2, lastSegmentDirection, lastSegmentUp));

            // new point is straight ahead
            if (newPointPerpendicularDirection.magnitude == 0)
            {
                //rightTrack.AddRange(
                //    GetMeshVerticesForDirection(point + rightLocal * railWidth / 2, lastSegmentDirection, Vector3.up)
                //);
                //leftTrack.AddRange(
                //    GetMeshVerticesForDirection(point - rightLocal * railWidth / 2, lastSegmentDirection, Vector3.up)
                //);

                segments.Add(new Segment(
                    point,
                    lastSegmentDirection,
                    Vector3.up
                ));

                return segments;
            }

            float rotationCircleRadius = railWidth / 2f + rotationSpacing;

            Vector3 rotationCenter = lastSegmentPosition +
                newPointPerpendicularDirection * rotationCircleRadius +
                lastSegmentDirection.normalized * rotationCircleRadius;

            Vector3 hypotenuseVector = Vector3.ProjectOnPlane(point - rotationCenter, horizontalPlaneLocal.normal);

            if (hypotenuseVector.magnitude < rotationCircleRadius)
            {
                rotationCircleRadius = hypotenuseVector.magnitude;

                //return new List<Segment>();
                rotationCenter = lastSegmentPosition +
                newPointPerpendicularDirection * rotationCircleRadius +
                lastSegmentDirection.normalized * rotationCircleRadius;
            }

            float perpendicularToTangentAngle = Mathf.Acos(rotationCircleRadius / hypotenuseVector.magnitude) * Mathf.Rad2Deg * -directionSign;

            Vector3 projectedPoint = horizontalPlaneLocal.ClosestPointOnPlane(point);

            Vector3 rotatedVector = RotatePointAroundPivot(projectedPoint, rotationCenter, new Vector3(0, perpendicularToTangentAngle, 0)) - rotationCenter;
            rotatedVector = rotatedVector.normalized * rotationCircleRadius;

            float resultAngle = Vector3.Angle(lastSegmentDirection, hypotenuseVector - rotatedVector);
            if (Vector3.Dot(lastSegmentDirection, rotatedVector) < 0)
            {
                resultAngle = 360f - resultAngle;
            }

            Vector3 initialSegmentPosition = rotationCenter - newPointPerpendicularDirection * rotationCircleRadius;
            Vector3 currentSegmentPosition = initialSegmentPosition;

            int segmentCount = (int)Mathf.Ceil(resultAngle / 5f);

            float anglePerSegment = resultAngle / segmentCount;
            Vector3 perpendicularVectorToRailAtCurrentPoint;

            for (int i = 0; i < segmentCount; i++)
            {
                currentSegmentPosition = RotatePointAroundPivot(initialSegmentPosition, rotationCenter, new Vector3(0, anglePerSegment * directionSign * i, 0));
                perpendicularVectorToRailAtCurrentPoint = (currentSegmentPosition - rotationCenter).normalized * railWidth / 2f;

                segments.Add(new Segment(
                    currentSegmentPosition,
                    Vector3.Cross(perpendicularVectorToRailAtCurrentPoint, Vector3.up) * Mathf.Sign(Vector3.Dot(newPointPerpendicularDirection, rightLocal)) * -1,
                    Vector3.up
                ));
            }

            Vector3 finalRight = Vector3.Cross(Vector3.up, point - currentSegmentPosition).normalized * -directionSign;

            segments.Add(new Segment(
                point,
                point - currentSegmentPosition,
                Vector3.up
            ));

            return segments;
        }

        public GameObject GetLastSegment()
        {
            if (transform.childCount == 0) {
                Instantiate(pointPrefab, transform);
            }

            return transform.GetChild(transform.childCount - 1).gameObject;
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        public Vector3 GetCurrentDirection()
        {
            return segmentLinkedList.Count > 0 ? segmentLinkedList.Last.Value.direction : transform.forward;
        }

        private Vector3[] GetMeshVerticesForDirection(Vector3 position, Vector3 direction, Vector3 up)
        {
            Vector3[] vertices = new Vector3[6];

            Vector3 right = Vector3.Cross(up, direction).normalized * railRadius;

            up = up.normalized * railRadius;

            vertices[0] = position + up * Mathf.Sin(Mathf.PI / 6) + right * Mathf.Cos(Mathf.PI / 6);
            vertices[1] = position + up * Mathf.Sin(Mathf.PI / 2) + right * Mathf.Cos(Mathf.PI / 2);
            vertices[2] = position + up * Mathf.Sin(Mathf.PI * 5 / 6) + right * Mathf.Cos(Mathf.PI * 5 / 6);
            vertices[3] = position + up * Mathf.Sin(Mathf.PI * 7 / 6) + right * Mathf.Cos(Mathf.PI * 7 / 6);
            vertices[4] = position + up * Mathf.Sin(Mathf.PI * 3 / 2) + right * Mathf.Cos(Mathf.PI * 3 / 2);
            vertices[5] = position + up * Mathf.Sin(Mathf.PI * 11 / 6) + right * Mathf.Cos(Mathf.PI * 11 / 6);

            return vertices;
        }

        public void Reset()
        {
            segmentLinkedList = new LinkedList<Segment>();

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            points = new List<GameObject>();

            GameObject p1 = Instantiate(pointPrefab, transform);
            GameObject p2 = Instantiate(pointPrefab, transform);

            p1.transform.localPosition = Vector3.zero;
            p2.transform.localPosition = Vector3.forward;

            points.Add(p1);
            points.Add(p2);
        }

        public void AddSegment()
        {
            GameObject p = Instantiate(pointPrefab, transform);

            Transform lastPointTransform = points[points.Count - 1].transform;

            p.transform.localPosition = lastPointTransform.localPosition + lastPointTransform.forward * 5;

            points.Add(p);
        }
    }
}
