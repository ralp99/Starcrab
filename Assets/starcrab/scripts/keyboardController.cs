using UnityEngine;
using System.Collections;

public class keyboardController : MonoBehaviour {


    public string LeftStickUpKey = "W";
    public string LeftStickDownKey = "S";
    public string LeftStickLeftKey = "A";
    public string LeftStickRightKey = "D";

    public string RightStickUpKey = "U";
    public string RightStickDownKey = "N";
    public string RightStickLeftKey = "H";
    public string RightStickRightKey = "J";

    public string buttonAKey = "M";
    public string buttonBKey = "I";
    public string buttonXKey = "L";
    public string buttonYKey = "Y";

    public string buttonLBKey = "X";
    public string buttonRBKey = "Z";

    public string DpadUpKey = "T";
    public string DpadDownKey = "V";
    public string DpadLeftKey = "F";
    public string DpadRightKey = "G";

    public string buttonBackKey = "O";
    public string buttonStartKey = "P";
    public string TriggerLeftKey = "B";
    public string TriggerRightKey = "C";
    public string AnalogInLeftKey = "Q";
    public string AnalogInRightKey = "K";

    public GameObject[] LeftStickUp;
    public GameObject[] LeftStickDown;
    public GameObject[] LeftStickLeft;
    public GameObject[] LeftStickRight;

    public GameObject[] RightStickUp;
    public GameObject[] RightStickDown;
    public GameObject[] RightStickLeft;
    public GameObject[] RightStickRight;

    public GameObject[] buttonA;
    public GameObject[] buttonB;
    public GameObject[] buttonX;
    public GameObject[] buttonY;
    public GameObject[] buttonLB;
    public GameObject[] buttonRB;

    public GameObject[] DpadUp;
    public GameObject[] DpadDown;
    public GameObject[] DpadLeft;
    public GameObject[] DpadRight;

    public GameObject[] buttonBack;
    public GameObject[] buttonStart;
    public GameObject[] TriggerLeft;
    public GameObject[] TriggerRight;
    public GameObject[] AnalogInLeft;
    public GameObject[] AnalogInRight;



    KeyCode LeftStickUpKeycode, LeftStickDownKeycode, LeftStickLeftKeycode, LeftStickRightKeycode,
        RightStickUpKeycode, RightStickDownKeycode, RightStickLeftKeycode, RightStickRightKeycode,
        buttonAKeycode, buttonBKeycode, buttonXKeycode, buttonYKeycode, buttonLBKeycode, buttonRBKeycode,
        DpadUpKeycode, DpadDownKeycode, DpadLeftKeycode, DpadRightKeycode,
        buttonBackKeycode, buttonStartKeycode, TriggerLeftKeycode, TriggerRightKeycode, AnalogInLeftKeycode, AnalogInRightKeycode;

