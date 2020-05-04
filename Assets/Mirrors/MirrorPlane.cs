using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class MirrorPlane : MonoBehaviour
{
    public Camera source;

    public Plane plane;

    private bool canSwap = true;

    RenderTexture viewTexture;

    public LayerMask layersToSwitch;

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    void Start()
    {
        source = transform.Find("Cam").gameObject.GetComponent<Camera>();
        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", viewTexture);
        source.targetTexture = viewTexture;

        Vector3 mirrorsNormal = gameObject.transform.localRotation * new Vector3(0f, 1, 0f);
        plane = new Plane(mirrorsNormal, gameObject.transform.position);
    }

    public void Update()
    {
        Debug.DrawLine(transform.position, transform.position + plane.normal, Color.red);
    }

    public void Render()
    {
        GL.invertCulling = Game.Current().player.cam.gameObject.GetComponent<CameraFollow>().IsLookingThroughTheMirror() ^ Game.Current().mirrorTransitionController.playerBehindMirror;
        SetReflectionCamPositionAndRotation(Camera.main);
        SetNearClipPlane();
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        source.Render();
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.gameObject.TryGetComponent(out player))
        {
            player.crossingMirror = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.gameObject.TryGetComponent(out player))
        {
            player.crossingMirror = null;
        }
        canSwap = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canSwap && plane.GetSide(other.gameObject.transform.position) == false)
        {
            Game.Current().mirrorTransitionController.TransferPlayer(this);
            canSwap = false;
        }
    }

    void SetNearClipPlane()
    {
        Transform clipPlane = transform;
        Camera playerCam = Game.Current().player.cam;

        Vector3 camSpacePos = source.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = source.worldToCameraMatrix.MultiplyVector(plane.normal);

        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, plane.GetDistanceToPoint(source.transform.position));

        source.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }

    public void SetReflectionCamPositionAndRotation(Camera originCamera)
    {
        source.transform.position = originCamera.transform.position + (plane.normal * -2) * plane.GetDistanceToPoint(originCamera.transform.position);

        float intersectionDistance;

        Ray rayToMirror = new Ray(originCamera.transform.position, originCamera.transform.forward);
        if (plane.Raycast(rayToMirror, out intersectionDistance))
        {
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);
            source.transform.LookAt(hitPoint);
        }
        else
        {
            rayToMirror = new Ray(originCamera.transform.position, originCamera.transform.forward * -1);
            plane.Raycast(rayToMirror, out intersectionDistance);
            Vector3 hitPoint = rayToMirror.GetPoint(intersectionDistance);

            source.transform.LookAt(hitPoint);
            Vector3 dir = source.transform.position - hitPoint;
            source.transform.LookAt(source.transform.position + dir);
        }
    }

    public static List<MirrorPlane> GetActiveMirrors()
    {
        List<MirrorPlane> mirrors = FindObjectsOfType<MirrorPlane>().ToList();
        return mirrors;
    }
}
