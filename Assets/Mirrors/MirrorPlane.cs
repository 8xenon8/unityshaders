using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MirrorPlane : MonoBehaviour
{
    GameObject planeObj;
    Plane plane;
    Camera source;
    Camera viewport;

    private bool canSwap = true;

    RenderTexture viewTexture;

    public bool isDisabled;

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    void Awake()
    {
        GameObject cameraObj = new GameObject("camera_" + gameObject.GetHashCode().ToString());
        cameraObj.hideFlags = HideFlags.DontSave;
        cameraObj.AddComponent<ReflectionCamera>();
        source = cameraObj.AddComponent<Camera>();

        planeObj = transform.gameObject;

        Mesh m = GetComponent<MeshFilter>().mesh;
        plane = new Plane(
            m.vertices[0],
            m.vertices[1],
            m.vertices[2]
        );

        viewport = Camera.main;
        mirrors.Add(this);
    }

    public void Render()
    {
        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
        source.targetTexture = viewTexture;
        planeObj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", viewTexture);

        Vector3 viewportRelativePosition = planeObj.transform.worldToLocalMatrix.MultiplyPoint(viewport.transform.position);
        Vector3 reflectionCameraPosition = new Vector3(
            viewportRelativePosition.x,
            viewportRelativePosition.y * -1,
            viewportRelativePosition.z
        );

        source.transform.position = planeObj.transform.localToWorldMatrix.MultiplyPoint(reflectionCameraPosition);

        Vector3 mirrorsNormal = planeObj.transform.localRotation * new Vector3(0f, 1, 0f);
        Plane planeOfMirror = new Plane(mirrorsNormal, planeObj.transform.position);

        float intersectionDistance;

        Ray rayToMirror = new Ray(viewport.transform.position, viewport.transform.forward);
        if (planeOfMirror.Raycast(rayToMirror, out intersectionDistance))
        {
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);
            source.transform.LookAt(hitPoint);
        } else {
            rayToMirror = new Ray(viewport.transform.position, viewport.transform.forward * -1);
            planeOfMirror.Raycast(rayToMirror, out intersectionDistance);
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);

            Debug.DrawLine(hitPoint - Vector3.left, hitPoint + Vector3.left);
            Debug.DrawLine(hitPoint - Vector3.forward, hitPoint + Vector3.forward);

            source.transform.LookAt(hitPoint);
            Vector3 dir = source.transform.position - hitPoint;
            Debug.DrawLine(hitPoint, hitPoint + dir * 10, Color.green);
            source.transform.LookAt(source.transform.position + dir);
            //source.transform.rotation *= Quaternion.Euler(0, -1, 0);
            //source.transform.rotation = Quaternion. (source.transform.rotation);
        }

        source.Render();
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 p = gameObject.GetComponent<Collider>().ClosestPoint(other.gameObject.transform.position);

        Vector3 diff = other.gameObject.transform.position - p;

        Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

        //other.gameObject.GetComponent<Rigidbody>().AddForce(diff * (1 - diff.magnitude) * 300, ForceMode.Force);
        //otherRb.velocity *= diff.magnitude;
        //otherRb.AddForce(diff * (1 - diff.magnitude) * 10, ForceMode.Force);

        if (canSwap && diff.magnitude < 1f)
        {
            Swap(otherRb);
            canSwap = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSwap = true;
    }

    private void Swap(Rigidbody rb)
    {
        rb.velocity *= -1;
        Game.Current().MirrorSwap();
        Camera.main.gameObject.GetComponent<CameraFollow>().FlipCamera();
    }

    private void OnDestroy()
    {
        Destroy(source.gameObject);
        mirrors.Remove(this);
    }
}
