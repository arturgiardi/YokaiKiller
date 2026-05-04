using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Area1Room1Cutscene02 : MonoBehaviour
{
    [System.Serializable]
    public class MessageContent
    {
        public float[] numbers;
        public Vector3[] vectors;
        public string[] strings;
    }

    [Header("Timelines")]
    PlayableDirector director;
    [SerializeField] PlayableAsset timeline1;
    [SerializeField] PlayableAsset timeline2;
    [SerializeField] PlayableAsset timeline3;
    [SerializeField] PlayableAsset timeline3_5;
    [SerializeField] PlayableAsset timeline4;
    [SerializeField] PlayableAsset timeline5;
    [SerializeField] PlayableAsset timeline6;
    [SerializeField] PlayableAsset timeline7;

    [Header("Dialogues")]
    [SerializeField] Dialogue[] dialogue1;
    [SerializeField] Dialogue[] dialogue2;
    [SerializeField] Dialogue[] dialogue2_5;
    [SerializeField] Dialogue[] dialogue3;
    [SerializeField] Dialogue[] dialogue4;
    [SerializeField] Dialogue[] dialogue4_5;
    [SerializeField] Dialogue[] dialogue5;
    [SerializeField] Dialogue[] dialogue6;
    [SerializeField] Dialogue[] dialogue7;
    

    [Header ("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip tensionMusic;
    [SerializeField] AudioClip ambushMusic;
    [SerializeField] AudioClip swordAudio;

    [Header("GO References")]
    [SerializeField] GameObject cutsceneRaiko;
    [SerializeField] GameObject raiko;
    [SerializeField] GameObject cutsceneCamera;
    [SerializeField] GameObject gameCamera;
    [SerializeField] Transform cameraFinalPosition;
    [SerializeField] Animator usui;
    [SerializeField] Animator watanabe;
    [SerializeField] Animator kintaro;
    [SerializeField] Animator urabe;
    
    [SerializeField] private GameObject target;
    [SerializeField] private string message;
    [SerializeField] private MessageContent messageContent;
    [SerializeField] private string message2;
    [SerializeField] private MessageContent message2Content;

    bool triggered = false;

    IEnumerator cutCoroutine;

    void OnDisable()
    {
        InputManager.OnPressBack -= SkipCut;
    }

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.playableAsset = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //checar se evento já começou
            //se não começou iniciar evento
            if (!triggered)
            {
                InputManager.OnPressBack += SkipCut;
                AudioManager.instance.ChangeAmbientVolume(0.5f,0);
                cutsceneCamera.SetActive(true);
                CinematicController.Cinematic_DisablePlayerPrefab();
                CinematicController.Cinematic_DisablePlayerController();
                CinematicController.Cinematic_DisableMainCamera();
                CinematicController.Cinematic_DisableIngameHud();

                cutCoroutine = _Cutscene();
                cutsceneRaiko.SetActive(true);
                cutsceneRaiko.transform.position = raiko.transform.position;                
                cutsceneCamera.transform.position = gameCamera.transform.position;
                StartCoroutine(cutCoroutine);
                triggered = true;
            }
        }
    }

    IEnumerator _Cutscene()
    {
        Debug.Log("Starting cutscene A1-R1-C02");
        WaitForSecondsRealtime waitHalfSecond = new WaitForSecondsRealtime(0.5f);
        CinematicController.Cinematic_Start();
        yield return new WaitForSecondsRealtime(2);
        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.05f);
        while (cutsceneCamera.transform.position.x < cameraFinalPosition.position.x)
        {
            cutsceneCamera.transform.position = new Vector3(
                cutsceneCamera.transform.position.x + 0.3f,
                cutsceneCamera.transform.position.y,
                cutsceneCamera.transform.position.z);
            yield return waitTime;
        }
        AudioManager.instance.SetMusic(tensionMusic, true);
        AudioManager.instance.music.Play();
        director.playableAsset = timeline1;
        director.Play();
        yield return new WaitForSeconds((float)timeline1.duration);        
        DialogueManager.instance.StartDialogue(dialogue1);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline2;
        director.Play();
        DialogueManager.instance.StartDialogue(dialogue2);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);

        yield return waitHalfSecond;
        DialogueManager.instance.StartDialogue(dialogue2_5);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        
        DialogueManager.instance.StartDialogue(dialogue3);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline3;
        director.Play();
        yield return new WaitForSeconds((float)(timeline3.duration *0.4));
        DialogueManager.instance.StartDialogue(dialogue4);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline3_5;
        director.Play();
        yield return new WaitForSeconds((float)timeline3_5.duration);
        DialogueManager.instance.StartDialogue(dialogue4_5);
        AudioManager.instance.ChangeVolume(0.3f, 0);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        yield return waitHalfSecond;

        director.playableAsset = timeline4;
        director.Play();
        yield return new WaitForSeconds((float)timeline4.duration);
        AudioManager.instance.ChangeVolume(10, 1);
        AudioManager.instance.SetMusic(ambushMusic, true);
        AudioManager.instance.music.Play();
        DialogueManager.instance.StartDialogue(dialogue5);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        //director.playableAsset = timeline5;
        //director.Play();
        //yield return new WaitForSeconds((float)timeline5.duration);

        kintaro.SetTrigger("Battle");
        watanabe.SetTrigger("Battle");
        urabe.SetTrigger("Battle");
        usui.SetTrigger("Battle");
        audioSource.clip = swordAudio;
        audioSource.Play();
        yield return new WaitForSeconds((float)timeline5.duration);

        DialogueManager.instance.StartDialogue(dialogue6);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);
        director.playableAsset = timeline6;
        director.Play();
        yield return new WaitForSeconds((float)timeline6.duration);

        target.SendMessage(message, messageContent, SendMessageOptions.DontRequireReceiver);
        yield return new WaitForSecondsRealtime(0.3f);
        director.playableAsset = timeline7;
        director.Play();
        yield return new WaitForSeconds(2.2f);
        kintaro.SetTrigger("Idle");

        DialogueManager.instance.StartDialogue(dialogue7);
        yield return new WaitUntil(() => DialogueManager.instance.endDialogue);

        InputManager.OnPressBack -= SkipCut;

        AudioManager.instance.ChangeVolume(0.5f, 0);
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[] { "Area1_Room2", "A" });
        GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
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
        AudioManager.instance.ChangeVolume(0.5f, 0);
        ScreenFaderManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(1.2f);
        //GameManager.instance.controller.gameObject.SendMessage(message2, message2Content, SendMessageOptions.DontRequireReceiver);
        //SceneManager.LoadScene(area1Room2);
        //GameManager.instance.sceneSwitcher.LoadScene("Area1_Room2");
        TriggerMessage.MessageContent transitionInfo = new TriggerMessage.MessageContent(new float[0], new Vector3[0], new string[] { "Area1_Room2", "A" });
        GameManager.instance.sceneSwitcher.StartSwitch(transitionInfo);
    }
}