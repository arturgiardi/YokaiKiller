using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

 public enum GameState {Playing, Paused, Finished, Cinematic};

public class GameManager : MonoBehaviour
{
   

    public static GameManager instance = null;    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameState gameState = GameState.Playing;


    [SerializeField]
    private NewController _controller;   //Reference to the character controller
    public NewController controller
    {
        get { return _controller; }
        private set { _controller = value; }
    }
    [SerializeField]
    private CameraManager _cameraController; //Reference to the camera controller
    public CameraManager cameraController
    {
        get { return _cameraController; }
        private set { _cameraController = value; }
    }
    [SerializeField]
    private GraphicsManager _graphicsManager; //Reference to the graphics manager
    public GraphicsManager graphicsManager
    {
        get { return _graphicsManager; }
        private set { _graphicsManager = value; }
    }
    [SerializeField]
    private SceneSwitchTest _sceneSwitcher; //Reference to the scene switcher
    public SceneSwitchTest sceneSwitcher
    {
        get { return _sceneSwitcher; }
        private set { _sceneSwitcher = value; }
    }

    [SerializeField]
    private HUDManager _hudManager; //Reference to the hud manager
    public HUDManager hudManager
    {
        get { return _hudManager; }
        private set { _hudManager = value; }
    }

    [SerializeField]
    private PlayerInventory _playerInventory; //Reference to the hud manager
    public PlayerInventory playerInventory
    {
        get { return _playerInventory; }
        private set { _playerInventory = value; }
    }

    [SerializeField]
    private InventoryManager _inventoryManager; //Reference to the hud manager
    public InventoryManager inventoryManager
    {
        get { return _inventoryManager; }
        private set { _inventoryManager = value; }
    }

    [SerializeField]
    private MenuManager _menuManager; //Reference to the hud manager
    public MenuManager menuManager
    {
        get { return _menuManager; }
        private set { _menuManager = value; }
    }

    public VolumeManager volumeManager;

    public LevelUpHud lvlUpHud;

    [SerializeField]
    private EnemyHealthbarManager _enemyHbManager; //Reference to the hud manager
    public EnemyHealthbarManager enemyHbManager
    {
        get { return _enemyHbManager; }
        private set { _enemyHbManager = value; }
    }


    [SerializeField]
    private string currentScene = "Area1_Room1"; //Name of the current loaded level

    [SerializeField] CanvasGroup GameMenuCG;
    [SerializeField] Button GameMenuFirstOption;



    ////////
    public ItemDatabase itemDB;
    public ItemObtainedValidation itemValidator;
    public InteractionManager interactionManager;
    //////



    public BossHpManager bossHp;

    private bool _paused = false;
    public bool paused
    {
        get{ return _paused; }
        private set { _paused = value; }
    }

    public Animator endGameAnimator;

    public bool ltPressed;
    public bool rtPressed;
    public bool selectPressed;

    //Awake is always called before any Start functions
    void Awake()
    {
        //If a game manager already exists, delete the new one and stop execution;
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        //If a game manager does not exists, pick this one as the active one
        else
        {
            this.gameObject.name = "ActiveGameManager";
            instance = this;
            DontDestroyOnLoad(gameObject); //Will not be destroyed 
        }
        //AudioListener.pause = false;
    }

    void OnDisable()
	{
		if(instance == this)
            instance = null;
	}

    void Start()
    {
        if(instance != this)
            return;
        InputManager.OnPressStart += PauseGame;
        InputManager.OnPressLT += PressLT;
        InputManager.OnPressRT += PressRT;
        InputManager.OnReleaseLT += ReleaseLT;
        InputManager.OnReleaseRT += ReleaseRT;
        InputManager.OnPressBack += PressSelect;
        InputManager.OnReleaseBack += ReleaseSelect;
        PlayerStats.instance.OnDeath += StartDeathBehaviour;
    }

    public void PauseGame()
    {
        if (ltPressed && rtPressed && selectPressed)
        {
            ExitDemo();
            return;
        }
        if (!paused)
        {
            menuManager.TurnMenu();
            gameState = GameState.Paused;
            Time.timeScale = 0;
            // GameMenuCG.alpha = 1;
            // GameMenuCG.interactable = true;
            // EventSystem.current.SetSelectedGameObject(GameMenuFirstOption.gameObject);
            paused = true;
        }
        else
        {
            menuManager.TurnMenu();
            Time.timeScale = 1;
            StartCoroutine(_SleepBeforeUnpause());
            // GameMenuCG.alpha = 0;
            // GameMenuCG.interactable = false;
            //EventSystem.current.SetSelectedGameObject(GameMenuFirstOption.gameObject);
        }
        
    }

    public void ChangeMenuState(bool locked)
    {
        if(locked)
        {
            InputManager.OnPressStart -= PauseGame;
            hudManager.SwitchMapButtonState(false);
        }
        else
        {
            InputManager.OnPressStart -= PauseGame;
            hudManager.SwitchMapButtonState(false);
            InputManager.OnPressStart += PauseGame;
            hudManager.SwitchMapButtonState(true);
        }
    }

    IEnumerator _SleepBeforeUnpause()
    {
        yield return null;
        gameState = GameState.Playing;
        paused = false;
    }

    void StartDeathBehaviour(GameObject instigator, DamageInfo damage, Stats attacker)
    {
        menuManager.SwitchDeathMenu();
        InputManager.OnPressStart -= PauseGame;
        InputManager.OnPressStart += LoadLastState;
    }
    void StartReviveBehaviour()
    {
        menuManager.SwitchDeathMenu();
        InputManager.OnPressStart -= LoadLastState;
        InputManager.OnPressStart += PauseGame;
    }

    void LoadLastState()
    {
        volumeManager.StopMusic(4);
        PlayerStatsController.instance.LoadSaveState();
        StartReviveBehaviour();
    }


    //Set the scene level we're currently in into the reference
    public void SetScene(string name)
    {
        currentScene = name;
    }
    //Set the scene level we're currently in into the reference
    public string GetScene()
    {
        return currentScene;
    }

    public void SwitchLevelUp(bool on)
    {
        if(on)
        {
            InputManager.OnPressStart -= PauseGame;
            gameState = GameState.Paused;
            Time.timeScale = 0;
            lvlUpHud.SetValuesUp(PlayerStatsController.instance.stats);
            menuManager.TurnLvlUpMenu(true);
        }
        else
        {
            InputManager.OnPressStart += PauseGame;
            Time.timeScale = 1;
            menuManager.TurnLvlUpMenu(false);
            StartCoroutine(_SleepBeforeUnpause());
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    public void EndDemo(TriggerMessage.MessageContent message)
    {
        PlayerStats.instance.Unset();
        InputManager.singleton.readingInput = false;
        endGameAnimator.SetTrigger("Finish");
    }

    private void Update()
    {
        if (instance != this)
            return;
    }


    void PressRT()
    {
        rtPressed = true;
    }

    void PressLT()
    {
        ltPressed = true;
    }

    void ReleaseRT()
    {
        rtPressed = false;
    }

    void ReleaseLT()
    {
        ltPressed = false;
    }

    void PressSelect()
    {
        selectPressed = true;
    }

    void ReleaseSelect()
    {
        selectPressed = false;
    }

    void ExitDemo()
    {
        PlayerStats.instance.Unset();
        InputManager.singleton.readingInput = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

}
