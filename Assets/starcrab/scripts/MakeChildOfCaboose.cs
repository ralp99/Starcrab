using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildOfCaboose : MonoBehaviour {


    StarGameManager starGameManagerRef;
    GameObject caboose;

    
    private void OnEnable()
    {

        print("-- make child of caboose is active -- proving that this script is used somewhere");

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        caboose = starGameManagerRef.Caboose;
        gameObject.transform.parent = caboose.transform;
        PoolListMembership poolListMembership = caboose.GetComponent<PoolListMembership>();

        poolListMembership.disableChildren.Add(gameObject.transform);
        poolListMembership.unparentChildren.Add(gameObject.transform);
    }

}
