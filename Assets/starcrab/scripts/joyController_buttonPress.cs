using UnityEngine;
using System.Collections;

public class joyController_buttonPress : MonoBehaviour {

	public enum SystemType {Windows, Mac, Linux, Autodetect}

	public SystemType systemType;

	public GameObject[] buttonX;
	public bool toggleX;

	public GameObject[] buttonY;
    public bool toggleY;
    public bool testCheckY;

	public GameObject[] buttonA;
    public bool toggleA;

	public GameObject[] buttonB;
    public bool toggleB;

	public GameObject[] buttonLB;
    public bool toggleLB;

	public GameObject[] buttonRB;
    public bool toggleRB;

	public GameObject[] stickLeftXL;
	//public bool togglestickLeftXL;

	public GameObject[] stickLeftXR;
	//public bool togglestickLeftXR;

	public GameObject[] stickLeftYD;
	//public bool togglestickLeftYD;

	public GameObject[] stickLeftYU;
	//public bool togglestickLeftYU;

	public GameObject[] stickRightXL;
	//public bool togglestickRightXL;
	
	public GameObject[] stickRightXR;
	//public bool togglestickRightXR;
	
	public GameObject[] stickRightYD;
//	public bool togglestickRightYD;
	
	public GameObject[] stickRightYU;
    //public bool togglestickRightYU;

    public GameObject[] TriggerLeft;
    public GameObject[] TriggerRight;

    public GameObject[] DpadLeft, DpadRight, DpadDown, DpadUp;



    public float thresholdX;
    public float thresholdY;
    public bool flipLeftX;
    public bool flipLeftY;
    public bool flipRightX;
    public bool flipRightY;



    GameObject[] useLeftXL;
    GameObject[] useLeftXR;
    GameObject[] useLeftYU;
    GameObject[] useLeftYD;


    GameObject[] useRightXL;
    GameObject[] useRightXR;
    GameObject[] useRightYU;
    GameObject[] useRightYD;

    bool useState;

	string XbuttonString, YbuttonString, AbuttonString, BbuttonString;
	string stickLeftXString, stickLeftYString;
	string stickRightXString, stickRightYString;

	string dPadHorizontalString, dPadVerticalString;
	string bumperRightString, triggerRightString, bumperLeftString, triggerLeftString;
	string DpadUpbuttonString, DpadDownbuttonString, DpadLeftbuttonString, DpadRightbuttonString;



    void Start()
    {


		if (systemType == SystemType.Autodetect) 
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor)||
				(Application.platform == RuntimePlatform.WindowsPlayer))
				systemType = SystemType.Windows;

			if ((Application.platform == RuntimePlatform.OSXEditor)||
				(Application.platform == RuntimePlatform.OSXPlayer))
				systemType = SystemType.Mac;

