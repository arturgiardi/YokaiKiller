using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Cutscene que mostra Raiko e seus companheiros chegando no monte Ooe
/// </summary>
[RequireComponent (typeof(PlayableDirector))]
public class Area1Room1Cutscene01 : MonoBehaviour
{
    [Header("Timelines")]
    PlayableDirector director;
    [SerializeField] PlayableAsset timeline1;
    [SerializeField] PlayableAsset timeline2;

    [Header("Dialogues")]
    [SerializeField] Dialogue[] dialogue1;

    [Header("GO References")]
    [SerializeField] GameObject cutscene_Raiko;
    [SerializeField] GameObject raiko;
    [SerializeField] Transform cutsceneCameraPosition;
    [SerializeField] GameObject gameCamera;


    bool triggered = false;

    IEnumerator cutCoroutine;

    void OnEnable()
    {
        if (!triggered)
        {
            InputManager.OnPressBack += SkipCut;
        }
    }

    void OnDisable()
    {
        InputManager.OnPressBack -= SkipCut;
    }



    void Start()
    {
        //raiko.SetActive(false);
        director = GetComponent<PlayableDirector>();
        director.playableAsset = null;
        CinematicController.Cinematic_DisablePlayerPrefab();
        CinematicController.Cinematic_DisableMainCamera();
        CinematicController.Cinematic_DisableIngameHud();     
    }

    void Update()
    {
        if (!triggered)
        {
            cutCoroutine = _Cutscene();
            StartCoroutine(cutCoroutine);
            triggered = true;
        }
    }

    IEnumerator _Cutscene()
    {
        Debug.Log("Starting cutscene A1-R1-C2");
        gameCamera.transform.position = cutsceneCameraPosition.position;
        CinematicController.Cinematic_Start();
        ScreenFaderManager.instance.ScreenOut();
        yield return new WaitForSeconds(1);
        ScreenFaderManager.instance.ScreenFadeIn();
        yield return new WaitForSeconds(1);
        director.playableAsset = timeline1;
        director.Play();
        yield return new WaitForSeconds((float)timeline1.duration); 
        DialogueManager.instance.StartDialogue(dialogue1);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline2;
        director.Play();
        yield return new WaitForSeconds((float)timeline2.duration);

        InputManager.OnPressBack -= SkipCut;

        raiko.transform.position = cutscene_Raiko.transform.position;
        raiko.SetActive(true);
        CinematicController.Cinematic_EnablePlayerPrefab();
        CinematicController.Cinematic_EnableMainCamera();
        CinematicController.Cinematic_EnableIngameHud();
        CinematicController.Cinematic_End();
        PlayerStatsController.instance.stats.Heal(999999);
        //PlayerStatsController.instance.SaveState();
        gameObject.SetActive(false);
    }

    void SkipCut()
    {
        InputManager.OnPressBack -= SkipCut;
        DialogueManager.instance.EndDialogue();
        if (cutCoroutine != null)
            StopCoroutine(cutCoroutine);
        StartCoroutine(_SkipCut());
    }

    IEnumerator _SkipCut()
    {
        yield return null;
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        director.playableAsset = timeline2;
        director.Play();
        yield return new WaitForSeconds((float)timeline2.duration);
        raiko.transform.position = cutscene_Raiko.transform.position;
        raiko.SetActive(true);
        gameCamera.transform.position = cutsceneCameraPosition.position;
        //Destroy(cutscene_Camera);
        //Destroy(cutscene_Raiko);
        //TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[] { "Area1_Room2", "A" });
        //GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
        CinematicController.Cinematic_EnablePlayerPrefab();
        CinematicController.Cinematic_EnableMainCamera();
        CinematicController.Cinematic_EnableIngameHud();
        ScreenFaderManager.instance.ScreenFadeIn();        
        CinematicController.Cinematic_End();
        PlayerStatsController.instance.stats.Heal(999999);
        //PlayerStatsController.instance.SaveState();
        gameObject.SetActive(false);
    }
}
