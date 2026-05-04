using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObtainedValidation : MonoBehaviour 
{
	//IDs para demo:
	//0: akaiitou sala 3.3
	//1: scroll do poder
	//2: scroll de vida
    //3: lente das almas

	public List<int> obtainedIds = new List<int>();
	public List<int> savedIds= new List<int>();


	public void RestoreIds()
	{
		obtainedIds = new List<int>();
		foreach(int id in savedIds)
		{
			obtainedIds.Add(id);
		}
		
	} 
	public void SaveIds()
	{
		savedIds = new List<int>();
		foreach(int id in obtainedIds)
		{
			savedIds.Add(id);
		}
		
	}

}
