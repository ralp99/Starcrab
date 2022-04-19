using UnityEngine;
using System.Collections;

public class animBoolean : MonoBehaviour {
	
	public GameObject[] animObject;
	public string boolName = "";
	public bool newState;
	
	void OnEnable () {



		foreach (GameObject picked in animObject) 
		{
			picked.GetComponent<Animator>().SetBool(boolName,newState);

		}



			gameObject.SetActive (false);
		
	}
	
	


}


