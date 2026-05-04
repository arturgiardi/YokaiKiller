using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Stats/Defense")]
public class DefenseInfo : ScriptableObject
{
	public float armor;
	public float postitiveVariation;
	public float negativeVariation;
	public ElementalInfo elementalInfo;
}