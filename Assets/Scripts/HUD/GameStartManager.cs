using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameStartManager : MonoBehaviour
{
	public Animator menu;
    public GameObject selector;

    public GameObject startOption;
    public GameObject continueOption;
    public GameObject optionsOption; // =p

    public GameObject toDestroy;

	public AudioSource effectSource;
	public AudioSource musicSource;

	public AudioClip selectEffect;

    private int selectedIndex = 1;

    void Start()
    {
        UpdateSelectorPosition();
    }

    public void NewGame()
    {	
        if(GameManager.instance != null)
            GameManager.instance = null;
        if(PlayerStatsController.instance != null)
            PlayerStatsController.instance = null;
        if(HUDManager.hudMan != null)
            HUDManager.hudMan = null;
        if(PlayerFXFeedback.instance != null)
            PlayerFXFeedback.instance = null;


        Destroy(toDestroy);
		Cursor.visible = false;
		menu.SetTrigger("Select");
		effectSource.PlayOneShot(selectEffect);
		StartCoroutine(_LoadFirstLevel());
		StartCoroutine(_LowerVolume());
    }

	IEnumerator _LoadFirstLevel(){
		yield return new WaitForSeconds(3);

		SceneManager.LoadScene("Area1_Room1");
	}

	IEnumerator _LowerVolume(){
		while(musicSource.volume > 0.05f){
			musicSource.volume -= 15*Time.deltaTime;
			yield return null;
		}
		musicSource.volume = 0;
	}

    public void LoadGame()
    {
		Cursor.visible = false;
        //PlayerStatus.Load();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

	void Update(){
		/*if (Input.GetKeyDown (KeyCode.F1)) {
			print ("reiniciando");
			SceneManager.LoadScene (0);
		}
		if (Input.GetKeyDown (KeyCode.F2)) {
			print ("reiniciando");
			SceneManager.LoadScene (1);
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			menu.SetTrigger("TurnMenu");
		}*/

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     if (selectedIndex < 3)
        //     {
        //         selectedIndex++;
        //         UpdateSelectorPosition();
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     if (selectedIndex > 1)
        //     {
        //         selectedIndex--;
        //         UpdateSelectorPosition();
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1"))
        // {
        //     HandleSelection();
        // }

        if(EventSystem.current.currentSelectedGameObject == null &&
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            EventSystem.current.SetSelectedGameObject(startOption);
        }
	}

    void UpdateSelectorPosition()
    {
        float newYValue = 0;
        switch (selectedIndex)
        {
            case 1:
                newYValue = startOption.transform.localPosition.y;
                break;
            case 2:
                newYValue = continueOption.transform.localPosition.y;
                break;
            case 3:
                newYValue = optionsOption.transform.localPosition.y;
                break;
        }

        selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, newYValue, selector.transform.localPosition.z);
    }

    void HandleSelection()
    {
        switch (selectedIndex)
        {
            case 1:
                NewGame();
                break; 
            case 2:
                LoadGame();
                break;
            case 3:
                // TODO: Options
                break;
        }
    }
}
