using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeReactor : MonoBehaviour 
{
	public delegate void OnDodgeEvent();
	public static OnDodgeEvent OnDodgeStart;
	public static OnDodgeEvent OnDodgeEnd;
}
