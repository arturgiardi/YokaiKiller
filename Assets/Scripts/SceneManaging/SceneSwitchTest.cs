using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchTest : MonoBehaviour {
	static SceneSwitchTest instance;
	[SerializeField]
	CanvasGroup canvasSwitch;
	[SerializeField]
	GameObject persistent;
	[SerializeField]
	Transform raiko;
	[SerializeField]
	CameraManager manager;
	[SerializeField]
	string findOnStart;

	[SerializeField] string[] areaOneRooms;

	[SerializeField] Animator loadingIndicator;

	Dictionary<string, Scene> loadedScenes = new Dictionary<string, Scene>();


	public delegate void RoomSwitchAction(string roomName);
	public static event RoomSwitchAction OnSwitchRoom;

	[SerializeField] int loadingRooms;
	[SerializeField] int loadedRooms;

	public string currentRoom;
 

	void Update()
	{
		//print("AQUI");
		if(loadingRooms-1 > loadedRooms && !loadingIndicator.GetBool("Loading"))
		{
			Time.timeScale = 0;
			loadingIndicator.SetBool("Loading", true);
		}
		else if (loadingRooms-1 == loadedRooms && loadingIndicator.GetBool("Loading"))
		{
			GameManager.instance.volumeManager.FadeMasterOn();
			Time.timeScale = 1;
			loadingIndicator.SetBool("Loading", false);
		}
	}
 
	void Start(){
		if(instance == null)
			instance = this;
		else
		{
			Destroy(this.gameObject);
			return;
		}
		GameManager.instance.volumeManager.FadeMasterOff();
		loadedScenes[SceneManager.GetActiveScene().name] = SceneManager.GetActiveScene();
		if(OnSwitchRoom != null)
				OnSwitchRoom(SceneManager.GetActiveScene().name);
		DontDestroyOnLoad(persistent);
		foreach(string room in areaOneRooms)
		{
			StartCoroutine(_LoadSceneInBackground(room));
			loadingRooms ++;
		}
		//LoadAdjacentScenes();
		GameManager.instance.cameraController.GetLimits();
	}

	public void FinishGame(){
		AudioManager.instance.ChangeAmbientVolume(0.3f,0);
		HUDManager.hudMan.FinishGame();
		GameManager.instance.controller.DisableController();
	}

	public void StartSwitch(TriggerMessage.MessageContent content)
	{
		StateController.lastPosit = 0;
		currentRoom = content.strings[0];
		StartCoroutine(_SwitchRoom(content));
	}


	IEnumerator _SwitchRoom(TriggerMessage.MessageContent content)
	{
		Time.timeScale = 0;
		System.GC.Collect();
		yield return StartCoroutine(_FadeOut());

		if(content.numbers != null)
		{
			if(content.numbers.Length >= 1)
			{
				if(content.numbers[0] > 0)
					GameManager.instance.cameraController.SetFov(content.numbers[0]);
			}
		}
		

		if(loadedScenes.ContainsKey(content.strings[0]))
		{
			foreach(GameObject rootOBJ in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				rootOBJ.SetActive(false);
			}
			foreach(GameObject rootOBJ in SceneManager.GetSceneByName(content.strings[0]).GetRootGameObjects())
			{
				rootOBJ.SetActive(true);
			}
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(content.strings[0]));
			raiko.GetComponent<CharacterController>().enabled = false;
			yield return null;
			Debug.Log(raiko.position);
			raiko.position = GameObject.FindGameObjectWithTag("Point_" + content.strings[1]).transform.position;	
			yield return null;
			raiko.GetComponent<CharacterController>().enabled = true;
			Debug.Log(raiko.position);
			Time.timeScale = 0;
			GameManager.instance.cameraController.Recenter();
			GameManager.instance.cameraController.GetLimits();
			LoadAdjacentScenes();
			if(OnSwitchRoom != null)
				OnSwitchRoom(content.strings[0]);
			yield return StartCoroutine(_FadeIn());
			Time.timeScale = 1;

		}
		else
		{
			StartCoroutine(_SwitchLevel(content.strings[0], content.strings[1]));
		}
		//
	}
	
	public void LoadScene(string sceneName)
	{
		TriggerMessage.MessageContent customMessage = new TriggerMessage.MessageContent(null, null, new string[]{sceneName, "A"});
		StartSwitch(customMessage);
	}

	IEnumerator _SwitchLevel(string switchTo, string point){
		bool ready = false;
		PoolManager.DisableAll();
		while(!ready){
			canvasSwitch.alpha += 15*Time.deltaTime;
			if(canvasSwitch.alpha >= 1f)
				ready = true;
			yield return null;
		}
		GameManager.instance.controller.DisableController ();
		raiko.position = new Vector3(-999,-999,-999);
		raiko.GetComponent<CharacterController>().enabled = false;
		yield return SceneManager.LoadSceneAsync(switchTo,LoadSceneMode.Additive);
		yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		yield return null;
		Debug.Log(raiko.position);
		raiko.position = GameObject.FindGameObjectWithTag("Point_" + point).transform.position;
		Debug.Log(raiko.position);
		manager.StartSwitching ();
		yield return null;
		manager.Recenter ();
		//yield return new WaitForSeconds (0.2f);
		yield return new WaitForSeconds(0.5f);
		manager.EndSwitching ();
		raiko.GetComponent<CharacterController>().enabled = true;
		GameManager.instance.controller.EnableController ();
		GameManager.instance.SetScene(switchTo);
		while(canvasSwitch.alpha > 0){
			canvasSwitch.alpha -= 15*Time.deltaTime;
			yield return null;
		}
	}

	public void StartLoad(string room){
		StartCoroutine(_LoadInfo(room));
	}

	IEnumerator _LoadInfo(string room){
		GameManager.instance.cameraController.SetFov(60);
		Time.timeScale = 0;
		System.GC.Collect();
		yield return StartCoroutine(_FadeOut());
		if(OnSwitchRoom != null)
				OnSwitchRoom(room);
		currentRoom = room;
		if(loadedScenes.ContainsKey(room))
		{
			foreach(GameObject rootOBJ in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				rootOBJ.SetActive(false);
			}
			foreach(GameObject rootOBJ in SceneManager.GetSceneByName(room).GetRootGameObjects())
			{
				rootOBJ.SetActive(true);
			}
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(room));
			raiko.position = GameObject.FindGameObjectWithTag("Point_SAVE").transform.position;
			GameManager.instance.cameraController.Recenter();
			GameManager.instance.cameraController.GetLimits();
			LoadAdjacentScenes();
			GameManager.instance.controller.Revive ();
			yield return StartCoroutine(_FadeIn());
			Time.timeScale = 1;
		}
	}

	void LoadAdjacentScenes()
	{

		 GameObject[] transitions = GameObject.FindGameObjectsWithTag("SwitchZone");
		 foreach(GameObject go in transitions)
		 {
		 	string sceneName = go.GetComponent<TriggerMessage>().GetStrings()[0];
		 	StartCoroutine(_LoadSceneInBackground(sceneName));
		 }
	}

	IEnumerator _LoadSceneInBackground(string sceneName)
	{
		Debug.Log("Loading scene: " + sceneName);
		if(!loadedScenes.ContainsKey(sceneName))
		{
			yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
			loadedRooms ++;
			foreach(GameObject rootOBJ in SceneManager.GetSceneByName(sceneName).GetRootGameObjects())
			{
				rootOBJ.SetActive(false);
			}
			loadedScenes[sceneName] = SceneManager.GetSceneByName(sceneName);
		}
		Debug.Log("Loaded scenes: " + loadedRooms);
	}

	IEnumerator _FadeOut()
	{
		while(canvasSwitch.alpha < 1){
			canvasSwitch.alpha += 0.03f;
			yield return new WaitForSecondsRealtime(0);
		}
	}

	IEnumerator _FadeIn()
	{
		while(canvasSwitch.alpha > 0){
			canvasSwitch.alpha -=  0.03f;
			yield return new WaitForSecondsRealtime(0);
		}
	}

}
