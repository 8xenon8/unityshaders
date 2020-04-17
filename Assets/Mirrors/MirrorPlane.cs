using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MirrorPlane : MonoBehaviour
{
    GameObject plane;
    Camera source;
    Camera viewport;

    RenderTexture viewTexture;

    private const string cameraObjName = "MirrorCamera";
    private const string planeObjName = "MirrorPlane";

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    void Start()
    {
        source = transform.Find(cameraObjName).GetComponent<Camera>();
        plane = transform.Find(planeObjName).gameObject;
        viewport = Camera.main;
        mirrors.Add(this);
    }

    public void Render()
    {
        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
        // Render the view from the portal camera to the view texture
        source.targetTexture = viewTexture;
        // Display the view texture on the screen of the linked portal
        //plane.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        plane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", viewTexture);

        Vector3 viewportRelativePosition = plane.transform.worldToLocalMatrix.MultiplyPoint(viewport.transform.position);
        Vector3 viewportRotation = viewport.transform.rotation.eulerAngles;
        viewportRotation.y = 180 - viewportRotation.y;
        Vector3 reflectionCameraPosition = new Vector3(
            viewportRelativePosition.x,
            viewportRelativePosition.y * -1,
            viewportRelativePosition.z
        );
        source.transform.position = plane.transform.localToWorldMatrix.MultiplyPoint(reflectionCameraPosition);
        source.transform.rotation = Quaternion.Euler(viewportRotation);

        source.Render();


        // start render

        //RenderTexture texture = source.targetTexture;
        //texture.
    }
}
