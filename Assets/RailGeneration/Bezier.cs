using UnityEngine;

public static class Bezier
{
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
        float oneMinusT = 1f - t;

		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3 * oneMinusT * oneMinusT * t * p1 +
			3 * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float oneMinusT = 1f - t;

		return 3 * oneMinusT * oneMinusT * (p1 - p0) +
			6 * oneMinusT * t * (p2 - p1) +
			(t * t + t * t + t * t) * (p3 - p2);
	}

	public static Vector3 GetSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		return 6 * (1f - t) * (p2 - 2 * p1 + p0) +
			6 * t * (p3 - 2 * p2 + p1);
	}

	public static float EstimateCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float controlNetLength = (p0 - p1).magnitude + (p1 - p2).magnitude + (p2 - p3).magnitude;
		float estimatedCurveLength = (p0 - p3).magnitude + controlNetLength / 2f;
		return estimatedCurveLength;
	}

	public static int GetPointCount(BezierCurve.BuildPoint p1, BezierCurve.BuildPoint p2)
    {
		return Mathf.FloorToInt(EstimateCurveLength(
			p1.point,
			p1.guide,
			p2.guide,
			p2.point
		));
    }
}