using UnityEngine;
using System.Collections;

public class pause : MonoBehaviour {

   public bool resume;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnEnable () {
        if (!resume) Time.timeScale = 0;
        else Time.timeScale = 1;

        gameObject.SetActive(false);


    }
}
