using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReticleBrightness
{
    public bool can_Illuminate = false;
    [Space(5)]
    public KeyCode brightness_increase = KeyCode.UpArrow;
    public KeyCode brightness_decrease = KeyCode.DownArrow;
    [Space(5)]
    public float v_brightness_increase = 1;
    public float v_brightness_max = 25;
    public float v_brightness_min = 0;
    public float v_brightness = 1;
}

[System.Serializable]
public class Zooming
{
    public enum ZoomType { None, Variable, Toggle }

    public ZoomType zoomType;

    public KeyCode ActivatorKey = KeyCode.LeftAlt;

    [Header("Variable")]

    public bool zoomReticle = false;

    public float startFOV = 60;

    public float zoomSpeed = 5;

    public float currentSize = 1;

    public float sizeIncrements = 1;
    public float minSize = 1;
    public float maxSize = 2;

    public float currentZoom = 1;

    public float zoomIncrements = 1;
    public int minZoom = 1;
    public int maxZoom = 6;

    [Header("Toggle")]

    public bool isToggled = false;

    public KeyCode ToggleKey = KeyCode.Mouse1;

    public float ToggledZoomFOV = 15;
    public float UntoggledZoomFOV = 60;

    public bool t_zoomReticle = false;

    public float ToggledReticleSize = 2;
    public float unToggledReticleSize = 1;

}

[System.Serializable]
public class CameraFPS
{
    public enum FPSLimitType { None, Half, Quarter ,Fixed }

    public FPSLimitType fPSLimitType;

    public float fixedFps = 60;
}

[System.Serializable]
public class CameraDisable
{
    public enum cameraDisable { None, Distance }

    public cameraDisable o_cameraDisable;

    public float distance;

    public bool canZoomCamera;
}

public class RealisticScopeEffectV2 : MonoBehaviour
{

    public Camera ScopeCamera;

    private Material scopeMaterial;

    public ReticleBrightness reticleBrightness;

    public Zooming zooming;
    public CameraFPS cameraFPS;
    public CameraDisable cameraDisable;

    private float targetZoom = 1;
    private float targetReticleSize = 1;

    private float fpsTimer = 0;
    private float fpsTime = 0;

