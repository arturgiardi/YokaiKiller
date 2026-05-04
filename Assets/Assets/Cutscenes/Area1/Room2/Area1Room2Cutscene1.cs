using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// Cutscene que mostra Raiko caindo no buraco e inicia o gameplay
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class Area1Room2Cutscene1 : MonoBehaviour
{
    //int uniqueID = 1;

    PlayableDirector director;
    [SerializeField]
    PlayableAsset timeline1;
    [SerializeField]
    PlayableAsset timeline2;
    [SerializeField]
    PlayableAsset timeline3;
    [SerializeField]
    PlayableAsset timeline4;
    [SerializeField]
    PlayableAsset timeline5;

    [SerializeField]
    Dialogue[] dialogue1;
    [SerializeField]
    Dialogue[] dialogue2;
    [SerializeField]
    Dialogue[] dialogue3;
    [SerializeField]
    Dialogue[] dialogue4;

    [SerializeField]
    PlayableDirector waterDropDirector;
    [SerializeField]
    AudioFade waterDropAudioSource;

    [SerializeField]
    AudioClip area1Music;

    bool triggered = false;

    [SerializeField]
    GameObject cutscene_Raiko;
    [SerializeField]
    GameObject raiko;
    [SerializeField]
    GameObject cutscene_Camera;
    [SerializeField]
    GameObject gameCamera;

    IEnumerator cutCoroutine;

    void Awake()
    {
        //SkipCut(); 
        if (CinematicController.skipCinematic)
        {
            SkipCut();
            CinematicController.skipCinematic = false;
        }
            
        //if (PlayerStatus.Instance.WasCutsceneWatched(uniqueID))
            //Destroy(gameObject);
    }

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
        //Debug.Log("Start");
        raiko = GameManager.instance.controller.gameObject;
        gameCamera = GameManager.instance.cameraController.focusPivot.gameObject;
        //gameCamera = GameObject.Find("Camera Focus Pivot");
        director = GetComponent<PlayableDirector>();
        CinematicController.Cinematic_DisablePlayerPrefab();
        CinematicController.Cinematic_DisableMainCamera();
        CinematicController.Cinematic_DisableIngameHud();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
        if (!triggered)
        {
            cutCoroutine = _Cutscene();
            StartCoroutine(cutCoroutine);
            triggered = true;
        }
    }

    IEnumerator _Cutscene()
    {
        Debug.Log("Starting cutscene A1-R2-C1", gameObject);
        //ScreenFaderManager.instance.ScreenOut();
        yield return new WaitForSecondsRealtime(.5f);
        ScreenFaderManager.instance.ScreenFadeIn();
        Debug.Log("Cutscene");
        director.playableAsset = timeline1;
        director.Play();
        yield return new WaitForSeconds((float)timeline1.duration + 1);
        director.playableAsset = timeline2;
        director.Play();
        yield return new WaitForSeconds((float)timeline2.duration - 3f);
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(3);
        waterDropDirector.Play();
        yield return new WaitForSeconds((float)waterDropDirector.playableAsset.duration * 1.5f);
        director.playableAsset = timeline3;
        director.Play();
        DialogueManager.instance.StartDialogue(dialogue1);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        ScreenFaderManager.instance.ScreenFadeIn();
        yield return new WaitForSeconds(1.5f);
        director.playableAsset = timeline4;
        director.Play();
        waterDropAudioSource.ChangeVolume(0.1f, 0);
        yield return new WaitForSeconds((float)timeline4.duration);
        DialogueManager.instance.StartDialogue(dialogue2);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline5;
        director.Play();
        yield return new WaitForSeconds((float)timeline5.duration+1);
        DialogueManager.instance.StartDialogue(dialogue3);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.StartDialogue(dialogue4);

        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);

        InputManager.OnPressBack -= SkipCut;
        
        raiko.transform.position = cutscene_Raiko.transform.position;
        Destroy(cutscene_Raiko);
        gameCamera.transform.position = cutscene_Camera.transform.position;
       
        Destroy(cutscene_Camera);

        CinematicController.Cinematic_EnablePlayerPrefab();
        CinematicController.Cinematic_EnableMainCamera();
        CinematicController.Cinematic_EnableCameraController();
        CinematicController.Cinematic_EnableIngameHud();
        AudioManager.instance.music.Stop();
        AudioManager.instance.ChangeVolume(5, 1);
        AudioManager.instance.music.clip = area1Music;
        AudioManager.instance.music.Play();
        CinematicController.Cinematic_End();
        PlayerStatsController.instance.stats.Heal(999999);
        PlayerStatsController.instance.SaveState();
        CinematicController.Cinematic_EnablePlayerController();
        //PlayerStatus.Instance.CheckCutsceneAsWatched(uniqueID);
        //PlayerStatus.Instance.CheckPoint(raiko.transform.position);



    }

    /*public void SkipCut()
    {
        raiko = GameManager.instance.controller.gameObject;
        Destroy(cutscene_Raiko);
        Destroy(cutscene_Camera);
        CinematicController.Cinematic_EnablePlayerPrefab();
        CinematicController.Cinematic_EnableMainCamera();
        CinematicController.Cinematic_EnableCameraController();
        CinematicController.Cinematic_EnableIngameHud();
        AudioManager.instance.music.Stop();
        AudioManager.instance.ChangeVolume(5, 1);
        AudioManager.instance.music.clip = area1Music;
        AudioManager.instance.music.Play();
        //PlayerStatus.Instance.CheckCutsceneAsWatched(uniqueID);
        //PlayerStatus.Instance.CheckPoint(raiko.transform.position);
        Destroy(gameObject);
    }*/

    void SkipCut()
    {
        InputManager.OnPressBack -= SkipCut;
        DialogueManager.instance.EndDialogue();
        if(cutCoroutine != null)
            StopCoroutine(cutCoroutine);
        StartCoroutine(_SkipCut());
    }

    IEnumerator _SkipCut()
    {
        yield return null;  
        //raiko.transform.position = cutscene_Raiko.transform.position;
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        raiko.transform.position = cutscene_Raiko.transform.position;
        gameCamera.transform.position = cutscene_Camera.transform.position;
        Destroy(cutscene_Camera);
        Destroy(cutscene_Raiko);
        TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[]{"Area1_Room2","A"});
        GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
        CinematicController.Cinematic_EnablePlayerPrefab();
        CinematicController.Cinematic_EnableMainCamera();
        CinematicController.Cinematic_EnableCameraController();
        CinematicController.Cinematic_EnableIngameHud();
        ScreenFaderManager.instance.ScreenFadeIn();
        AudioManager.instance.music.Stop();
        AudioManager.instance.ChangeVolume(5, 1);
        AudioManager.instance.music.clip = area1Music;
        AudioManager.instance.music.Play();
        CinematicController.Cinematic_End();
        PlayerStatsController.instance.stats.Heal(999999);
        PlayerStatsController.instance.SaveState();
    }

}
