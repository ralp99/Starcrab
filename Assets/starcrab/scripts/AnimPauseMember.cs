using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPauseMember : MonoBehaviour {

    StarGameManager starGameManagerRef;
    public Animator animator;
    List<Animator> pauseList;

    private void Start()
    {
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }



    void StarGameManagerRefChecker()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
    }


    private void OnEnable()
    {
        StarGameManagerRefChecker();
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        starGameManagerRef.PauseAnimList.Add(animator);
    }

    private void OnDisable()
    {
        if (pauseList == null)
        {
            pauseList = starGameManagerRef.PauseAnimList;
        }

        pauseList.Remove(animator);
    }


}
