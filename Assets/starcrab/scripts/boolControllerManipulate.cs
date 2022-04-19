using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boolControllerManipulate : MonoBehaviour {
    // THIS IS PROBABLY OBSOLETE
    // RMA 2019_07_17


    public enum BoolType { Move, Rotate, Scale }

    public BoolType boolType;

    public GameObject usingObject;

    public bool newValue;




	void OnEnable () {

        if (boolType == BoolType.Move)
            usingObject.GetComponent<GestureManager>().manualMove = newValue;

        if (boolType == BoolType.Rotate)
            usingObject.GetComponent<GestureManager>().manualRotate = newValue;

        if (boolType == BoolType.Scale)
            usingObject.GetComponent<GestureManager>().manualScale = newValue;

        gameObject.SetActive(false);


    }
}
