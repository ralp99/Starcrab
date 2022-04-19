using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventPlayer2 : MonoBehaviour
{

    public string EventNote1;
    public UnityEvent Event1;
    public string EventNote2;
    public UnityEvent Event2;
    public string EventNote3;
    public UnityEvent Event3;
    public string EventNote4;
    public UnityEvent Event4;
    public string EventNote5;
    public UnityEvent Event5;
    public string EventNote6;
    public UnityEvent Event6;
    public string EventNote7;
    public UnityEvent Event7;
    public string EventNote8;
    public UnityEvent Event8;
    public string EventNote9;
    public UnityEvent Event9;


    void animEventDo1()
    {
        Event1.Invoke();
    }

    void animEventDo2()
    {
        Event2.Invoke();
    }

    void animEventDo3()
    {
        Event3.Invoke();
    }

    void animEventDo4()
    {
        Event4.Invoke();
    }

    void animEventDo5()
    {
        Event5.Invoke();
    }

    void animEventDo6()
    {
        Event6.Invoke();
    }

    void animEventDo7()
    {
        Event7.Invoke();
    }

    void animEventDo8()
    {
        Event8.Invoke();
    }

    void animEventDo9()
    {
        Event9.Invoke();
    }
    
}
