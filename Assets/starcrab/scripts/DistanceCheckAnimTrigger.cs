using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheckAnimTrigger : MonoBehaviour {

    public StarGameManager starGameManagerRef;
    public float thresholdAppear = 0.479f;
    private float initThresholdAppear;

    public Animator Animator;
    public string trigger;
    private GameObject centerCheck;
    private bool appeared;


    void Start ()
    {
        starGameManagerRef = StarGameManager.instance;
        centerCheck = starGameManagerRef.ActorsParent;
        initThresholdAppear = thresholdAppear;

        if (!starGameManagerRef.DistanceCheckResize.Contains(gameObject))
        {
            starGameManagerRef.DistanceCheckResize.Add(gameObject);
        }

        AdjustDistanceCheckForResize();
    }

    private void OnEnable()
    {
        appeared = false;
        if (starGameManagerRef != null)
        {
            AdjustDistanceCheckForResize();
        }
    }


    public void AdjustDistanceCheckForResize()
    {
        thresholdAppear = initThresholdAppear * starGameManagerRef.StageSize;
    }


    void Update ()

    {
  
        float currentDistVector = Vector3.Distance(centerCheck.transform.position, gameObject.transform.position);
        // print(currentDistVector);


        if (!appeared)
        {
            if (thresholdAppear > currentDistVector)
            {
                appeared = true;
                Animator.SetTrigger(trigger);
            }
        }

        else
        {
              if (currentDistVector > thresholdAppear)
              {
                  gameObject.SetActive(false);
              }
        }
    }
}
