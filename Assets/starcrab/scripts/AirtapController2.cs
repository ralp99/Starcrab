using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AirtapController2 : MonoBehaviour {

    public enum JoyButtonPress { None, Global, A, B, X, Y, LB, RB }

    public JoyButtonPress joyButtonPress;
    public bool limit;
    public UnityEvent ThisEvent;
    bool released = true;

    private string XbuttonString;
    private string YbuttonString;
    private string AbuttonString;
    private string BbuttonString;
    private string LBbuttonString;
    private string RBbuttonString;

    private string usingButtonString;
    StarGameManager starGameManagerRef;

    Sensor_RAM2 gazeSensor;

    public bool Activated;

    private void OnEnable()
    {
        released = true;
    }
    

    private void Start()
    {

        starGameManagerRef = StarGameManager.instance;

        XbuttonString = "360_X";
        YbuttonString = "360_Y";
        AbuttonString = "360_A";
        BbuttonString = "360_B";
        LBbuttonString = "360_LB";
        RBbuttonString = "360_RB";


        switch (joyButtonPress)
        {
            case JoyButtonPress.None:
                break;

            case JoyButtonPress.Global:
                usingButtonString = starGameManagerRef.usingButtonString;

                break;

            case JoyButtonPress.A:
                usingButtonString = AbuttonString;
                break;

            case JoyButtonPress.B:
                usingButtonString = BbuttonString;
                break;

            case JoyButtonPress.X:
                usingButtonString = XbuttonString;
                break;

            case JoyButtonPress.Y:
                usingButtonString = YbuttonString;
                break;

            case JoyButtonPress.LB:
                usingButtonString = LBbuttonString;
                break;

            case JoyButtonPress.RB:
                usingButtonString = RBbuttonString;
                break;
        }

        gazeSensor = gameObject.GetComponent<Sensor_RAM2>();

    }

    void Update()

    {

    if (Input.GetButtonDown(usingButtonString) && gazeSensor.selected)

       {
            ThisEvent.Invoke();
            Activated = true;

            starGameManagerRef.UiManager.PlaySelectMadeSound();  // works w joystick press
        }

    }

    void OnSelect()
    {
        print("tapped");

        // this is happening twice for some reason, when I am finger tapping MOVE button
        if (!limit)
        {
            released = true;
        }

        if (released)
        {
            ThisEvent.Invoke();
            Activated = true;

            starGameManagerRef.UiManager.PlaySelectMadeSound();  // works w finger

            if (limit)

            {
                if (gameObject.transform.parent.gameObject.activeSelf)  // trying bandaid
                {
                    StartCoroutine(ReleaseTimer());  // parent check do this if not inactive
                }
            }
        }
    }


    IEnumerator ReleaseTimer()
    {
        print("01 CR started");
        released = false;
        yield return new WaitForSeconds(0.1f);
        print("02 CR FINISHED");
        released = true;
        Activated = false;

    }

}
