using UnityEngine;
using System.Collections;

public class toggleComponent : MonoBehaviour {

    public Behaviour[] components;

    void OnEnable () {

        foreach (Behaviour picked in components)
        {
            if (picked.enabled = true) picked.enabled = false;
            else picked.enabled = true;
        }
            gameObject.SetActive(false);
    }
	
}
