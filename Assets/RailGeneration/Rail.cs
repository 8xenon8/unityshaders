using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    public float railWidth = 1f;
    public float rotationSpacing = 0.05f;

    private struct Segment
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

    public List<Vector3> CalculateRouteToPoint(Vector3 point)
    {
        Segment lastSegment = GetLastSegment();
        Vector3 lastSegmentPosition = lastSegment.position;
        Vector3 right = Vector3.Cross(lastSegment.up, lastSegment.direction);

        Plane forwardUpPlane = new Plane(lastSegment.direction, lastSegment.up);
        Plane rightUpPlane = new Plane(right, lastSegment.up);
        Plane rightForwardPlane = new Plane(right, lastSegment.direction);
        Vector3 newPointDirection = forwardUpPlane.ClosestPointOnPlane(point).normalized;
        float directionSign = Mathf.Sign(Vector3.Dot(right, newPointDirection));

        List<Vector3> points = new List<Vector3>();
        points.Add(lastSegment.position);

        // new point is straight ahead
        if (newPointDirection.magnitude == 0)
        {
            points.Add(point);
            return points;
        }

        float rotationCircleRadius = railWidth / 2f + rotationSpacing;

        Vector3 rotationCenter = lastSegment.position +
            newPointDirection * rotationCircleRadius +
            lastSegment.direction.normalized * rotationCircleRadius;

        Vector3 hypotenuseVector = point - rotationCenter;
        float perpendicularToTangentAngle = Mathf.Acos(rotationCircleRadius / hypotenuseVector.magnitude) * Mathf.Rad2Deg * -directionSign;

        Vector3 rotatedVector = RotatePointAroundPivot(point, rotationCenter, new Vector3(0, perpendicularToTangentAngle, 0)) - rotationCenter;
        rotatedVector = rotatedVector.normalized * rotationCircleRadius;

        float resultAngle = Vector3.Angle(lastSegment.direction, hypotenuseVector - rotatedVector);
        if (Vector3.Dot(lastSegment.direction, rotatedVector) < 0) {
            resultAngle = 360f - resultAngle;
        }
        Debug.Log(resultAngle);

        points.Add(rotationCenter);
        points.Add(rotationCenter + rotatedVector);
        points.Add(point);

        return points;
    }

    private Segment GetLastSegment()
    {
        if (segmentLinkedList.Count == 0) {
            segmentLinkedList.AddLast(new Segment(
                Vector3.zero,
                transform.forward,
                transform.up
            ));
        }

        return segmentLinkedList.Last.Value;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
