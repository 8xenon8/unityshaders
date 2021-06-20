using UnityEngine;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour
{
	public bool isClosed = false;
	public float detalization = 1.0f;

	public struct BuildPoint
    {
		public Vector3 point;
		public Vector3 guide;
		public float rotation;
		public float width;

		public BuildPoint(Vector3 p, Vector3 g, float r, float w)
		{
			point = p;
			guide = g;
			rotation = r;
			width = w;
		}
    }

	public struct Point
    {
		public Vector3 position;
		public Vector3 derivative;
		public Vector3 up;

		public Point(Vector3 position, Vector3 derivative, Vector3 up)
        {
			this.position = position;
			this.derivative = derivative;
			this.up = up;
        }
    }

	public List<BuildPoint> points = new List<BuildPoint>() {
			new BuildPoint(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), 0f, 0.3f),
			new BuildPoint(new Vector3(2f, 0f, 0f), new Vector3(2f, 1f, 0f), 0f, 0.3f),
            new BuildPoint(new Vector3(3f, 0f, 0f), new Vector3(3f, 1f, 0f), 0f, 0.3f),
        };

    public List<Point> GetPoints()
    {
		BuildPoint p1, p2;

		Vector3 up = transform.up;

		List<Point> resultPoints = new List<Point>();

		if (points.Count < 2) {
			throw new UnityException("Not enough points to make a curve");
        }
		
		for (int i = 0; i < points.Count; i++) {
			p1 = points[i];

			if (points.Count == i + 1 && isClosed) {
				p2 = points[0];
			} else if (points.Count == i + 1) {
				break;
			} else {
				p2 = points[i + 1];
            }

			int steps = Mathf.FloorToInt(Bezier.EstimateCurveLength(
				p1.point,
				p1.guide,
				p2.guide + (p2.point - p2.guide) * 2,
				p2.point
			) * detalization);

			for (int step = 0; step < steps; step++) {
				float t = (float)step / (float)steps;

				Vector3 position = Bezier.GetPoint(p1.point, p1.guide, p2.guide + (p2.point - p2.guide) * 2, p2.point, t);
				position += transform.position;

				Vector3 derivative = Bezier.GetFirstDerivative(p1.point, p1.guide, p2.guide + (p2.point - p2.guide) * 2, p2.point, t).normalized;

				Vector3 newRight = Vector3.Cross(derivative, Vector3.up).normalized;

				//if (newRight == Vector3.zero) {
				//	up = Vector3.Cross(Vector3.Cross(derivative, Vector3.up), derivative).normalized;
				//} else {
					Vector3 newUp = Vector3.Cross(newRight, derivative);
					if (Vector3.Dot(up, newUp) < 0) {
                        newUp *= -1;
                    }
                    up = newUp;
                //}

				Point p = new Point(position, derivative, up);

				resultPoints.Add(p);
            }
        } 

		return resultPoints;
    }

    public void Reset()
	{
		points = new List<BuildPoint>() {
			new BuildPoint(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), 0f, 0.3f),
			new BuildPoint(new Vector3(2f, 0f, 0f), new Vector3(2f, 1f, 0f), 0f, 0.3f),
		};
	}

	public Vector3 GetPoint(float t)
	{
		BuildPoint[] closestPoints = GetClosestPoints(t);

        return transform.TransformPoint(
			Bezier.GetPoint(
				closestPoints[0].point,
				closestPoints[0].guide,
				closestPoints[1].guide + (closestPoints[1].point - closestPoints[1].guide) * 2,
				closestPoints[1].point,
				(points.Count - 1) * t % 1
			)
		);
	}

	public Vector3 GetVelocity(float t)
    {
		BuildPoint[] closestPoints = GetClosestPoints(t);

		return transform.TransformPoint(
            Bezier.GetFirstDerivative(
				closestPoints[0].point,
				closestPoints[0].guide,
				closestPoints[1].guide + (closestPoints[1].point - closestPoints[1].guide) * 2,
				closestPoints[1].point,
                (points.Count - 1) * t % 1
            )
        ) - transform.position;
    }

	public Vector3[] GetTrackPoints(float t)
    {
		Vector3 point = GetPoint(t);
		Vector3 velocity = GetVelocity(t);
        BuildPoint[] closestPoints = GetClosestPoints(t);

		BuildPoint p1 = closestPoints[0];
		BuildPoint p2 = closestPoints[1];

		float tLocal = (t * (points.Count - 1)) % 1;

		float width0 = p1.width;
		float width1 = p2.width;

		float widthLocal = (1 - tLocal) * width0 + tLocal * width1;

		Vector3 right = Vector3.Cross(Vector3.up, velocity).normalized * widthLocal;

        return new Vector3[] {
			point + right,
			point - right
		};
    }

	private BuildPoint[] GetClosestPoints(float t)
	{
		return new BuildPoint[] {
			points[(int)Mathf.Floor((points.Count - 1) * t)],
			points[(int)Mathf.Ceil((points.Count - 1) * t)]
		};
    }

	public Vector3 GetNormal(float t)
    {
		BuildPoint[] closestPoints = GetClosestPoints(t);

		Vector3 tangent = Bezier.GetFirstDerivative(closestPoints[0].point, closestPoints[0].guide, closestPoints[1].guide, closestPoints[1].point, t);
		Vector3 nextTangent = Bezier.GetSecondDerivative(closestPoints[0].point, closestPoints[0].guide, closestPoints[1].guide, closestPoints[1].point, t);
		Vector3 c = Vector3.Cross(nextTangent, tangent);

		Vector3 normal = Vector3.Cross(c, tangent).normalized;

        if (Vector3.Dot(normal, tangent) < 0)
        {
            normal *= -1;
        }

        return normal;
	}
}