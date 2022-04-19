using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeStageTest : MonoBehaviour {

    public GameObject StageSizeObject;
    public float Increment = 10.0f;
    public float jumpsize;
    StarGameManager starGameManagerRef;

    public bool DoBigger, DoSmaller, DoJumpsize;


    public void JumpSize (float newSize)
    {
        StageSizeObject.transform.localScale = new Vector3(newSize, newSize, newSize);
        starGameManagerRef.AdjustTerminatorDistance();
    }

    public void Bigger()
    {
        StageSizeObject.transform.localScale = StageSizeObject.transform.localScale * Increment;
        starGameManagerRef.AdjustTerminatorDistance();

    }

    public void Smaller()
    {
        StageSizeObject.transform.localScale = StageSizeObject.transform.localScale / Increment;
        starGameManagerRef.AdjustTerminatorDistance();

    }

    void Start () {

         starGameManagerRef = StarGameManager.instance;
        starGameManagerRef.AdjustTerminatorDistance();

    }



    void Update () {

        if (DoBigger)
        {
            DoBigger = false;
            Bigger();
        }

        if (DoSmaller)
        {
            DoSmaller = false;
            Smaller();
        }

        if (DoJumpsize)
        {
            DoJumpsize = false;
            JumpSize(jumpsize);
        }




    }
}
