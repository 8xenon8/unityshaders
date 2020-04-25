using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCamera : MainCamera
{
    public MirrorPlane mirror;
    public MainCamera originCamera;

    public void SetPositionAndRotationByOriginCameraRecursively(Transform t)
    {
        gameObject.transform.position = originCamera.transform.position + (mirror.plane.normal * -2) * mirror.plane.GetDistanceToPoint(originCamera.transform.position);

        float intersectionDistance;

        Ray rayToMirror = new Ray(originCamera.transform.position, originCamera.transform.forward);
        if (mirror.plane.Raycast(rayToMirror, out intersectionDistance))
        {
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);
            transform.LookAt(hitPoint);
        }
        else
        {
            rayToMirror = new Ray(originCamera.transform.position, originCamera.transform.forward * -1);
            mirror.plane.Raycast(rayToMirror, out intersectionDistance);
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);

            transform.LookAt(hitPoint);
            Vector3 dir = transform.position - hitPoint;
            transform.LookAt(transform.position + dir);
        }

        if (camerasToRender == null)
        {
            return;
        }

        foreach (Camera cam in camerasToRender)
        {
            ReflectionCamera rCam;
            if (cam.gameObject.TryGetComponent(out rCam))
            {
                rCam.SetPositionAndRotationByOriginCameraRecursively(gameObject.transform);
            }
        }

        gameObject.GetComponent<Camera>().Render();
    }

    private void OnDestroy()
    {
        foreach (Camera cam in camerasToRender)
        {
            Destroy(cam.gameObject);
            ReflectionCamera rCam;
            if (cam.gameObject.TryGetComponent(out rCam))
            {
                rCam.mirror.attachedCameras.Remove(gameObject.name);
            }
        }
        originCamera.camerasToRender.Remove(gameObject.GetComponent<Camera>());
    }
}
