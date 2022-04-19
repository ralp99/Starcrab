using UnityEngine;
using System.Collections;

public class changeMaterial : MonoBehaviour {

	public GameObject[] objects;
	public Material material;

	void OnEnable () {
	
		foreach (GameObject picked in objects) 
		{

			//THIS DID THE INVERSE - GRABBED AN INSTANCE OF THE MATERIAL. USEFUL FOR GARBBING OFF ONE AND ASSIGNING TO SOMETHING ELSE
				//	material = picked.GetComponent<Renderer> ().material;
			picked.GetComponent<Renderer> ().material = material;
		
		}
		gameObject.SetActive (false);
	}
}