    private void Start()
    {
        scopeMaterial = GetComponent<MeshRenderer>().material;
        
        if (reticleBrightness.can_Illuminate)
            scopeMaterial.SetFloat("Reticle_Brightness", 1);

        switch (zooming.zoomType)
        {
            case Zooming.ZoomType.None:
                break;
            case Zooming.ZoomType.Variable:

                ScopeCamera.fieldOfView = zooming.startFOV;

                targetReticleSize = zooming.minSize;

                zooming.currentSize = targetReticleSize;

                if (zooming.zoomReticle)
                {
                    scopeMaterial.SetFloat("Reticle_Size", zooming.minSize);
                }
                

                break;
            case Zooming.ZoomType.Toggle:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (reticleBrightness.can_Illuminate)
        {
            changeReticleIllumnation();
        }
        switch (zooming.zoomType)
        {
            case Zooming.ZoomType.None:
                break;
            case Zooming.ZoomType.Variable:

                if (zooming.zoomReticle)
                {
                    zooming.sizeIncrements = ((zooming.maxSize - zooming.minSize) / zooming.maxZoom) * zooming.zoomIncrements;
                }

                if (Input.GetKey(zooming.ActivatorKey))
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0f && targetZoom < zooming.maxZoom)
                    {
                        increaseZoom();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") < 0f && targetZoom > zooming.minZoom)
                    {
                        decreaseZoom();
                    }
                }

                if (zooming.zoomReticle)
                {
                    targetReticleSize = zooming.minSize + zooming.sizeIncrements * targetZoom;
                }

                zooming.currentSize = Mathf.Lerp(zooming.currentSize, targetReticleSize, zooming.zoomSpeed * Time.deltaTime);

                zooming.currentZoom = Mathf.Lerp(zooming.currentZoom, targetZoom, zooming.zoomSpeed * Time.deltaTime);

                if (ScopeCamera != null)
                    ScopeCamera.fieldOfView = zooming.startFOV / zooming.currentZoom;

                if (zooming.zoomReticle)
                {
                    scopeMaterial.SetFloat("Reticle_Size", zooming.currentSize);
                }

                break;
            case Zooming.ZoomType.Toggle:

                if(Input.GetKey(zooming.ActivatorKey))
                {
                    if(Input.GetKeyDown(zooming.ToggleKey))
                    {
                        zooming.isToggled = !zooming.isToggled;
                    }
                }

                if(zooming.isToggled)
                {
                    if (ScopeCamera != null)
                        ScopeCamera.fieldOfView = zooming.ToggledZoomFOV;
                    scopeMaterial.SetFloat("Reticle_Size", zooming.ToggledReticleSize);
                }
                if (!zooming.isToggled)
                {
                    if (ScopeCamera != null)
                        ScopeCamera.fieldOfView = zooming.UntoggledZoomFOV;
                    scopeMaterial.SetFloat("Reticle_Size", zooming.unToggledReticleSize);
                }


                break;
            default:
                break;
        }

        float currentFPS = 1 / Time.deltaTime;
        float _cFPS = 0;

        switch (cameraFPS.fPSLimitType)
        {
            case CameraFPS.FPSLimitType.None:

                if (!ScopeCamera.enabled && ScopeCamera != null)
                    ScopeCamera.enabled = true;

                break;
            case CameraFPS.FPSLimitType.Half:

                if (ScopeCamera.enabled && ScopeCamera != null)
                    ScopeCamera.enabled = false;

                if (cameraDisable.canZoomCamera)
                {
                    _cFPS = currentFPS / 2;

                    fpsTime = 1 / _cFPS;

                    if (fpsTimer < fpsTime)
                        fpsTimer += Time.deltaTime;

                    if (fpsTimer > fpsTime)
                    {
                        fpsTimer = 0;
                        ScopeCamera.Render();
                    }
                }

                break;
            case CameraFPS.FPSLimitType.Quarter:

                if (ScopeCamera.enabled && ScopeCamera != null)
                    ScopeCamera.enabled = false;

                if (cameraDisable.canZoomCamera)
                {

                    _cFPS = currentFPS / 4;

                    fpsTime = 1 / _cFPS;

                    if (fpsTimer < fpsTime)
                        fpsTimer += Time.deltaTime;

                    if (fpsTimer > fpsTime)
                    {
                        ScopeCamera.Render();
                        fpsTimer = 0;
                    }
                }

                break;
            case CameraFPS.FPSLimitType.Fixed:

                if (ScopeCamera.enabled)
                    ScopeCamera.enabled = false;

                if(cameraDisable.canZoomCamera)
                {
                    fpsTime = 1 / cameraFPS.fixedFps;

                    if (fpsTimer < fpsTime)
                        fpsTimer += Time.deltaTime;

                    if (fpsTimer > fpsTime)
                    {
                        ScopeCamera.Render();
                        fpsTimer = 0;
                    }
                }            

                break;
            
        }

        switch (cameraDisable.o_cameraDisable)
        {
            case CameraDisable.cameraDisable.None:

                cameraDisable.canZoomCamera = true;

                break;
            case CameraDisable.cameraDisable.Distance:

                Debug.Log(Vector3.Distance(this.transform.position, Camera.main.transform.position));

                if(Vector3.Distance(this.transform.position, Camera.main.transform.position) < cameraDisable.distance)
                {
                    cameraDisable.canZoomCamera = true;
                }
                else
                {
                    cameraDisable.canZoomCamera = false;
                }

                break;
        }

        // Debug.Log("FPS: " + 1.0f / Time.deltaTime + " | " + Time.deltaTime + " | " + Time.fixedDeltaTime + " | " + 1.0f / Time.smoothDeltaTime);
    }

    public void increaseZoom()
    {
        targetZoom += zooming.zoomIncrements;

        if (targetZoom > zooming.maxZoom)
            targetZoom = zooming.maxZoom;
    }

    public void decreaseZoom()
    {
        targetZoom -= zooming.zoomIncrements;

        if (targetZoom < zooming.minZoom)
            targetZoom = zooming.minZoom;
    }

    public void changeReticleIllumnation()
    {
        reticleBrightness.v_brightness = scopeMaterial.GetFloat("Reticle_Brightness");

        if (Input.GetKey(reticleBrightness.brightness_increase))
        {
            if (reticleBrightness.v_brightness < reticleBrightness.v_brightness_max)
                reticleBrightness.v_brightness += reticleBrightness.v_brightness_increase;
        }
        if (Input.GetKey(reticleBrightness.brightness_decrease))
        {
            if (reticleBrightness.v_brightness > reticleBrightness.v_brightness_min)
                reticleBrightness.v_brightness -= reticleBrightness.v_brightness_increase;
        }

        scopeMaterial.SetFloat("Reticle_Brightness", reticleBrightness.v_brightness);
    }

}
