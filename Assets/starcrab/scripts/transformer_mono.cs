using UnityEngine;
using System.Collections;

public class transformer_mono : MonoBehaviour {
	//public class transformer_WIP3 : MonoBehaviour {

	public MonoBehaviour[] killMono;


	public GameObject picked;

	public float positionX;
	public bool consecutivePX;
	public float positionY;
	public bool consecutivePY;
	public float positionZ;
	public bool consecutivePZ;

	public float rotationX;
	public bool consecutiveRX;
	public float rotationY;
	public bool consecutiveRY;
	public float rotationZ;
	public bool consecutiveRZ;

	public float scaleX;
	public bool consecutiveSX;
	public float scaleY;
	public bool consecutiveSY;
	public float scaleZ;
	public bool consecutiveSZ;

	public bool posXToZero;
	public bool posYToZero;
	public bool posZToZero;

	public bool rotXToZero;
	public bool rotYToZero;
	public bool rotZToZero;

	public bool scaXToOne;
	public bool scaYToOne;
	public bool scaZToOne;

	public bool EnforceMinScales;
	public float minimumScaX;
	public float minimumScaY;
	public float minimumScaZ;

	public bool EnforceMaxScales;
	public float maximumScaX;
	public float maximumScaY;
	public float maximumScaZ;

	public bool disableAfterOnce;
	public bool keepDoing;
	public float moveSpeed;

	float positionCurrentX;
	float positionCurrentY;
	float positionCurrentZ;

	float rotationCurrentX;
	float rotationCurrentY;
	float rotationCurrentZ;

	float scaleCurrentX;
	float scaleCurrentY;
	float scaleCurrentZ;

	Vector3 newPosition;
	Vector3 newRotation;
	Vector3 newScale;

	float activepositionX;
	float activepositionY;
	float activepositionZ;
	float activerotationX;
	float activerotationY;
	float activerotationZ;
	float activescaleX;
	float activescaleY;
	float activescaleZ;
	bool clearForQuit;

   public float limit;
   public bool useZeroLimit;
 //   public GameObject watchMultiplier;
   // float overrideValue;
  //  bool dontUseOverride;
    bool checkLimit;



	void Awake () {

    //    if (watchMultiplier != null) overrideValue = watchMultiplier.GetComponent<ValueStore>().controllerMultiply;
     //   else dontUseOverride = true;


        if ((limit != 0) || (useZeroLimit)) checkLimit = true;
       

        activepositionX = positionX;
		activepositionY = positionY;
		activepositionZ = positionZ;
		
		activerotationX = rotationX;
		activerotationY = rotationY;
		activerotationZ = rotationZ;
		
		activescaleX = scaleX;
		activescaleY = scaleY;
		activescaleZ = scaleZ;

	
	}


//	 Vector3 Reorient(Vector3 startValue, Vector3 gotoValue)
//	
//	{
//		return Vector3.MoveTowards(startValue,gotoValue,moveSpeed*Time.deltaTime);
//	}


//	void disableCurrentObject()
//	{
//		clearForQuit = false;
//		gameObject.SetActive (false);
//	}




//	void Reposition(float positionToChange, float positionCurrent, float positionInput, bool consecutive)
//	{
//	}


	void OnEnable()
	{
		DoTransform ();
	}


    float CheckLimitTransform(float positionCurrent, float positionUserInput)

    {
        if ((!useZeroLimit)&&(!checkLimit)) return positionCurrent + positionUserInput;
        if (useZeroLimit) limit = 0;
        if (checkLimit)
        {

            
            float newSum = positionCurrent + positionUserInput;
            bool outOfLimit = false;
            
            if ((limit < 0) && (newSum <= limit)) outOfLimit = true;
            if ((positionCurrent < limit ) && ((limit > 0) && (newSum >= limit))) outOfLimit = true;
            if ((positionCurrent > limit) && ((limit > 0) && (newSum <= limit))) outOfLimit = true;


            if ((outOfLimit) || ((limit == 0) && (positionCurrent < 0) && ((positionCurrent + positionUserInput) >= limit)) ||
               ((limit == 0) && (positionCurrent > 0) && ((positionCurrent + positionUserInput) <= limit)))
               
            {

         //       if ((limit > 0) && ((positionCurrent + positionUserInput) >= limit)) print("A");

          //     if ((limit < 0) && ((positionCurrent + positionUserInput) <= limit)) print("B");

           //     if ((limit > 0) && ((positionCurrent + positionUserInput) <= limit)) print("C");
           //     print(""+gameObject);



 //   print("CANNOT");


                // if (0.05 > 0) (YES!!) and (0.24 >= 0.05) (ALSO  YES)

           //     print("sum "+ (positionCurrent + positionUserInput));



                //print("CANNOT DO ANYTHING");
                //print("limit "+limit);
                //print("pos current "+positionCurrent);
                //print("pos user input " + positionUserInput);
                //print("sum "+ (positionCurrent + positionUserInput));
                return positionCurrent;
                // DO NOTHING
            }
            else return positionCurrent + positionUserInput;
        }
        else return positionUserInput;

    }



    void DoTransform()

