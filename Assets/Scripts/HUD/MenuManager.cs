using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable] public class MainMenu
{
	public TMP_Text pow;
	public TMP_Text critChan;
	public TMP_Text critMulti;
	public TMP_Text fire;
	public TMP_Text water;
	public TMP_Text earth;
	public TMP_Text thunder;
	public TMP_Text level;
	public TMP_Text spririts;
	public TMP_Text curHealth;
	public TMP_Text maxHealth;
	public Image healthBar;

}

[System.Serializable] public class MenuWindow
{
	public GameObject firstElement;
	public CanvasGroup canvasGroup;
	public GameObject lastSelected;
	//public RectTransform view;
	//public LayoutGroup layoutGroup;
	//public Vector2 fitInLayout;
	//public int currentStep;
}

[System.Serializable] public class MenuGrid
{
	public RectTransform gridView;
	public LayoutGroup layoutGroup;
	public Vector2 fitInLayout;
	public int currentStep;
}

[System.Serializable] public class EquipmentMenu
{
	public CanvasGroup weaponSelectionCG;
	public CanvasGroup artifactSelectionCG;
	
	public CanvasGroup weaponListCG;
	public CanvasGroup artifactListCG;
	public CanvasGroup subweaponListCG;

	public Transform weaponListParent;
	public Transform artifcatListParent;
	public Transform subWeaponListParent;

	public CanvasGroup statsCG;

	public GameObject lastSelectionBtn;

	public GameObject lastSelectedWeapon;
	public GameObject lastSelectedArtifact;
	public GameObject lastSelectedSubweapon;
	[HideInInspector] public bool changingWeapon;
	[HideInInspector] public bool changingArtifact;
	[HideInInspector] public bool changingSubweapon;
}

public class MenuManager : MonoBehaviour 
{

	[SerializeField] AudioSource menuAudioSource;
	[SerializeField] AudioClip[] clips; //[0] -> accept [1] -> refuse [2] -> move [3] -> equip [4] -> special
	[SerializeField] bool menuOpen = false;
	[SerializeField] CanvasGroup menuCanvas;
	[SerializeField] AudioMixer mixer;
	//[SerializeField] AudioMixerSnapshot[] snapshots;
	[SerializeField] GameObject firstButton;
	[SerializeField] MenuWindow mainWindow;
	[SerializeField] MenuWindow inventoryWindow;
	[SerializeField] MenuWindow equipmentWindow;
	[SerializeField] MenuWindow currentWindow;
	[SerializeField] MenuGrid inventoryGrid;
	[SerializeField] MenuGrid equipmentGrid;
	[SerializeField] MenuGrid artifactGrid;

	[SerializeField] EquipmentMenu equipmentMenu;
	[SerializeField] MainMenu mainMenu;

	MenuGrid currentGrid;
	[SerializeField] List<MenuWindow> navigationStack = new List<MenuWindow>();

	[SerializeField] MenuWindow levelUpMenu;

	[SerializeField] Animator deathMenu;

	IEnumerator snapCoroutine;

	Vector2 fitInGrid;

	void OnEnable()
	{
		inventoryGrid.fitInLayout = CalculateGridFit(inventoryGrid.gridView, (GridLayoutGroup) inventoryGrid.layoutGroup);
		equipmentGrid.fitInLayout = CalculateGridFit(equipmentGrid.gridView, (GridLayoutGroup) equipmentGrid.layoutGroup);
		//artifactGrid.fitInLayout = CalculateGridFit(artifactGrid.gridView, (GridLayoutGroup) artifactGrid.layoutGroup);
		
		currentWindow = mainWindow;
		FindObjectOfType<CustomInputModule>().RegisterEvents();
		
		InputManager.OnPressB += ReturnWindow;
		PlayerStats.instance.OnChangeHP += OnHpChange;
		PlayerStats.OnStatsChange += OnStatsChange;
		PlayerStats.instance.OnEarnXP += OnXpChange;
	}

