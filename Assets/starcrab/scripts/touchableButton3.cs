using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class touchableButton3 : MonoBehaviour {


    public UnityEvent TouchdownEvent;
    public UnityEvent ReleaseEvent;




    void OnTouchDown()
    {
        TouchdownEvent.Invoke();
    }

    void OnTouchUp()
    {
        ReleaseEvent.Invoke();
    }


    void OnTouchStay() // TOUCH DRAG OFF AND BACK ON TO GO DOWN AGAIN
    {

    }

    void OnTouchExit() // DESELECTED
    {

    }
}
