using UnityEngine;
using System.Collections;

public class editorCheck : MonoBehaviour {

	public GameObject[] enableEditor;
	public GameObject[] disableEditor;
	public GameObject[] enableDevice;
	public GameObject[] disableDevice;



	void Start () {
	
			if (Application.isEditor)

		{

			foreach (GameObject picked in enableEditor) 
				
			{
				picked.SetActive(true);
				
			}

			foreach (GameObject picked in disableEditor) 
				
			{
				picked.SetActive(false);
				
			}


		}

		else

		{
			
			foreach (GameObject picked in enableDevice) 
				
			{
				picked.SetActive(true);
				
			}
			
			foreach (GameObject picked in disableDevice) 
				
			{
				picked.SetActive(false);
				
			}
			
			
		}


	}


}
