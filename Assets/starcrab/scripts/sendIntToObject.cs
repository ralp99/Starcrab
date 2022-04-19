using UnityEngine;
using System.Collections;

public class sendIntToObject : MonoBehaviour {

	public GameObject objectSend;
	public string method;
	public int integer;
	//public bool disable;
	//public string message;
	
	void OnEnable () {
		


		//lastHitByRay.SendMessage ("animateNoticed", false);
//		objectSend.SendMessage (method, value);
	//	objectSend.SendMessage (method, integer, disable);

		//	objectSend.SendMessage (method, disable);
		objectSend.SendMessage (method, integer);

		//	objectSend.SendMessage (message);

		
		gameObject.SetActive(false);
		
	}
}
