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

        public List<Vector3>[] CalculateRouteToPoint(Vector3 point)
        {
            Segment lastSegment = GetLastSegment();
            Vector3 lastSegmentPosition = lastSegment.position;
            Vector3 rightLocal = Vector3.Cross(
                lastSegment.up,
                lastSegment.direction
            ).normalized;

            Plane horizontalPlaneLocal = new Plane(Vector3.up, lastSegment.position);
            Plane verticalPlaneLocal = new Plane(rightLocal, lastSegment.position);

            Plane forwardUpPlane = new Plane(rightLocal, lastSegment.position);
            Plane rightUpPlane = new Plane(lastSegment.direction, lastSegment.position);
            Plane rightForwardPlane = new Plane(lastSegment.up, lastSegment.position);
            Vector3 newPointPerpendicularVector = point - forwardUpPlane.ClosestPointOnPlane(point);
            Vector3 newPointPerpendicularDirection = newPointPerpendicularVector.normalized;
            float directionSign = Mathf.Sign(Vector3.Dot(rightLocal, newPointPerpendicularDirection));

            List<Vector3> rightTrack = new List<Vector3>();
            List<Vector3> leftTrack = new List<Vector3>();

            rightTrack.AddRange(GetMeshVerticesForDirection(lastSegment.position - newPointPerpendicularDirection * railWidth / 2, lastSegment.direction, lastSegment.up));
            leftTrack.AddRange(GetMeshVerticesForDirection(lastSegment.position + newPointPerpendicularDirection * railWidth / 2, lastSegment.direction, lastSegment.up));

            // new point is straight ahead
            if (newPointPerpendicularDirection.magnitude == 0)
            {
                rightTrack.AddRange(
                    GetMeshVerticesForDirection(point + Vector3.Cross(point - lastSegmentPosition, Vector3.up) * railWidth / 2, lastSegment.direction, Vector3.up)
                );
                leftTrack.AddRange(
                    GetMeshVerticesForDirection(point - Vector3.Cross(point - lastSegmentPosition, Vector3.up) * railWidth / 2, lastSegment.direction, Vector3.up)
                );
                return new List<Vector3>[] { rightTrack, leftTrack };
            }

            float rotationCircleRadius = railWidth / 2f + rotationSpacing;

            Vector3 rotationCenter = lastSegment.position +
                newPointPerpendicularDirection * rotationCircleRadius +
                lastSegment.direction.normalized * rotationCircleRadius;

            Vector3 hypotenuseVector = Vector3.ProjectOnPlane(point - rotationCenter, horizontalPlaneLocal.normal);

            if (hypotenuseVector.magnitude < rotationCircleRadius)
            {
                return new List<Vector3>[0];
            }

            float perpendicularToTangentAngle = Mathf.Acos(rotationCircleRadius / hypotenuseVector.magnitude) * Mathf.Rad2Deg * -directionSign;

            Vector3 projectedPoint = horizontalPlaneLocal.ClosestPointOnPlane(point);

            Vector3 rotatedVector = RotatePointAroundPivot(projectedPoint, rotationCenter, new Vector3(0, perpendicularToTangentAngle, 0)) - rotationCenter;
            rotatedVector = rotatedVector.normalized * rotationCircleRadius;

            float resultAngle = Vector3.Angle(lastSegment.direction, hypotenuseVector - rotatedVector);
            if (Vector3.Dot(lastSegment.direction, rotatedVector) < 0)
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

                rightTrack.AddRange(
                    GetMeshVerticesForDirection(
                        currentSegmentPosition + perpendicularVectorToRailAtCurrentPoint,
                        Vector3.Cross(perpendicularVectorToRailAtCurrentPoint, Vector3.up) * Mathf.Sign(Vector3.Dot(perpendicularVectorToRailAtCurrentPoint, rightLocal)),
                        Vector3.up
                    )
                );
                leftTrack.AddRange(
                    GetMeshVerticesForDirection(
                        currentSegmentPosition - perpendicularVectorToRailAtCurrentPoint,
                        Vector3.Cross(perpendicularVectorToRailAtCurrentPoint, Vector3.up) * Mathf.Sign(Vector3.Dot(perpendicularVectorToRailAtCurrentPoint, rightLocal)),
                        Vector3.up
                    )
                );

                //points.Add(currentSegmentPosition + perpendicularVectorToRailAtCurrentPoint);
                //points.Add(currentSegmentPosition - perpendicularVectorToRailAtCurrentPoint);
            }

            Vector3 finalRight = Vector3.Cross(Vector3.up, point - currentSegmentPosition).normalized * -directionSign;

            rightTrack.AddRange(
                    GetMeshVerticesForDirection(
                        point + finalRight * railWidth / 2,
                        point - currentSegmentPosition,
                        Vector3.up
                    )
                );
            leftTrack.AddRange(
                GetMeshVerticesForDirection(
                        point - finalRight * railWidth / 2,
                        point - currentSegmentPosition,
                        Vector3.up
                )
            );

            return new List<Vector3>[] { rightTrack, leftTrack };
        }

        public Segment GetLastSegment()
        {
            if (segmentLinkedList.Count == 0)
            {
                segmentLinkedList.AddLast(new Segment(
                    Vector3.forward,
                    Vector3.forward,
                    Vector3.up
                ));
            }

            return segmentLinkedList.Last.Value;
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        public Vector3 GetCurrentDirection()
        {
            return segmentLinkedList.Count > 0 ? segmentLinkedList.Last.Value.direction : transform.forward;
        }

        public void CreateSegment(Vector3 point)
        {
            List<Vector3>[] points = CalculateRouteToPoint(point);
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
        }
    }
}
