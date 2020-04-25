using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MirrorPlane : MonoBehaviour
{
    Camera source;
    Camera viewport;

    public Plane plane;

    private bool canSwap = true;

    RenderTexture viewTexture;

    public bool isDisabled;

    public LayerMask layersToSwitch;

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    public Dictionary<string, ReflectionCamera> attachedCameras;

    void Start()
    {
        attachedCameras = new Dictionary<string, ReflectionCamera>();
        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", viewTexture);

        Vector3 mirrorsNormal = gameObject.transform.localRotation * new Vector3(0f, 1, 0f);
        plane = new Plane(mirrorsNormal, gameObject.transform.position);
    }

    public void Update()
    {

    }

    ReflectionCamera GetCam(MainCamera cam)
    {
        if (attachedCameras.ContainsKey(cam.gameObject.name))
        {
            return attachedCameras[cam.gameObject.name];
        } else
        {
            GameObject newCamObj = new GameObject();
            newCamObj.SetActive(false);
            newCamObj.hideFlags = HideFlags.DontSave;
            newCamObj.name = "Camera_" + gameObject.name + "_" + newCamObj.GetHashCode().ToString();
            Camera newCam = newCamObj.AddComponent<Camera>();
            newCam.cullingMask = cam.gameObject.GetComponent<Camera>().cullingMask ^ layersToSwitch;
            ReflectionCamera newRCam = newCamObj.AddComponent<ReflectionCamera>();

            newCam.projectionMatrix = cam.gameObject.GetComponent<Camera>().projectionMatrix;
            newCam.targetTexture = viewTexture;

            newRCam.depth = cam.depth + 1;
            newRCam.mirror = this;
            newRCam.originCamera = cam;
            cam.camerasToRender.Add(newCam);
            attachedCameras[Camera.current.gameObject.name] = newRCam;

            newRCam.SetPositionAndRotationByOriginCameraRecursively(cam.transform);
            newCamObj.SetActive(true);

            return newRCam;
        }
    }

    private void OnWillRenderObject()
    {
        MainCamera rCam;

        if (Camera.current.gameObject.name == "Scene Camera") { return; }

        if (!plane.GetSide(Camera.current.transform.position)) { return; }

        if (Camera.current.TryGetComponent(out rCam))
        {
            if (rCam.depth >= 5) { return; }

            ReflectionCamera newrCam = GetCam(rCam);
        }
    }

    private void OnBecameInvisible()
    {
        foreach (KeyValuePair<string, ReflectionCamera> cam in attachedCameras)
        {
            Destroy(cam.Value.gameObject);
        }
        attachedCameras.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (canSwap && plane.GetSide(other.gameObject.transform.position) == false)
        {
            Swap(other.gameObject.GetComponent<Rigidbody>());
            canSwap = false;
        } else
        {
            canSwap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSwap = true;
    }

    private void Swap(Rigidbody rb)
    {
        rb.velocity = Vector3.Reflect(rb.velocity.normalized, plane.normal) * rb.velocity.magnitude;
        Game.Current().MirrorSwap(layersToSwitch);
        Camera.main.gameObject.GetComponent<CameraFollow>().FlipCamera(attachedCameras[Camera.main.gameObject.name]);
        Camera.main.cullingMask ^= layersToSwitch;

        foreach (KeyValuePair<string, ReflectionCamera> keyvalue in attachedCameras)
        {
            keyvalue.Value.gameObject.GetComponent<Camera>().cullingMask ^= layersToSwitch;
        }
    }

    private void OnDestroy()
    {
        //Destroy(source.gameObject);
        mirrors.Remove(this);
    }
}