    void Start () {

        string upperLeftStickUpKey = LeftStickUpKey.ToUpper();
        string upperLeftStickDownKey = LeftStickDownKey.ToUpper();
        string upperLeftStickLeftKey  = LeftStickLeftKey.ToUpper();
        string upperLeftStickRightKey = LeftStickRightKey.ToUpper();

        string upperRightStickUpKey = RightStickUpKey.ToUpper();
        string upperRightStickDownKey = RightStickDownKey.ToUpper();
        string upperRightStickLeftKey = RightStickLeftKey.ToUpper();
        string upperRightStickRightKey = RightStickRightKey.ToUpper();

        string upperbuttonAKey = buttonAKey.ToUpper();
        string upperbuttonBKey = buttonBKey.ToUpper();
        string upperbuttonXKey = buttonXKey.ToUpper();
        string upperbuttonYKey = buttonYKey.ToUpper();
        string upperbuttonLBKey = buttonLBKey.ToUpper();
        string upperbuttonRBKey = buttonRBKey.ToUpper();
        string upperDpadUpKey = DpadUpKey.ToUpper();
        string upperDpadDownKey = DpadDownKey.ToUpper();
        string upperDpadLeftKey = DpadLeftKey.ToUpper();
        string upperDpadRightKey = DpadRightKey.ToUpper();

        string upperbuttonBackKey = buttonBackKey.ToUpper();
        string upperbuttonStartKey = buttonStartKey.ToUpper();
        string upperTriggerLeftKey = TriggerLeftKey.ToUpper();
        string upperTriggerRightKey = TriggerRightKey.ToUpper();
        string upperAnalogInLeftKey = AnalogInLeftKey.ToUpper();
        string upperAnalogInRightKey = AnalogInRightKey.ToUpper();



        if (upperLeftStickUpKey != null)  LeftStickUpKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperLeftStickUpKey);
        if (upperLeftStickDownKey != "") LeftStickDownKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperLeftStickDownKey);
        if (upperLeftStickLeftKey != "") LeftStickLeftKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperLeftStickLeftKey);
        if (upperLeftStickRightKey != "") LeftStickRightKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperLeftStickRightKey);

        if (upperRightStickUpKey != "") RightStickUpKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperRightStickUpKey);
        if (upperRightStickDownKey != "") RightStickDownKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperRightStickDownKey);
        if (upperRightStickLeftKey != "") RightStickLeftKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperRightStickLeftKey);
        if (upperRightStickRightKey != "") RightStickRightKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperRightStickRightKey);

        if (upperbuttonAKey != "") buttonAKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonAKey);
        if (upperbuttonBKey != "") buttonBKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonBKey);
        if (upperbuttonXKey != "") buttonXKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonXKey);
        if (upperbuttonYKey != "") buttonYKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonYKey);
        if (upperbuttonLBKey != "") buttonLBKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonLBKey);
        if (upperbuttonRBKey != "") buttonRBKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonRBKey);

        if (upperDpadUpKey != "") DpadUpKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperDpadUpKey);
        if (upperDpadDownKey != "") DpadDownKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperDpadDownKey);
        if (upperDpadLeftKey != "") DpadLeftKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperDpadLeftKey);
        if (upperDpadRightKey != "") DpadRightKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperDpadRightKey);

        if (upperbuttonBackKey != "") buttonBackKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonBackKey);
        if (upperbuttonStartKey != "") buttonStartKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperbuttonStartKey);
        if (upperTriggerLeftKey != "") TriggerLeftKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperTriggerLeftKey);
        if (upperTriggerRightKey != "") TriggerRightKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperTriggerRightKey);
        if (upperAnalogInLeftKey != "") AnalogInLeftKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperAnalogInLeftKey);
        if (upperAnalogInRightKey != "") AnalogInRightKeycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upperAnalogInRightKey);


    }


    void Update () {

        checkKey(LeftStickUpKeycode, LeftStickUp);
        checkKey(LeftStickDownKeycode, LeftStickDown);
        checkKey(LeftStickLeftKeycode, LeftStickLeft);
        checkKey(LeftStickRightKeycode, LeftStickRight);

        checkKey(RightStickUpKeycode, RightStickUp);
        checkKey(RightStickDownKeycode, RightStickDown);
        checkKey(RightStickLeftKeycode, RightStickLeft);
        checkKey(RightStickRightKeycode, RightStickRight);

        checkKey(buttonAKeycode, buttonA);
        checkKey(buttonBKeycode, buttonB);
        checkKey(buttonXKeycode, buttonX);
        checkKey(buttonYKeycode, buttonY);
        checkKey(buttonLBKeycode, buttonLB);
        checkKey(buttonRBKeycode, buttonRB);

        checkKey(DpadUpKeycode, DpadUp);
        checkKey(DpadDownKeycode, DpadDown);
        checkKey(DpadLeftKeycode, DpadLeft);
        checkKey(DpadRightKeycode, DpadRight);

        checkKey(buttonBackKeycode, buttonBack);
        checkKey(buttonStartKeycode, buttonStart);
        checkKey(TriggerLeftKeycode, TriggerLeft);
        checkKey(TriggerRightKeycode, TriggerRight);
        checkKey(AnalogInLeftKeycode, AnalogInLeft);
        checkKey(AnalogInRightKeycode, AnalogInRight);

    }

    void checkKey(KeyCode checkingKey, GameObject[] doAction)
    {
        if (Input.GetKey(checkingKey)) DoAction(doAction, true);
        else DoAction(doAction, false);
    }

    void DoAction(GameObject[] activeSet, bool newState)
    {
        foreach (GameObject picked in activeSet) picked.SetActive(newState);
    }
    


}
