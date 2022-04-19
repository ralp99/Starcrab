using UnityEngine;
using System.Collections;

public class enableComponentCollider : MonoBehaviour {

	public GameObject[] objectUsing;
	//public string componentAffect;
	Collider myCollider;
	public bool hide;
	
	void Update () {
		
		foreach (GameObject picked in objectUsing) 
		{
			myCollider = picked.GetComponent<Collider> ();
			if (!hide) myCollider.enabled = true;
			else myCollider.enabled = false;
		}

		gameObject.SetActive(false);
		
	}
}
