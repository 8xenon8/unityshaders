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
        Vector3 viewportRotation = viewport.transform.rotation.eulerAngles;
        viewportRotation.y = 180 - viewportRotation.y;
        Vector3 reflectionCameraPosition = new Vector3(
            viewportRelativePosition.x,
            viewportRelativePosition.y * -1,
            viewportRelativePosition.z
        );

        source.ResetWorldToCameraMatrix();
        source.ResetProjectionMatrix();
        Vector3 scale = new Vector3(-1, 1, 1);
        source.projectionMatrix = source.projectionMatrix * Matrix4x4.Scale(scale);

        source.transform.position = planeObj.transform.localToWorldMatrix.MultiplyPoint(reflectionCameraPosition);
        source.transform.rotation = Quaternion.Euler(viewportRotation);

        //if (viewport.gameObject.GetComponent<CameraFollow>().flipHorizontal)
        //{
        //    //Quaternion tmpRot = viewport.transform.rotation;
        //    //Vector3 tmpPos = viewport.transform.position;

        //    //viewport.transform.rotation = source.transform.rotation;
        //    //viewport.transform.position = source.transform.position;

        //    //source.transform.rotation = tmpRot;
        //    //source.transform.position = tmpPos;
        //    viewport
        //}

        source.Render();


        // start render

        //RenderTexture texture = source.targetTexture;
        //texture.
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