//			if ((Application.platform == RuntimePlatform.LinuxPlayer)||
//				(Application.platform == RuntimePlatform.LinuxEditor))
		 	if (Application.platform == RuntimePlatform.LinuxPlayer)
				systemType = SystemType.Linux;

		}


		stickLeftXString = "360_LeftJoystickX";
		stickLeftYString = "360_LeftJoystickY";

		if ((systemType == SystemType.Windows) || (systemType == SystemType.Linux) )
		
		{
			XbuttonString = "360_X";
			YbuttonString = "360_Y";
			AbuttonString = "360_A";
			BbuttonString = "360_B";

			dPadHorizontalString = "360_DpadHorizontal";
			dPadVerticalString = "360_DpadVertical";

			bumperRightString = "360_RB";
			bumperLeftString = "360_LB";
			triggerRightString = "360_TriggerRight";
			triggerLeftString = "360_TriggerLeft";

			stickRightXString = "360_RightJoystickX";
			stickRightYString = "360_RightJoystickY";

		} 

		if (systemType == SystemType.Mac)  
			
		{
			XbuttonString = "360_X_mac";
			YbuttonString = "360_Y_mac";
			AbuttonString = "360_A_mac";
			BbuttonString = "360_B_mac";

			dPadHorizontalString = "360_DpadHorizontal_mac";
			dPadVerticalString = "360_DpadVertical_mac";

			bumperRightString = "360_RB_mac";
			bumperLeftString = "360_LB_mac";
			triggerRightString = "360_TriggerRight_mac";
			triggerLeftString = "360_TriggerLeft_mac";

			stickRightXString = "360_RightJoystickX_mac";
			stickRightYString = "360_RightJoystickY_mac";

			DpadUpbuttonString = "360_DpadUp_mac";
			DpadDownbuttonString = "360_DpadDown_mac";
			DpadLeftbuttonString = "360_DpadLeft_mac";
			DpadRightbuttonString = "360_DpadRight_mac";


		}

		if (systemType == SystemType.Linux)  

		{
			triggerRightString = "360_TriggerRight_mac";
			triggerLeftString = "360_TriggerLeft_linux";

			dPadHorizontalString = "360_DpadHorizontal_linux";
			dPadVerticalString = "360_DpadVertical_linux";
		
		}



        useLeftXL = stickLeftXL;
        useLeftXR = stickLeftXR;
        useLeftYU = stickLeftYU;
        useLeftYD = stickLeftYD;

        useRightXL = stickRightXL;
        useRightXR = stickRightXR;
        useRightYU = stickRightYU;
        useRightYD = stickRightYD;

        if (flipLeftX)
        {
            useLeftXL = stickLeftXR;
            useLeftXR = stickLeftXL;
        }

        if (flipLeftY)
        {
            useLeftYU = stickLeftYD;
            useLeftYD = stickLeftYU;
        }

        if (flipRightX)
        {
            useRightXL = stickRightXR;
            useRightXR = stickRightXL;
        }

        if (flipRightY)
        {
            useRightYU = stickRightYD;
            useRightYD = stickRightYU;
        }


    }


    void Update () {




		if (systemType == SystemType.Mac) 
		{
			// DPAD used digital buttons on Mac

			if (DpadUp != null) 

			{
				if ((Input.GetButtonDown(DpadUpbuttonString) || (Input.GetButtonUp(DpadUpbuttonString))))
					CheckDigitalStates (DpadUp, false);

			}



			if (DpadDown != null) 

			{
				if ((Input.GetButtonDown(DpadDownbuttonString)|| (Input.GetButtonUp(DpadDownbuttonString))))
					CheckDigitalStates (DpadDown, false);
			
			}


			if (DpadLeft != null) 

			{
				if ((Input.GetButtonDown(DpadLeftbuttonString))||(Input.GetButtonUp(DpadLeftbuttonString)))
					CheckDigitalStates (DpadLeft, false);

			}



			if (DpadRight != null) 

			{
				if ((Input.GetButtonDown(DpadRightbuttonString))||(Input.GetButtonUp(DpadRightbuttonString)))
					CheckDigitalStates (DpadRight, false);

			}


		} // END MAC CHECK

		else


		{
			
			CheckAnalogStates (dPadHorizontalString, DpadLeft, 0, false);
			CheckAnalogStates (dPadHorizontalString, DpadRight, 0, true);


			if (systemType == SystemType.Windows) {


				CheckAnalogStates (dPadVerticalString, DpadDown, 0, false);
				CheckAnalogStates (dPadVerticalString, DpadUp, 0, true);

			}


			else 
				// USE REVERSE VERTICALS FOR LINUX

			{

				CheckAnalogStates (dPadVerticalString, DpadDown, 0, true);
				CheckAnalogStates (dPadVerticalString, DpadUp, 0, false);

			}

				

		} // END ELSE DPAD


		CheckAnalogStates (triggerLeftString, TriggerLeft, 0, true);
		CheckAnalogStates (triggerRightString, TriggerRight, 0, true);


        if (buttonX != null) {


			if ((Input.GetButtonDown(XbuttonString))||((!toggleX)&&(Input.GetButtonUp(XbuttonString))))
				CheckDigitalStates (buttonX, false);

        }



        if (buttonY != null) {


			if ((Input.GetButtonDown(YbuttonString))||((!toggleY)&&(Input.GetButtonUp(YbuttonString))))
				CheckDigitalStates (buttonY, testCheckY);

        }
        //88


		if (buttonA != null) {
			
			if ((Input.GetButtonDown(AbuttonString))||((!toggleA)&&(Input.GetButtonUp(AbuttonString))))
				CheckDigitalStates (buttonA, false);

        }



		if (buttonB != null) {


			if ((Input.GetButtonDown(BbuttonString))||((!toggleB)&&(Input.GetButtonUp(BbuttonString))))
				CheckDigitalStates (buttonB, false);

			
        }


		if (buttonLB != null) {

			if ((Input.GetButtonDown(bumperLeftString))||((!toggleLB)&&(Input.GetButtonUp(bumperLeftString))))
				CheckDigitalStates (buttonLB, false);
			
        }



		if (buttonRB != null) {

			if ((Input.GetButtonDown(bumperRightString))||((!toggleRB)&&(Input.GetButtonUp(bumperRightString))))
				CheckDigitalStates (buttonRB, false);

        }

		CheckAnalogStates (stickLeftXString, useLeftXL, (thresholdX * -1), false);
		CheckAnalogStates (stickLeftXString, useLeftXR, thresholdX, true);
		CheckAnalogStates (stickLeftYString, useLeftYU, (thresholdY*-1), false);
		CheckAnalogStates (stickLeftYString, useLeftYD, thresholdY, true);

		CheckAnalogStates (stickRightXString, useRightXL, 0, false);
		CheckAnalogStates (stickRightXString, useRightXR, 0, true);
		CheckAnalogStates (stickRightYString, useRightYU, 0, false);
		CheckAnalogStates (stickRightYString, useRightYD, 0, true);
		
	} // END UPDATE




	
	void CheckAnalogStates(string checkString, GameObject[] pickedSet, float threshold, bool checkGreater)
		{
	
		if (pickedSet != null)
		
			{

			if (checkGreater) 
				{
				if (Input.GetAxis (checkString) > threshold)
					useState = true;
				else
					useState = false;
				}

			else
				
				{
				if (Input.GetAxis (checkString) < threshold)
					useState = true;
				else
					useState = false;
				}


			foreach (GameObject picked in pickedSet) picked.SetActive(useState);

			}

		}



    //void CheckDigitalStates (GameObject[] pickedSet)

    //{

    //	foreach (GameObject picked in pickedSet)
    //	{

    //		if (picked.gameObject.activeSelf)
    //			picked.SetActive(false);
    //		else picked.SetActive(true);

    //	}


    //}



    void CheckDigitalStates(GameObject[] pickedSet, bool dontReverseCheck)

    {

        foreach (GameObject picked in pickedSet)
        {
            if (dontReverseCheck)

            {
                print("doing test check");
                //if (picked.gameObject.activeSelf) picked.SetActive(true);
                //else picked.SetActive(true);
                if (picked.gameObject.active) print("active");
                else picked.SetActive(true);

            }

            else

            { 
            if (picked.gameObject.activeSelf)
                picked.SetActive(false);
            else picked.SetActive(true);
            }
        }


    }






}
