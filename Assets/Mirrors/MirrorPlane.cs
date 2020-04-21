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

    public LayerMask layersToSwitch;

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    void Awake()
    {
        GameObject cameraObj = new GameObject("camera_" + gameObject.GetHashCode().ToString());
        cameraObj.hideFlags = HideFlags.DontSave;
        cameraObj.AddComponent<ReflectionCamera>();
        source = cameraObj.AddComponent<Camera>();
        source.cullingMask ^= layersToSwitch;

        planeObj = transform.gameObject;

        Vector3 mirrorsNormal = planeObj.transform.localRotation * new Vector3(0f, 1, 0f);
        plane = new Plane(mirrorsNormal, planeObj.transform.position);

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

        float intersectionDistance;

        Ray rayToMirror = new Ray(viewport.transform.position, viewport.transform.forward);
        if (plane.Raycast(rayToMirror, out intersectionDistance))
        {
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);
            source.transform.LookAt(hitPoint);
        } else {
            rayToMirror = new Ray(viewport.transform.position, viewport.transform.forward * -1);
            plane.Raycast(rayToMirror, out intersectionDistance);
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);

            source.transform.LookAt(hitPoint);
            Vector3 dir = source.transform.position - hitPoint;
            source.transform.LookAt(source.transform.position + dir);
        }

        source.Render();
    }

    private void OnTriggerStay(Collider other)
    {
        if (canSwap && plane.GetSide(other.gameObject.transform.position) == false)
        {
            Swap(other.gameObject.GetComponent<Rigidbody>());
            canSwap = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSwap = true;
    }

    private void Swap(Rigidbody rb)
    {
        rb.velocity = Vector3.Reflect(rb.velocity.normalized, plane.normal) * rb.velocity.magnitude;
        Game.Current().MirrorSwap();
        Camera.main.gameObject.GetComponent<CameraFollow>().FlipCamera();
        Camera.main.cullingMask ^= layersToSwitch;
        source.cullingMask ^= layersToSwitch;

        for (int i = 0; i < 32; i++)
        {
            bool doCollide;

            int layerMask = (int)Mathf.Pow(2, i);
            int playerLayer = LayerMask.NameToLayer("Player");

            if ((layersToSwitch & layerMask) == layerMask)
            {
                doCollide = Physics.GetIgnoreLayerCollision(i, playerLayer);
                Physics.IgnoreLayerCollision(i, playerLayer, !doCollide);
            }

        }
    }

    private void OnDestroy()
    {
        Destroy(source.gameObject);
        mirrors.Remove(this);
    }
}
