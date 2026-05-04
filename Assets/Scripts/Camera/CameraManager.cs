using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    [SerializeField] public Transform focusPivot;
    [SerializeField] private Transform shakePivot;
    [SerializeField] private Transform focusTarget;
    [SerializeField] private float smoothnes;
    [SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 minCameraPosition;
	[SerializeField] private Vector3 maxCameraPosition;

    [SerializeField] private bool isFollowingFocus = true;

	[SerializeField] private LayerMask cameraLimitMask;

	[SerializeField] private Camera gameCamera;

	float shakeInt = 0.5f;

    Vector3 velocity;
	bool bounds = true;

	bool switching = false;

	public static RenderTexture finalRender;

	void Start () 
	{
		GetLimits();
		gameCamera.transparencySortMode = TransparencySortMode.Orthographic;
		GetRT();
	}
	
	// Update is called once per frame
	void Update () 
	{

        if (Input.GetKeyDown(KeyCode.F3))
            ChangeShake();


		Debug.DrawLine(new Vector3(0,0,-offset.z) + transform.position , new Vector3(projectDistance(), 0, -offset.z) + transform.position , Color.green);

		if (Time.deltaTime == 0)
			return;
		//RayscastLimits ();

		if (switching)
			return;

        if(focusTarget){
            Vector3 focusPoint = new Vector3(focusTarget.position.x, focusTarget.position.y, focusTarget.position.z) + offset; 
			if (bounds)
			{
				focusPoint = new Vector3(
					Mathf.Clamp(focusPoint.x, minCameraPosition.x, maxCameraPosition.x),
					Mathf.Clamp(focusPoint.y, minCameraPosition.y, maxCameraPosition.y),
					focusPoint.z
				);
			}
            if(isFollowingFocus){
				//focusPivot.position = Vector3.SmoothDamp(focusPivot.position, focusPoint, ref velocity, smoothnes * Time.smoothDeltaTime);
				focusPivot.position = Vector3.Lerp(focusPivot.position, focusPoint, smoothnes * Time.deltaTime);
            }
        }


	}

    public void ChangeFocus(Transform newFocusTarget, float newFocusSpeed = 7){
		smoothnes = newFocusSpeed;
        focusTarget = newFocusTarget;
    }

    public void ShakeCamera(float time, float intensity)
	{
        StartCoroutine(_ShakeCamera(time, intensity));
    }

	public void SetLimits(Vector3 min, Vector3 max){
		minCameraPosition = min;
		maxCameraPosition = max;
	}

    IEnumerator _ShakeCamera(float time, float intensity)
	{
		while (Time.timeScale == 0) {
			yield return null;
		}
		RumbleController.RumbleThatShit (this, intensity, time+0.2f);
        //StartCoroutine(_StutterTime(1));
        time = time * 2;
        //intensity = intensity/120;
		intensity = intensity/5;
        float timer = 0;
		bool toRight = false;
		while(timer <= time){
			timer += Time.deltaTime;
			//Quaternion newRotation = Quaternion.Euler(Vector3.zero);
            Vector3 newPosition = new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f), 0) * shakeInt;
			//if(toRight)
				//newRotation = Quaternion.Euler(new Vector3(0,0,intensity));
			//else
				//newRotation = Quaternion.Euler(new Vector3(0,0,-intensity));
            float reachTimer = 0.01f;
            while (reachTimer > 0)
            {
				//shakePivot.localRotation = Quaternion.Lerp(shakePivot.localRotation, newRotation, 60 * Time.deltaTime);
				shakePivot.localPosition = Vector3.Lerp(shakePivot.localPosition, newPosition, 60 * Time.deltaTime);
                reachTimer -= Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }
			toRight = !toRight;
            yield return null;
		}
        shakePivot.localRotation = Quaternion.Euler(Vector3.zero);
		shakePivot.localPosition = Vector3.zero;
		
    }

	public IEnumerator _StutterTime(float oldTime){
		//yield return new WaitForSecondsRealtime (0.1f);
		var oldTS = Time.timeScale;
		Time.timeScale = 0.1f;
		yield return new WaitForSecondsRealtime (0.1f);
		Time.timeScale = oldTime;
	}

	[ContextMenu ("Set min")]
	void SetMin(){
		minCameraPosition = focusPivot.position;
	}
	[ContextMenu ("Set max")]
	void SetMax(){
		maxCameraPosition = focusPivot.position;
	}

	public void Recenter(){
		//recentering = true;
		Vector3 focusPoint = new Vector3(focusTarget.position.x, focusTarget.position.y, focusTarget.position.z) + offset; 
		focusPivot.position = focusPoint;
		//StartCoroutine(HideOffscreen ());
	}

	public void StartSwitching(){
		switching = true;
	}

	public void EndSwitching(){
		switching = false;
	}

	void RayscastLimits(){
		//check left
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0,0.5f,0));
		RaycastHit hit = new RaycastHit ();

		if (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
            focusPivot.position = focusPivot.position + Vector3.right / 5;
            minCameraPosition.x = focusPivot.position.x + 5;
        } 
		else if(Physics.Raycast (ray.origin + Vector3.left/4, ray.direction, out hit, 150, cameraLimitMask)){
            minCameraPosition.x =  focusPivot.position.x;
		}
		else if (!Physics.Raycast (ray.origin + Vector3.left/4, ray.direction, out hit, 150, cameraLimitMask)){
			minCameraPosition.x = -999;
		}

		ray = Camera.main.ViewportPointToRay(new Vector3(1,0.5f,0));
		Debug.DrawRay (ray.origin, ray.direction);
		if (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			focusPivot.position = focusPivot.position + Vector3.left / 5;
			maxCameraPosition.x = transform.position.x - 5;
        } 
		else if(Physics.Raycast (ray.origin + Vector3.right/4, ray.direction, out hit, 150, cameraLimitMask)){
			maxCameraPosition.x = transform.position.x;
		}
		else if (!Physics.Raycast (ray.origin + Vector3.right/4, ray.direction, out hit, 150, cameraLimitMask)){
			maxCameraPosition.x = 999;
		}

		ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,1,0));
		Debug.DrawRay (ray.origin, ray.direction);
		if (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			focusPivot.position = focusPivot.position + Vector3.down / 5;
			maxCameraPosition.y = transform.position.y - 5;
        } 
		else if(Physics.Raycast (ray.origin + Vector3.up/4, ray.direction, out hit, 150, cameraLimitMask)){
			maxCameraPosition.y = transform.position.y;
		}
		else if (!Physics.Raycast (ray.origin + Vector3.up/4, ray.direction, out hit, 150, cameraLimitMask)){
			maxCameraPosition.y = 999;
		}
		ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0,0));
		Debug.DrawRay (ray.origin, ray.direction);
		if (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			focusPivot.position = focusPivot.position + Vector3.up / 5;
			minCameraPosition.y = transform.position.y + 5;
		} 
		else if(Physics.Raycast (ray.origin + Vector3.down/4, ray.direction, out hit, 150, cameraLimitMask)){
			minCameraPosition.y = transform.position.y;
		}
		else if (!Physics.Raycast (ray.origin + Vector3.down/4, ray.direction, out hit, 150, cameraLimitMask)){
			minCameraPosition.y = -999;
		}

	}

	IEnumerator HideOffscreen(){
		yield return new WaitForSeconds(0.1f);
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0,0.5f,0));
		RaycastHit hit = new RaycastHit ();
		while (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			print ("Tried First");
			transform.position = transform.position + Vector3.right/5;
			ray = Camera.main.ViewportPointToRay(new Vector3(0,0.5f,0));
			//yield return null;
		} 
		ray = Camera.main.ViewportPointToRay(new Vector3(1,0.5f,0));
		Debug.DrawRay (ray.origin, ray.direction);
		while (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			transform.position = transform.position + Vector3.left/5;
			ray = Camera.main.ViewportPointToRay(new Vector3(1,0.5f,0));
			//yield return null;
		}

		ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,1,0));
		Debug.DrawRay (ray.origin, ray.direction);
		while (Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			transform.position = transform.position + Vector3.down/5;
			ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,1,0));
			//yield return null;
		}
		ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0,0));
		Debug.DrawRay (ray.origin, ray.direction);
		while(Physics.Raycast (ray, out hit, 150, cameraLimitMask)) {
			transform.position = transform.position + Vector3.up/5;
			ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0,0));
			//yield return null;
		} 
				

	}

	public void GetLimits()
	{
		float maxX = -9999;
		float maxY = -9999;
		float minX = 9999;
		float minY = 9999;
		GameObject[] limitators = GameObject.FindGameObjectsWithTag("CameraLimits");
		foreach(GameObject limitator in limitators)
		{
			if(limitator.transform.position.x - projectDistance() > maxX)
				maxX = limitator.transform.position.x - projectDistance();
			if(limitator.transform.position.y - projectVerticalDistance() > maxY)
				maxY = limitator.transform.position.y - projectVerticalDistance();
			if(limitator.transform.position.x + projectDistance() < minX)
				minX = limitator.transform.position.x + projectDistance();
			if(limitator.transform.position.y + projectVerticalDistance() < minY)
				minY =  limitator.transform.position.y + projectVerticalDistance();
		}

		minCameraPosition = new Vector3(minX, minY, 0);
		maxCameraPosition = new Vector3(maxX, maxY, 0);

		Vector3 focusPoint = new Vector3(
					Mathf.Clamp(focusPivot.position.x, minCameraPosition.x, maxCameraPosition.x),
					Mathf.Clamp(focusPivot.position.y, minCameraPosition.y, maxCameraPosition.y),
					focusPivot.position.z
		);
		focusPivot.position = focusPoint;
		

	}

	public float projectDistance()
	{
		float focalDistance = Mathf.Abs(offset.z + 0.3f);
		Vector3 cameraDirection = GetComponent<Camera>().ViewportPointToRay(new Vector3(1,0.5f,0)).direction;
		float viewSlope = cameraDirection.z/cameraDirection.x;
		float xPoint = focalDistance/viewSlope;
		float distance = Vector3.Distance(new Vector3(0,0,focalDistance), new Vector3(xPoint,0,focalDistance));
		return distance+2;
	}

	public float projectVerticalDistance()
	{
		float focalDistance = Mathf.Abs(offset.z + 0.3f);
		Vector3 cameraDirection = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f,1,0)).direction;
		float viewSlope = cameraDirection.z/cameraDirection.y;
		float yPoint = focalDistance/viewSlope;
		float distance = Vector3.Distance(new Vector3(0,0,focalDistance), new Vector3(0,yPoint,focalDistance));
		return distance+2;
	}

	public void SetFov(float newFov)
	{
		gameCamera.fieldOfView = newFov;
	}

    void ChangeShake()
    {
        Debug.Log("changing sslider");
        if (shakeInt == 2f)
            shakeInt = 1.5f;
        else if(shakeInt == 1.5f)
            shakeInt = 0;
        else if (shakeInt == 0)
            shakeInt = 2f;
        Debug.Log(shakeInt);

    }

	void GetRT()
	{
		gameCamera.targetTexture = GraphicsManager.gameRenderTexture;
	}
}
