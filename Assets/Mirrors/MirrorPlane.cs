using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.Diagnostics;

public class MirrorPlane : MonoBehaviour
{
    public float thickness = 1.0f;

    public Camera source;

    public Plane plane;

    private protected bool canSwap = true;

    RenderTexture viewTexture;

    public LayerMask layersToSwitch;

    public static List<MirrorPlane> mirrors = new List<MirrorPlane>();

    void Start()
    {
        source = transform.Find("Cam").gameObject.GetComponent<Camera>();
        source.projectionMatrix = Camera.main.projectionMatrix;
        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", viewTexture);
        source.targetTexture = viewTexture;
        gameObject.GetComponent<Renderer>().material.SetInt("_Inverse", 1);

        Vector3 mirrorsNormal = gameObject.transform.localRotation * new Vector3(0f, 1, 0f);
        plane = new Plane(mirrorsNormal, gameObject.transform.position + transform.up * thickness * 0.5f);
    }

    public void Update()
    {
        Vector3 p = plane.ClosestPointOnPlane(transform.position);
        Debug.DrawLine(p, p + plane.normal, Color.red, 0, false);
        //Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red, 0, false);
    }

    public void Render()
    {
        //GL.invertCulling = Game.Current().player.cam.gameObject.GetComponent<CameraFollow>().IsLookingThroughTheMirror() ^ Game.Current().mirrorTransitionController.playerBehindMirror;
        SetReflectionCamPositionAndRotation(Camera.main);
        SetNearClipPlane();
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        source.cullingMask = Camera.main.cullingMask ^ layersToSwitch;
        source.Render();
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.crossingMirror = this;
            player.mirrorSide = plane.GetSide(other.gameObject.transform.position);
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

    private protected void OnTriggerStay(Collider other)
    {
        Vector3 nextFramePlayerPosition = other.transform.position + other.gameObject.GetComponent<Rigidbody>().velocity * Time.deltaTime;
        Utility.DrawCross(nextFramePlayerPosition, Color.green);

        if (plane.GetSide(Game.Current().player.transform.position) != Game.Current().player.mirrorSide)
        {
            throw new System.Exception("Other side");
        }

        if (canSwap && plane.GetSide(nextFramePlayerPosition) != Game.Current().player.mirrorSide)
        {
            Game.Current().mirrorTransitionController.TransferPlayer(this);
        }
    }

    void SetNearClipPlane()
    {
        source.ResetProjectionMatrix();

        Vector3   normal               = source.worldToCameraMatrix.MultiplyVector(plane.normal);
        float     dot                  = -Vector3.Dot(source.worldToCameraMatrix.MultiplyPoint(transform.position + transform.up * 0.5f), normal);
        Vector4   clipPlaneCameraSpace = new Vector4(normal.x, normal.y, normal.z, dot);

        if (Mathf.Abs(dot) > 0.001f)
        {
            source.projectionMatrix = source.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
    }

    public void SetReflectionCamPositionAndRotation(Camera originCamera)
    {
        source.transform.position = originCamera.transform.position + (plane.normal * -2) * plane.GetDistanceToPoint(originCamera.transform.position);
        source.transform.rotation = Quaternion.LookRotation(Vector3.Reflect(originCamera.transform.forward, plane.normal), Vector3.Reflect(originCamera.transform.up, plane.normal));
    }

    public static List<MirrorPlane> GetActiveMirrors()
    {
        List<MirrorPlane> mirrors = FindObjectsOfType<MirrorPlane>().ToList();
        return mirrors;
    }
}