	{
		//SET X, Y, Z current positions


		positionCurrentX = picked.transform.localPosition.x;
		positionCurrentY = picked.transform.localPosition.y;
		positionCurrentZ = picked.transform.localPosition.z;




        // IF USER DEFINED IS ACTUALLY A NUMBER
        if (positionX != 0)
        {

            // IF CONSECUTIVE, ADD IT TO CURRENT
            if (consecutivePX) positionCurrentX = CheckLimitTransform(positionCurrentX, positionX);

            // ELSE CURRENT IS USER DEFINED NUMBER
            else positionCurrentX = positionX;

		}

		if (positionY != 0) {
			
			if (consecutivePY) positionCurrentY = CheckLimitTransform(positionCurrentY, positionY);
            else positionCurrentY = positionY;
			
		}

		if (positionZ != 0) {
			
			if (consecutivePZ) positionCurrentZ = CheckLimitTransform(positionCurrentZ, positionZ);
            else positionCurrentZ = positionZ;
			
		}


		if (posXToZero) positionCurrentX = 0;
		if (posYToZero) positionCurrentY = 0;
		if (posZToZero) positionCurrentZ = 0;


        //	if (!((positionX == 0) && (positionY == 0) && (positionZ == 0) && (posXToZero == false) && (posYToZero == false) && (posZToZero == false))) 

        if ((limit != 0) || (useZeroLimit))
            { }

			picked.transform.localPosition = new Vector3(positionCurrentX,positionCurrentY,positionCurrentZ);






//		if (killMono != null) foreach (MonoBehaviour pickedm in killMono) {
//
//				pickedm.enabled = false;
//		}
//		clearForQuit = true;

		// ----------------------------------------------------------------------------------------------------------------


		scaleCurrentX = picked.transform.localScale.x;
		scaleCurrentY = picked.transform.localScale.y;
		scaleCurrentZ = picked.transform.localScale.z;


		if (scaleX != 0) {
			
			if (consecutiveSX) scaleCurrentX = scaleCurrentX + scaleX;
			else scaleCurrentX = scaleX;
			
		}
		
		if (scaleY != 0) {
			
			if (consecutiveSY) scaleCurrentY = scaleCurrentY + scaleY;
			else scaleCurrentY = scaleY;
			
		}
		
		if (scaleZ != 0) {
			
			if (consecutiveSZ) scaleCurrentZ = scaleCurrentZ + scaleZ;
			else scaleCurrentZ = scaleZ;
			
		}
		
		
		if (scaXToOne) scaleCurrentX = 1;
		if (scaYToOne) scaleCurrentY = 1;
		if (scaZToOne) scaleCurrentZ = 1;

		if (EnforceMinScales) 
		
		{
			if (scaleCurrentX <= minimumScaX) scaleCurrentX = minimumScaX;
			if (scaleCurrentY <= minimumScaY) scaleCurrentY = minimumScaY;
			if (scaleCurrentZ <= minimumScaZ) scaleCurrentZ = minimumScaZ;

		}


		if (EnforceMaxScales) 
			
		{
			if (scaleCurrentX >= maximumScaX) scaleCurrentX = maximumScaX;
			if (scaleCurrentX >= maximumScaY) scaleCurrentY = maximumScaY;
			if (scaleCurrentX >= maximumScaZ) scaleCurrentZ = maximumScaZ;

			
		}


		picked.transform.localScale = new Vector3(scaleCurrentX,scaleCurrentY,scaleCurrentZ);
		//picked.transform.lossyScale = new Vector3(scaleCurrentX,scaleCurrentY,scaleCurrentZ); 

	//	picked.transform.localScale.






		// ----------------------------------------------------------------------------------------------------------------

		if (activerotationX != 0) {

			{
				if (!consecutiveRX) {
					// REPLACE ROTATION
					// switched from position, script is working in opposite booleans sort of
				//	picked.transform.rotation = Quaternion.Euler(rotationX,picked.transform.rotation.y,picked.transform.rotation.z);
					picked.transform.rotation = Quaternion.Euler(activerotationX,0,0);

				} 
				
				else 
					
				{
					// CONSECUTIVE ROTATION

					//picked.transform.Rotate (rotationX, picked.transform.rotation.y,picked.transform.rotation.z);
					//THIS SHOULD BE ADDING ZERO TO OTHER OBJECTS
					picked.transform.Rotate (activerotationX, 0,0);

				}
				
			}
		}
		
		if (rotXToZero) {

		//	picked.transform.rotation = Quaternion.Euler(0,picked.transform.rotation.y,picked.transform.rotation.z);
				picked.transform.rotation = Quaternion.Euler(0,picked.transform.rotation.y,picked.transform.rotation.z);


		}
		
		// ----------------------


		
		if (activerotationY != 0) {
			
			{
				if (!consecutiveRY) {
					// REPLACE ROTATION
					// switched from position, script is working in opposite booleans sort of
					picked.transform.rotation = Quaternion.Euler(picked.transform.rotation.x,activerotationY,picked.transform.rotation.z);
				} 
				
				else 
					
				{
					// CONSECUTIVE ROTATION
					//THIS SHOULD BE ADDING ZERO TO OTHER OBJECTS
					//picked.transform.Rotate (rotationX, picked.transform.rotation.y,picked.transform.rotation.z);
					picked.transform.Rotate (0,activerotationY,0);
					
				}
				
			}
		}
		
		if (rotYToZero) {
			
			picked.transform.rotation = Quaternion.Euler(picked.transform.rotation.x,0,picked.transform.rotation.z);
			
			
		}
		
		// ----------------------



		if (killMono != null) foreach (MonoBehaviour pickedm in killMono) {
			
			pickedm.enabled = false;
		}

		clearForQuit = true;







	//	//gameObject.SetActive (false);
//		if (clearForQuit) {
//		//	//disableCurrentObject ();
//			
//			if (disableAfterOnce) gameObject.SetActive (false);
//			if (keepDoing)
//			{
//				//make it continue
//			}

		//	//GetComponent<transformer_mono> ().enabled = false; //NOT USRE IF THIS SHOULD BE DISABLED


		}

	void Update ()
	{

	if (clearForQuit) {
			//	//disableCurrentObject ();
		
			if (disableAfterOnce)
				gameObject.SetActive (false);
			if (keepDoing) {
				//make it continue
				DoTransform();
			}
		}



	}


}
