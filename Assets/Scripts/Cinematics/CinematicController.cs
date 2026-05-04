using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour 
{

    public static bool skipCinematic = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
            Cinematic_DisableIngameHud();
        if (Input.GetKeyDown(KeyCode.End))
            Cinematic_EnableIngameHud();
    }

    public static void Cinematic_Start()
    {
        GameManager.instance.ChangeMenuState(true);
    }
    public static void Cinematic_End()
    {
        GameManager.instance.ChangeMenuState(false);
    }

    public static void Cinematic_DisablePlayerController()
    {
        GameManager.instance.controller.enabled = false;
        //GameManager.instance.controller.animator.applyRootMotion = true;
    }
    public static void Cinematic_EnablePlayerController()
    {
        GameManager.instance.controller.enabled = true;
        //GameManager.instance.controller.animator.applyRootMotion = false;
    }
    public static void Cinematic_DisableCameraController()
    {
        GameManager.instance.cameraController.enabled  = false;
    }
    public static void Cinematic_EnableCameraController()
    {
        GameManager.instance.cameraController.enabled = true;
    }
    public static void Cinematic_DisableIngameHud()
    {
        GameManager.instance.hudManager.HideIngameHud();
    }
    public static void Cinematic_EnableIngameHud()
    {
        GameManager.instance.hudManager.ShowIngameHud();
    }
    public static void Cinematic_DisablePlayerPrefab()
    {
        GameManager.instance.controller.gameObject.SetActive(false);
    }
    public static void Cinematic_EnablePlayerPrefab()
    {
        GameManager.instance.controller.gameObject.SetActive(true);
    }
    public static void Cinematic_DisableMainCamera()
    {
        GameManager.instance.cameraController.gameObject.SetActive(false);
    }
    public static void Cinematic_EnableMainCamera()
    {
        GameManager.instance.cameraController.gameObject.SetActive(true);
        GameManager.instance.graphicsManager.RestoreRT();
    }
}
