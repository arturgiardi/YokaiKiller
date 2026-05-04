using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public static HUDManager hudMan;

	[SerializeField]
    private Animator hudAnim;
    static Animator hudAnim_st;

    [SerializeField]
    private  EventSystem evSys;
    static EventSystem evSys_st;

    [SerializeField]
    private GameObject gameOverFocus;
    static GameObject gameOverFocus_st;

    public GameObject pauseMenu;

	[SerializeField]
	AudioClip menuSelectSound;

    [SerializeField]
    private GameObject pauseHud;
    static GameObject pauseHud_st;

    [SerializeField]
    private BossHpManager _bossHpManager;
    public BossHpManager bossHpManager
    {
        get
        {
            return _bossHpManager;
        }
        private set
        {
            _bossHpManager = value;
        }
    }

    public GameObject itemPromptCardPrefab;
    public Transform itemPromptList;
  
    public InventoryManager inventoryManager;

    public CanvasGroup mapCG;
    IEnumerator mapCoroutine;

    void Awake()
    {
        if (hudMan == null)
        {
            hudMan = this;
            evSys_st = evSys;
            gameOverFocus_st = gameOverFocus;
            hudAnim_st = hudAnim;
        }
            
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (hudMan != this)
            return;
        pauseHud_st = pauseHud;
    }

    void OnEnable()
    {
        InventoryManager.OnItemGet += OnItemGet;
        InputManager.OnPressLT += ShowMap;
        InputManager.OnReleaseLT += HideMap;
    }

    void OnDisable()
    {
        InventoryManager.OnItemGet -= OnItemGet;
        InputManager.OnPressLT -= ShowMap;
        InputManager.OnReleaseLT -= HideMap;
        if(hudMan == this)
            hudMan = null;
    }

    public void SwitchMapButtonState(bool on)
    {
        if(on)
            InputManager.OnPressLT += ShowMap;
        else
            InputManager.OnPressLT -= ShowMap;

    }

    public static void ShowGameOver(GameObject instigator)
    {
		Cursor.visible = true;
		AudioManager.instance.ChangeVolume(5,0);
        evSys_st.SetSelectedGameObject(gameOverFocus_st);
        evSys_st.firstSelectedGameObject = gameOverFocus_st;
       // gameOverFocus_st.GetComponent<Button>().Select();
        hudAnim_st.SetTrigger("GameOver");
    }

    public void FocusGameOverContinue()
    {
        if (hudMan != this)
            return;
        //if (!hudAnim)
        //return;
        print("Marcando");
        evSys_st.SetSelectedGameObject(gameOverFocus);
        evSys_st.firstSelectedGameObject = gameOverFocus;
        //gameOverFocus.GetComponent<Button>().Select();
        StartCoroutine(_WaitToFocus());
    }

    private IEnumerator _WaitToFocus()
    {
        evSys.SetSelectedGameObject(null, null);
        yield return null;
        //evSys.SetSelectedGameObject(gameOverFocus);
        // evSys.firstSelectedGameObject = gameOverFocus;
        evSys.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.1f);
        evSys.SetSelectedGameObject(gameOverFocus);
        print("desisto");
    }

    public void ResetHUD()
    {
        evSys.SetSelectedGameObject(null);
        hudAnim.ResetTrigger ("GameOver");
		hudAnim.SetTrigger("Default");
		AudioManager.instance.PlayEffect(menuSelectSound);
    }
	public void PrintMe()
    {
	}

    public void RestartGame()
    {
		ResetHUD();
		StartCoroutine (_Revive ());
    }

    public void MainMenu()
    {
		Cursor.visible = true;
		Destroy(GameManager.instance.gameObject);
		SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        if (!pauseMenu.activeSelf)
        {
			Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else
        {
			Cursor.visible = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

	IEnumerator _Revive()
    {
		Cursor.visible = false;
		yield return new WaitForSeconds (2);
		//PlayerStatus.Instance.RevivePlayer();
	}

	public void FinishGame(){
		Cursor.visible = false;
		hudAnim.SetTrigger("Finish");
	}

    public void HideIngameHud(){
        hudAnim.SetTrigger("HideIngame");
    }

    public void ShowIngameHud(){
        hudAnim.SetTrigger("ShowIngame");
    }

    public void PauseMenuStatus(bool enabled)
    {
        pauseHud_st.SetActive(enabled);
    }
 

    //BOSS METHODS
    public void UpdateBossHP(float max, float current)
    {
        bossHpManager.BossHpAmmount(max, current);
    }
    public void HideBossHP()
    {
        bossHpManager.HideBossHP();
    }
    public void ShowBossHP()
    {
        bossHpManager.ShowBossHP();
    }

    //ITEM METHODS

    void OnItemGet(Item item)
    {
        GameObject newItemPrompt = Instantiate(itemPromptCardPrefab, itemPromptList) as GameObject;
        newItemPrompt.GetComponent<ItemPrompt>().PopulatePrompt(item);
    }

    //MAP METHODS
    void ShowMap()
    {
        Debug.Log("AQUI");
        if(mapCoroutine != null)
            StopCoroutine(mapCoroutine);
        mapCoroutine = _MapSwitch(true);
        StartCoroutine(mapCoroutine);
        
    }
    void HideMap()
    {
        Debug.Log("AQUI");
        if(mapCoroutine != null)
            StopCoroutine(mapCoroutine);
        mapCoroutine = _MapSwitch(false);
        StartCoroutine(mapCoroutine);
    }

    IEnumerator _MapSwitch(bool state)
    {
        if(state)
        {
            while(mapCG.alpha < 1)
            {
                mapCG.alpha += 0.05f;
                yield return null;
            }
        }
        else
        {
            while(mapCG.alpha > 0)
            {
                mapCG.alpha -= 0.05f;
                yield return null;
            }
        }
    }

}