	public void TurnMenu()
	{
		if(menuOpen)
		{

			menuAudioSource.PlayOneShot(clips[1]);
			EventSystem.current.SetSelectedGameObject(null);
			menuOpen = false;
			menuCanvas.interactable = false;
			menuCanvas.blocksRaycasts = false;
			menuCanvas.alpha = 0;
			GameManager.instance.volumeManager.FadeMusicVolume(0, 15);
			
			navigationStack.Clear();

			if(equipmentMenu.changingWeapon)
				FinishWeaponSelection();
			if(equipmentMenu.changingArtifact)
				FinishArtifactSelection();
			if(equipmentMenu.changingSubweapon)
				FinishSubweaponSelection();

			equipmentMenu.lastSelectedArtifact = null;
			equipmentMenu.lastSelectedWeapon = null;
			equipmentMenu.lastSelectedSubweapon = null;

		}
		else
		{
			menuOpen = true;
			menuCanvas.interactable = true;
			menuCanvas.blocksRaycasts = true;
			menuCanvas.alpha = 1;
			GameManager.instance.volumeManager.FadeMusicVolume(-15, 15);
			
			AccessWindow(mainWindow);
			OnStatsChange();
			
		}
	}

	//IEnumerator _SelectMenuElement(GameObject element)
	//{
		
		//EventSystem.current.SetSelectedGameObject(element);
	//}

	Vector2 CalculateGridFit(RectTransform view, GridLayoutGroup grid)
	{
		var wi = view.rect.width;
		var he = view.rect.height;
		var siz = grid.cellSize;
		//print(wi);
		//print(he);
		//print(siz);
		var fitInRow = (int)((wi - grid.padding.left - grid.padding.right + (grid.spacing.x*2))/(siz.x + grid.spacing.x));
		var fitInHeight = (int)((he - grid.padding.top - grid.padding.bottom + (grid.spacing.y*2))/(siz.y + grid.spacing.y));
		//print(fitInRow);
		//print(fitInHeight);
		return new Vector2(fitInRow, fitInHeight);
	}

	public void SnapTo(UnityEngine.EventSystems.BaseEventData eventData)
    {
		currentGrid.fitInLayout = CalculateGridFit(currentGrid.gridView, (GridLayoutGroup)currentGrid.layoutGroup);
		int thisRow = (int)(eventData.selectedObject.transform.GetSiblingIndex()/currentGrid.fitInLayout.x) + 1;
		int maxRows = (int)((eventData.selectedObject.transform.parent.childCount-1)/currentGrid.fitInLayout.x);
		int maxSteps = maxRows+2-(int)currentGrid.fitInLayout.y;
	
		if(maxSteps < 1)
			maxSteps = 1;

		if(thisRow > currentGrid.currentStep + (currentGrid.fitInLayout.y-1))
		{
			currentGrid.currentStep = thisRow - (int)currentGrid.fitInLayout.y + 1;
		}
		else if(thisRow < currentGrid.currentStep)
		{
			currentGrid.currentStep = thisRow;
		}

		if(currentGrid.currentStep < 1)
			currentGrid.currentStep = 1;
		float scrollNormalizedPosition = -(1/((float)maxSteps-1))*((float)currentGrid.currentStep-1)+1;


		if(snapCoroutine != null)
			StopCoroutine(snapCoroutine);
		snapCoroutine = _snap(scrollNormalizedPosition);
		StartCoroutine(snapCoroutine);
    }

