using UnityEngine;
using System.Collections;

public class orient : MonoBehaviour {
    public GameObject source, affectObject;

	
	
	void OnEnable () {

        affectObject.transform.localRotation = source.transform.localRotation;

        gameObject.SetActive(false);

    }
}
