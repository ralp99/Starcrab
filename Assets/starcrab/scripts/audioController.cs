using UnityEngine;
using System.Collections;

public class audioController : MonoBehaviour {
    public bool pause;


	void Start () {
	
	}
	
	void Update () {
      AudioListener.pause = (!pause);
        gameObject.SetActive(false);

    }
}
