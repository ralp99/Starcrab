using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchHealth : MonoBehaviour {

    public string floatName;
    public float normalizedFloat;

    StarGameManager starGameManagerRef;

    float min, max, i;

    // Use this for initialization
    void Start()
    {
        if (starGameManagerRef == null)
            starGameManagerRef = StarGameManager.instance;

        min = 0;
        max = 100;

    }

    

    void Update()
    {
        i = starGameManagerRef.currentHealth;

        //Calculate the normalized float;

        normalizedFloat = (i - min) / (max - min);

        //Clamp the "i" float between "min" value and "max" value

        i = Mathf.Clamp(i, min, max);

        //Clamp the normalized float between 0 and 1

        normalizedFloat = Mathf.Clamp(normalizedFloat, 0, 1);
        
        gameObject.GetComponent<Animator>().SetFloat(floatName, normalizedFloat);


    }


}
