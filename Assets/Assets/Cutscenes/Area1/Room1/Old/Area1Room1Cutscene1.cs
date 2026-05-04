using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Cutscene que mostra Raiko e seus companheiros chegando no monte Ooe
/// </summary>
[RequireComponent (typeof(PlayableDirector))]
public class Area1Room1Cutscene1 : MonoBehaviour 
{
    //int uniqueID = 1;
    PlayableDirector director;
    [SerializeField] PlayableAsset timeline1;
    [SerializeField] PlayableAsset timeline2;
    [SerializeField] PlayableAsset timeline3;
    [SerializeField] PlayableAsset timeline4;
    [SerializeField] PlayableAsset timeline5;
    [SerializeField] PlayableAsset timeline6;
    [SerializeField] PlayableAsset timelineIbarakiSpell;
    [SerializeField] PlayableAsset timeline7;

    [SerializeField] Dialogue[] dialogue1;
    [SerializeField] Dialogue[] dialogue2;
    [SerializeField] Dialogue[] dialogue3;
    [SerializeField] Dialogue[] dialogue4;
    [SerializeField] Dialogue[] dialogue5;
    [SerializeField] Dialogue[] dialogue6;
    [SerializeField] Dialogue[] dialogue7;
    [SerializeField] Dialogue[] dialogue8;
    [SerializeField] Dialogue[] dialogue9;




    [SerializeField] AudioClip ambushMusic;
    [SerializeField] string area1Room2;

    [System.Serializable]
    public class MessageContent
    {
        public float[] numbers;
        public Vector3[] vectors;
        public string[] strings;
    }

    [SerializeField] private GameObject target;
    [SerializeField] private string message;
    [SerializeField] private MessageContent messageContent;
    [SerializeField] private string message2;
    [SerializeField] private MessageContent message2Content;

    IEnumerator cutCoroutine;
    //[SerializeField] private bool singeton = false;

    bool triggered = false;

    void Awake()
    {
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
        director = GetComponent<PlayableDirector>();
        CinematicController.Cinematic_DisablePlayerPrefab();
        CinematicController.Cinematic_DisableCameraController();
        CinematicController.Cinematic_DisableIngameHud();
    }

	void Update () 
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
        Debug.Log("Starting cutscene A1-R1-C1");
        CinematicController.Cinematic_Start();
        //ScreenFaderManager.instance.ScreenOut();
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
        DialogueManager.instance.StartDialogue(dialogue2);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        AudioManager.instance.ChangeAmbientVolume(0.5f, 0);
        yield return new WaitForSeconds(1);
        DialogueManager.instance.StartDialogue(dialogue3);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        AudioManager.instance.music.clip = ambushMusic;
        AudioManager.instance.music.Play();
        DialogueManager.instance.StartDialogue(dialogue4);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline3;
        director.Play();
        yield return new WaitForSeconds((float)timeline3.duration);
        DialogueManager.instance.StartDialogue(dialogue5);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline4;
        director.Play();
        DialogueManager.instance.StartDialogue(dialogue6);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline5;
        director.Play();
        yield return new WaitForSeconds(.2f);
        DialogueManager.instance.StartDialogue(dialogue7);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        yield return new WaitForSeconds(.2f);
        director.playableAsset = timeline6;
        director.Play();
        yield return new WaitForSeconds((float)timeline6.duration);
        DialogueManager.instance.StartDialogue(dialogue8);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timelineIbarakiSpell;
        director.Play();
        yield return new WaitForSeconds((float)timelineIbarakiSpell.duration);

        target.SendMessage(message, messageContent, SendMessageOptions.DontRequireReceiver);
        director.playableAsset = timeline7;
        director.Play();
        yield return new WaitForSeconds(2.2f);
        DialogueManager.instance.StartDialogue(dialogue9);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);

        InputManager.OnPressBack -= SkipCut;

        AudioManager.instance.ChangeVolume(0.5f, 0);
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        //GameManager.instance.controller.gameObject.SendMessage(message2, message2Content, SendMessageOptions.DontRequireReceiver);
        //SceneManager.LoadScene(area1Room2);
        //GameManager.instance.sceneSwitcher.LoadScene("Area1_Room2");
        TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[]{"Area1_Room2","A"});
        GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
    }

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
        AudioManager.instance.ChangeVolume(0.5f, 0);
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        //GameManager.instance.controller.gameObject.SendMessage(message2, message2Content, SendMessageOptions.DontRequireReceiver);
        //SceneManager.LoadScene(area1Room2);
        //GameManager.instance.sceneSwitcher.LoadScene("Area1_Room2");
        TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[]{"Area1_Room2","A"});
        GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
    }

}
