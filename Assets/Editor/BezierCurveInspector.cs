using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
	private BezierCurve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private const int lineSteps = 100;

	private void OnSceneGUI()
	{
		curve = target as BezierCurve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		for (int pointIndex = 0; pointIndex < curve.points.Count; pointIndex++) {
			ShowPoint(pointIndex);
		}

		Handles.color = Color.white;

		float step = 0.01f;
		float iPrevious = 0f;
		//Vector3 lineStart = curve.GetPoint(0f);
		Vector3[] trackPointsPrevious = curve.GetTrackPoints(0f);
		Handles.color = Color.green;

		List<BezierCurve.Point> points = curve.GetPoints();
		BezierCurve.Point point = points[1];
		BezierCurve.Point previousPoint = points[0];

		//Handles.DrawLine(lineStart, lineStart + curve.GetVelocity(0f));
		for (int pointIndex = 1; pointIndex < points.Count; pointIndex++) {

			point = points[pointIndex];

			Handles.color = Color.white;
			Handles.DrawLine(previousPoint.position, point.position);
			Handles.color = Color.red;
			Vector3 right = Vector3.Cross(point.up, point.derivative);
			Vector3 previousRight = Vector3.Cross(previousPoint.up, previousPoint.derivative);
			Handles.DrawLine(previousPoint.position + previousRight, point.position + right);
			Handles.color = Color.yellow;
			Handles.DrawLine(previousPoint.position - previousRight, point.position - right);
			//Handles.DrawLine(point.position, point.position + point.);
			//Handles.DoRotationHandle(handleRotation, lineStart);
			Handles.color = Color.green;

			//Vector3[] trackPoints = curve.GetTrackPoints(i);

			//Vector3 directionPrevious = curve.GetVelocity(iPrevious);
			//Vector3 velocity = curve.GetVelocity(iCurrent);

			//Handles.DrawLine(
			//    curve.GetPoint(i),
			//    curve.GetPoint(i) + curve.GetNormal(i)
			//);

			//         Handles.DrawLine(
			//	trackPoints[0],
			//	trackPointsPrevious[0]
			//);
			//Handles.color = Color.yellow;
			//Handles.DrawLine(
			//	trackPoints[1],
			//	trackPointsPrevious[1]
			//);
			//Handles.color = Color.green;

			//trackPointsPrevious = trackPoints;
			//iPrevious = i;

			//step = 0.005f * Mathf.Pow(curve.GetVelocity(i).magnitude, 2);
			//step = Mathf.Min(0.02f, step);

			previousPoint = point;
		}
	}

	private Vector3 ShowPoint(int index)
	{
		BezierCurve.BuildPoint p = curve.points[index];
		Vector3 point = handleTransform.TransformPoint(p.point);
		Vector3 guide = handleTransform.TransformPoint(p.guide);

		Handles.color = Color.red;
		Handles.DrawLine(point, guide);

		EditorGUI.BeginChangeCheck();

		point = Handles.DoPositionHandle(point, handleRotation);
		guide = Handles.DoPositionHandle(guide, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
            p.point = handleTransform.InverseTransformPoint(point);
			p.guide = handleTransform.InverseTransformPoint(guide);

			curve.points[index] = p;
		}
		return point;
	}
}