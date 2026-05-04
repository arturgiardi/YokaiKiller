using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class RumbleController : MonoBehaviour {
	bool playerIndexSet = false;
	static PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	static IEnumerator rumbleCoroutine;

	void Start(){
		//rumbleCoroutine = _Rumble ();
		//StartCoroutine(_Rumble());
	}

	void FixedUpdate()
	{
		// SetVibration should be sent in a slower rate.
		// Set vibration according to triggers
		//GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
	}
	void Update()
	{
		// Find a PlayerIndex, for a single player game
		// Will find the first controller that is connected ans use it
		if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}

		prevState = state;
		state = GamePad.GetState(playerIndex);

	}

	public static void RumbleThatShit(MonoBehaviour mono, float intensity, float time)
	{
		//Debug.Log("Rumbling");
		if(time < .3f)
			time = .3f;

		if (rumbleCoroutine != null)
			mono.StopCoroutine (rumbleCoroutine);
		rumbleCoroutine = _Rumble (intensity, time);
		mono.StartCoroutine (rumbleCoroutine);
	}

	static IEnumerator _Rumble(float intensity, float time){
		GamePad.SetVibration(playerIndex, intensity, intensity);
		yield return new WaitForSecondsRealtime (time);
		GamePad.SetVibration(playerIndex, 0, 0);
	}

}
