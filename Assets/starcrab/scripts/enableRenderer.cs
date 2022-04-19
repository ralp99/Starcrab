using UnityEngine;
using System.Collections;

public class enableRenderer : MonoBehaviour {

	public GameObject[] enable;
	public GameObject[] disable;

	MeshRenderer myRenderer;
	
	void OnEnable () {
		
		foreach (GameObject picked in enable) 
			
		{
			myRenderer = picked.GetComponent<MeshRenderer>();
			//myRenderer.enabled = !myRenderer.enabled;
			myRenderer.enabled = true;

		}
		
		foreach (GameObject picked in disable) 
			
		{
			myRenderer = picked.GetComponent<MeshRenderer>();
			myRenderer.enabled = false;
			
		}	

			gameObject.SetActive(false);
	}
	
	
	
	

}
