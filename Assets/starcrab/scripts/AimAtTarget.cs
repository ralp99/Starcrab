using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtTarget : MonoBehaviour {
    public Transform TargetOverride;
    private StarGameManager starGameManagerRef;

    void Update ()
    {

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (TargetOverride == null)
        {
            if (starGameManagerRef != null)
            {
                TargetOverride = starGameManagerRef.HeroShip.transform;
            }
        }

        if (TargetOverride != null)
        {
               transform.LookAt(TargetOverride);
        }
    }
}