	IEnumerator _snap(float targetScrollPosition)
	{
		
		while(currentGrid.gridView.GetComponent<ScrollRect>().verticalNormalizedPosition != targetScrollPosition)
		{
			currentGrid.gridView.GetComponent<ScrollRect>().verticalNormalizedPosition=Mathf.MoveTowards(currentGrid.gridView.GetComponent<ScrollRect>().verticalNormalizedPosition, targetScrollPosition, 2f*Time.unscaledDeltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

	public void AccessInventory()
	{
		AccessWindow(inventoryWindow);
		currentGrid = inventoryGrid;
		// CalculateGridFit(currentGrid.gridView, (GridLayoutGroup) currentGrid.layoutGroup);
		List<GameObject> cards = GameManager.instance.inventoryManager.GetAllItemCards();
		foreach(GameObject card in cards)
		{
			card.transform.SetParent(inventoryGrid.layoutGroup.transform,false);
			EventTrigger trigger = card.GetComponent<EventTrigger>();
        	EventTrigger.Entry entry = new EventTrigger.Entry();
        	entry.eventID = EventTriggerType.Select;
        	entry.callback.AddListener((data) => { SnapTo((UnityEngine.EventSystems.BaseEventData)data); });
        	trigger.triggers.Add(entry);
		}
		if(cards.Count > 0)
			inventoryWindow.firstElement = cards[0];
		CalculateGridFit(inventoryGrid.gridView, (GridLayoutGroup) inventoryGrid.layoutGroup);
	}

	public void AccessEquipments()
	{
		AccessWindow(equipmentWindow);
		//currentGrid = equipmentGrid;
		//CalculateGridFit(currentGrid.gridView, (GridLayoutGroup) currentGrid.layoutGroup);
		GetArtifactsList();
		GetWeaponsList();
	}


	void GetWeaponsList()
	{
		List<GameObject> weaponCards = GameManager.instance.inventoryManager.GetWeaponCards();
		foreach(GameObject card in weaponCards)
		{
			card.transform.SetParent(equipmentGrid.layoutGroup.transform,false);
			EventTrigger trigger = card.GetComponent<EventTrigger>();
        	EventTrigger.Entry entry = new EventTrigger.Entry();
        	entry.eventID = EventTriggerType.Select;
        	entry.callback.AddListener((data) => { SnapTo((UnityEngine.EventSystems.BaseEventData)data); });
        	trigger.triggers.Add(entry);
		}
		CalculateGridFit(equipmentGrid.gridView, (GridLayoutGroup) equipmentGrid.layoutGroup);
	}
	void GetArtifactsList()
	{
		List<GameObject> artifactCards = GameManager.instance.inventoryManager.GetArtifactCards();
		foreach(GameObject card in artifactCards)
		{
			ItemCard cardData = card.GetComponent<ItemCard>();

			card.transform.SetParent(artifactGrid.layoutGroup.transform,false);
			EventTrigger trigger = card.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Select;
			entry.callback.AddListener((data) => { SnapTo((UnityEngine.EventSystems.BaseEventData)data); });
			trigger.triggers.Add(entry);
			
		}
		CalculateGridFit(artifactGrid.gridView, (GridLayoutGroup) artifactGrid.layoutGroup);
	}

	public void AccessMain()
	{
		AccessWindow(mainWindow);
	}

	IEnumerator _AccessCoroutine(MenuWindow window, bool returning = false)
	{
		EventSystem.current.SetSelectedGameObject(null);
		yield return null;
		EventSystem.current.SetSelectedGameObject(null);
		yield return null;
		currentWindow.canvasGroup.alpha = 0;
		currentWindow.canvasGroup.interactable = false;
		currentWindow.canvasGroup.blocksRaycasts = false;
		currentWindow = window;
		currentWindow.canvasGroup.alpha = 1;
		currentWindow.canvasGroup.interactable = true;
		currentWindow.canvasGroup.blocksRaycasts = true;
		yield return null;
		yield return null;
		if(!returning)
			EventSystem.current.SetSelectedGameObject(currentWindow.firstElement);
		else
			EventSystem.current.SetSelectedGameObject(currentWindow.lastSelected);
	}
	void AccessWindow(MenuWindow window)
	{
		if(navigationStack.Count > 0)
			navigationStack[navigationStack.Count-1].lastSelected = EventSystem.current.currentSelectedGameObject;
		GameManager.instance.inventoryManager.PoolCardsIn();
		menuAudioSource.PlayOneShot(clips[0]);
		window.lastSelected = EventSystem.current.currentSelectedGameObject;
		navigationStack.Add(window);
		StartCoroutine(_AccessCoroutine(window));
	}

	public void ReturnWindow()
	{
		if(!menuOpen)
			return;
		if(navigationStack.Count > 1)
		{
			menuAudioSource.PlayOneShot(clips[1]);
			navigationStack.RemoveAt(navigationStack.Count-1);
			StartCoroutine(_AccessCoroutine(navigationStack[navigationStack.Count-1], true));
		}
		else
		{
			GameManager.instance.PauseGame();
			navigationStack.Clear();
			EventSystem.current.SetSelectedGameObject(null);
			menuOpen = false;
			menuCanvas.interactable = false;
			menuCanvas.blocksRaycasts = false;
			menuCanvas.alpha = 0;
			GameManager.instance.volumeManager.FadeMusicVolume(0, 15);
			//mixer.TransitionToSnapshots(snapshots, new float[2]{0,1}, 0);
		}
	}
	
	public void OnHoover()
	{
		menuAudioSource.PlayOneShot(clips[2]);
	}

	public void OnHighlightWeaponSlot()
	{
		equipmentMenu.weaponListCG.alpha = 1;
		equipmentMenu.artifactListCG.alpha = 0;
		currentGrid = equipmentGrid;

		ItemDescName.OnItemSelect?.Invoke(PlayerStatsController.instance.currentWeapon);

		CalculateGridFit(currentGrid.gridView, (GridLayoutGroup) currentGrid.layoutGroup);
	}

	public void OnHighlightArtifactSlot()
	{
		equipmentMenu.weaponListCG.alpha = 0;
		equipmentMenu.artifactListCG.alpha = 1;
		currentGrid = artifactGrid;
		CalculateGridFit(currentGrid.gridView, (GridLayoutGroup) currentGrid.layoutGroup);
	}

	public void StartWeaponSelection()
	{
		equipmentMenu.lastSelectionBtn = EventSystem.current.currentSelectedGameObject;

		InputManager.OnPressB -= ReturnWindow;
		InputManager.OnPressB += FinishWeaponSelection;
		InputManager.OnPressB += PlayCanceled;

		ItemCard.onPickItem += ChangeWeapon;

		equipmentMenu.changingWeapon = true;

		CloseSelection();

		equipmentMenu.weaponListCG.interactable = true;
		equipmentMenu.weaponListCG.blocksRaycasts = true;
		equipmentMenu.weaponListCG.alpha = 1;

		if(equipmentMenu.lastSelectedWeapon != null)
			EventSystem.current.SetSelectedGameObject(equipmentMenu.lastSelectedWeapon);
		else
			EventSystem.current.SetSelectedGameObject(equipmentGrid.layoutGroup.transform.GetChild(0).gameObject);

	}

	public void StartArtifactSelection(int slot)
	{
		equipmentMenu.lastSelectionBtn = EventSystem.current.currentSelectedGameObject;

		PlayerStatsController.instance.selectedArtifactSlot = slot;

		InputManager.OnPressB -= ReturnWindow;
		InputManager.OnPressB += FinishArtifactSelection;
		InputManager.OnPressB += PlayCanceled;

		ItemCard.onPickItem += ChangeArtifact;

		equipmentMenu.changingArtifact = true;

		CloseSelection();

		equipmentMenu.artifactListCG.interactable = true;
		equipmentMenu.artifactListCG.blocksRaycasts = true;
		equipmentMenu.artifactListCG.alpha = 1;

		if(equipmentMenu.lastSelectedArtifact != null)
		{
			if(equipmentMenu.lastSelectedArtifact.activeInHierarchy)
			{
				
				EventSystem.current.SetSelectedGameObject(equipmentMenu.lastSelectedArtifact);
			}
			else
			{
				equipmentMenu.lastSelectedArtifact = null;
				EventSystem.current.SetSelectedGameObject(artifactGrid.layoutGroup.transform.GetChild(0).gameObject);
			}
		}
			
		else
			EventSystem.current.SetSelectedGameObject(artifactGrid.layoutGroup.transform.GetChild(0).gameObject);

	}

	void CloseSelection()
	{
		menuAudioSource.PlayOneShot(clips[0]);

		equipmentMenu.statsCG.interactable = true;
		equipmentMenu.statsCG.blocksRaycasts = true;
		equipmentMenu.statsCG.alpha = 1;

		equipmentMenu.weaponSelectionCG.interactable = false;
		equipmentMenu.weaponSelectionCG.blocksRaycasts = false;
		equipmentMenu.weaponSelectionCG.alpha = 0;

		equipmentMenu.artifactSelectionCG.interactable = false;
		equipmentMenu.artifactSelectionCG.blocksRaycasts = false;
		equipmentMenu.artifactSelectionCG.alpha = 0;
	}	

	void OpenSelection()
	{
		equipmentMenu.weaponSelectionCG.interactable = true;
		equipmentMenu.weaponSelectionCG.blocksRaycasts = true;
		equipmentMenu.weaponSelectionCG.alpha = 1;

		equipmentMenu.artifactSelectionCG.interactable = true;
		equipmentMenu.artifactSelectionCG.blocksRaycasts = true;
		equipmentMenu.artifactSelectionCG.alpha = 1;

		equipmentMenu.statsCG.interactable = false;
		equipmentMenu.statsCG.blocksRaycasts = false;
		equipmentMenu.statsCG.alpha = 0;
	}

	public void FinishWeaponSelection()
	{
		equipmentMenu.lastSelectedWeapon = EventSystem.current.currentSelectedGameObject;

		InputManager.OnPressB -= FinishWeaponSelection;
		InputManager.OnPressB -= PlayCanceled;
		InputManager.OnPressB += ReturnWindow;

		ItemCard.onPickItem -= ChangeWeapon;

		equipmentMenu.changingWeapon = false;

		equipmentMenu.weaponListCG.interactable = false;
		equipmentMenu.weaponListCG.blocksRaycasts = false;
		equipmentMenu.weaponListCG.alpha = 1;

		OpenSelection();

		EventSystem.current.SetSelectedGameObject(equipmentMenu.lastSelectionBtn);
	}
	public void FinishArtifactSelection()
	{
		equipmentMenu.lastSelectedArtifact = EventSystem.current.currentSelectedGameObject;

		InputManager.OnPressB -= FinishArtifactSelection;
		InputManager.OnPressB -= PlayCanceled;
		InputManager.OnPressB += ReturnWindow;

		ItemCard.onPickItem -= ChangeArtifact;
		
		equipmentMenu.changingArtifact = false;

		equipmentMenu.artifactListCG.interactable = false;
		equipmentMenu.artifactListCG.blocksRaycasts = false;
		equipmentMenu.artifactListCG.alpha = 1;

		OpenSelection();

		EventSystem.current.SetSelectedGameObject(equipmentMenu.lastSelectionBtn);
	}
	public void FinishSubweaponSelection()
	{
		equipmentMenu.lastSelectedSubweapon = EventSystem.current.currentSelectedGameObject;

		InputManager.OnPressB -= FinishSubweaponSelection;
		InputManager.OnPressB += ReturnWindow;
		equipmentMenu.changingSubweapon = false;

		OpenSelection();

		EventSystem.current.SetSelectedGameObject(equipmentMenu.lastSelectionBtn);
	}

	public void ChangeWeapon(Item newWeapon)
	{
		menuAudioSource.PlayOneShot(clips[3]);
		PlayerStatsController.instance.ChangeWeapon(newWeapon);
		FinishWeaponSelection();
	}

	public void ChangeArtifact(Item newArtifact)
	{
		menuAudioSource.PlayOneShot(clips[3]);
		PlayerStatsController.instance.ChangeArtifact(newArtifact);
		FinishArtifactSelection();
	}

	void PlayCanceled()
	{
		menuAudioSource.PlayOneShot(clips[1]);
	}

	void OnHpChange(float maxHP, float currentHP)
	{
		mainMenu.maxHealth.text = maxHP.ToString();
		mainMenu.curHealth.text = currentHP.ToString();
		mainMenu.healthBar.fillAmount = currentHP/maxHP;
	}

	void OnXpChange(float xp, float perToNext)
	{
		//mainMenu.spririts.text = string.Format("{0:000000}/{1:111111}", PlayerStats.instance.experience, PlayerStats.instance.XpToNextLevel(PlayerStats.instance.level));
		//mainMenu.level.text = string.Format("{0:000}",PlayerStats.instance.level.ToString());
	}

	void OnStatsChange()
	{
		mainMenu.maxHealth.text = PlayerStats.instance.maxHealth.ToString();
		mainMenu.curHealth.text = PlayerStats.instance.currentHealth.ToString();
		mainMenu.healthBar.fillAmount = PlayerStats.instance.currentHealth/PlayerStats.instance.maxHealth;

		mainMenu.pow.text = PlayerStats.instance.damage.power.ToString();
		//mainMenu.critChan.text = PlayerStats.instance.damage.criticalChance.ToString();
		mainMenu.critChan.text = string.Format("{0:F1}", PlayerStats.instance.damage.criticalChance/10);
		mainMenu.critMulti.text = PlayerStats.instance.damage.criticalMultiplier.ToString();

		mainMenu.fire.text = (PlayerStats.instance.damage.elementalInfo.fire*3).ToString();
		mainMenu.water.text = (PlayerStats.instance.damage.elementalInfo.fire*3).ToString();
		mainMenu.earth.text = (PlayerStats.instance.damage.elementalInfo.fire*3).ToString();
		mainMenu.thunder.text = (PlayerStats.instance.damage.elementalInfo.fire*3).ToString();

		//print( string.Format("{0:000000}/{1:000000}", PlayerStats.instance.experience, PlayerStats.instance.XpToNextLevel(PlayerStats.instance.level)));
		//mainMenu.spririts.text = string.Format("{0:000000}/{1:000000}", PlayerStats.instance.experience, PlayerStats.instance.XpToNextLevel(PlayerStats.instance.level));
		//mainMenu.level.text = string.Format("{0:000}",PlayerStats.instance.level.ToString());
		mainMenu.spririts.text = PlayerStats.instance.experience.ToString() + "/" + PlayerStats.instance.XpToNextLevel(PlayerStats.instance.level).ToString();
		mainMenu.level.text = PlayerStats.instance.level.ToString("000");

	}

	public void TurnLvlUpMenu(bool state)
	{
		StartCoroutine(_TurnLvlUpMenu(state));
	}

	public void SelectLvlMenuButton()
	{

	}

	IEnumerator _TurnLvlUpMenu(bool state)
	{
		if(state)
		{
			yield return new WaitForSecondsRealtime(1f);
			levelUpMenu.canvasGroup.GetComponent<Animator>().SetTrigger("Switch");
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			yield return new WaitForSecondsRealtime(0.3f);
			levelUpMenu.canvasGroup.interactable = true;
			levelUpMenu.canvasGroup.blocksRaycasts = true;
			yield return null;
			EventSystem.current.SetSelectedGameObject(levelUpMenu.firstElement);
			yield return null;
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			menuAudioSource.PlayOneShot(clips[4]);
			levelUpMenu.canvasGroup.interactable = false;
			levelUpMenu.canvasGroup.blocksRaycasts = false;
			levelUpMenu.canvasGroup.GetComponent<Animator>().SetTrigger("Switch");	
			yield return null;
			levelUpMenu.canvasGroup.alpha = 0;
			yield return null;
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
		}
		
	}

	public void SwitchDeathMenu()
	{
		deathMenu.SetTrigger("Trigger");
	}
		
}
