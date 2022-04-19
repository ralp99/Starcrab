 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemyAdd : MonoBehaviour {

    StarGameManager starGameManagerRef;

	// Use this for initialization
	void Start () {

        starGameManagerRef = StarGameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {

        starGameManagerRef.currentScore = starGameManagerRef.currentScore + 1;
        gameObject.SetActive(false);

	}
}
