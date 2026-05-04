using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour
{
    public Camera mainGameCamera;
    public Camera reflexCamera;
    [TextArea()] public string iniPath;
    public static RenderTexture gameRenderTexture;
    public Material[] reflectiveMats;
    public RawImage renderImage;

    void Start()
    {
        if(gameRenderTexture == null)
            GetDefaultRT();
    }

    [ContextMenu("Change to quarter")]
    public void ChangeRTQuarter()
    {
        gameRenderTexture = new RenderTexture( 480, 270, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        gameRenderTexture.filterMode = FilterMode.Point;
        ApplyNewRT(gameRenderTexture);
    }

    public void ChangeRTThird()
    {
        gameRenderTexture = new RenderTexture( 640, 360, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        gameRenderTexture.filterMode = FilterMode.Point;
        ApplyNewRT(gameRenderTexture);
    }

    public void ChangeRTFull()
    {
        gameRenderTexture = new RenderTexture( 1920, 1080, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        gameRenderTexture.filterMode = FilterMode.Point;
        ApplyNewRT(gameRenderTexture);
    }

    public void GetDefaultRT()
    {
        gameRenderTexture = new RenderTexture( Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        gameRenderTexture.filterMode = FilterMode.Point;
        ApplyNewRT(gameRenderTexture);
    }


    void ApplyNewRT(RenderTexture rt)
    {
        if ( mainGameCamera.targetTexture != null ) 
        {
            mainGameCamera.targetTexture.Release( );
        }
        foreach(Material mat in reflectiveMats)
        {
            mat.SetTexture("_Reflection", rt);
        }
        mainGameCamera.targetTexture = rt;
        renderImage.texture = rt;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ToggleReflex();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ToggleRenderPath();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeRTFull();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ChangeRTThird();
        }
    }


    void ToggleReflex()
    {
        if(reflexCamera.gameObject.activeInHierarchy)
        {
            reflexCamera.gameObject.SetActive(false);
        }
        else
        {
            reflexCamera.gameObject.SetActive(true);
        }
    }

    void ToggleRenderPath()
    {
        if (mainGameCamera.renderingPath == RenderingPath.DeferredShading)
        {
            mainGameCamera.renderingPath = RenderingPath.Forward;
            reflexCamera.renderingPath = RenderingPath.Forward;
        }
        else
        {
            mainGameCamera.renderingPath = RenderingPath.DeferredShading;
            reflexCamera.renderingPath = RenderingPath.DeferredShading;
        }
    }

    public void AssingRT(Camera targetCamera)
    {
        if(gameRenderTexture == null)
            GetDefaultRT();
        mainGameCamera.targetTexture.Release();
        targetCamera.targetTexture = gameRenderTexture;
    }

    public void RestoreRT()
    {
        mainGameCamera.targetTexture = gameRenderTexture;
    }
}
