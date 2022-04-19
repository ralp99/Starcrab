using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolController2 : MonoBehaviour {

    StarGameManager starGameManagerRef;
    public Animator Animator;
    bool currentState;

void AnimatorValueCheck()

    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (starGameManagerRef != null && Animator == null)
        {
            Animator = starGameManagerRef.UiManager.PerimeterBoxOutlineAnim;
        }
    }


    public void SetBoolTrue(string boolName)
    {
        AnimatorValueCheck();
        currentState = true;
        EffectAnimatorBool(boolName, currentState);
      //  Animator.SetBool(boolName, true);
    }

    public void SetBoolFalse(string boolName)
    {
        AnimatorValueCheck();
        currentState = false;
        EffectAnimatorBool(boolName, currentState);
        //  Animator.SetBool(boolName, false);
    }

    public void FlipBool(string boolName)
    {
        AnimatorValueCheck();
        currentState = !currentState;
        EffectAnimatorBool(boolName, currentState);

    }

    void EffectAnimatorBool(string boolName, bool state)
    {
        AnimatorValueCheck();
        if (Animator != null)
        {
            Animator.SetBool(boolName, state);
        }
    }

}
