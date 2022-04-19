using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
   METHODS:
 
  ClearList
  AddToList
  RemoveFromList
  RemoveIntFromList
  LowerList
  advance - "end"
  jumpTo

*/





public class sequenceNav : MonoBehaviour {

	public string msgA;
	public string msgB;
	public int startFrom = 1;
	public bool beginActive;
	public bool disableInactive;
	public bool firstStop;
	public bool lastStop;
	public bool random;
	public bool noRepeats;
	public bool displayFieldCurrent;
	public GameObject currentActive;
	 bool flipOnWrap;
	bool flipReady;

	public List<GameObject> events;
	List<GameObject> finishedEvents = new List<GameObject>();

	int sequenceEvent;
	GameObject currentObject;
	bool emptyCheck;



	void DisplayCurrentActiveObject ()
	{
		if (displayFieldCurrent)
		currentActive = events [sequenceEvent];



	}

	void Awake ()
	{
		if (events.Count == 0) emptyCheck=true;
		sequenceEvent = (startFrom - 1);
		if (beginActive )events [sequenceEvent].SetActive (true);
		DisplayCurrentActiveObject ();
	}


//	public void jumpTo (int receivedInt, bool activestate)
	public void jumpTo (int receivedInt)


	{
		sequenceEvent = (receivedInt-1);
		DisableAll ();
//		events [sequenceEvent].SetActive (!activestate);
		events [sequenceEvent].SetActive (true);
		DisplayCurrentActiveObject ();


	}

	public void ClearList ()

	{
		events.Clear();
		//events = new List<GameObject>();
	}



	public void AddToList (object[] parms)
	{
		string message = (string)parms [0];
		GameObject[] useObject = (GameObject[])parms [2];
		GameObject referenceObject  = (GameObject)parms [3];
		foreach (GameObject picked in useObject)
		{
			//	events.Add (picked);
			if (message == "0") events.Insert(0,picked);
			if (message == "1") events.Insert(events.Count,picked);
			if (message == "") events.Add (picked);
				
		}
		if (referenceObject != null) events.Add (referenceObject);
		DisplayCurrentActiveObject ();

	}




	public void RemoveFromList (object[] parms)
	{
		GameObject[] useObject = (GameObject[])parms [2];
		GameObject referenceObject  = (GameObject)parms [3];

		foreach (GameObject picked in useObject)
		{
			events.Remove (picked);
		}

		if (referenceObject != null) events.Remove (referenceObject);
		DisplayCurrentActiveObject ();


	}

	public void RemoveIntFromList (int removePosition)
	{
		events.RemoveAt (removePosition);
		DisplayCurrentActiveObject ();

	}







	public void LowerList ()
	{
		if (events.Count > 0)
		events.RemoveAt (events.Count-1);
		DisplayCurrentActiveObject ();

	}






	public void advance (string receivedMessage)
		
	{
		if (random) RandomDo();

			else
		{

		if (receivedMessage == msgA)
		{

				sequenceChange(true);
//				sequenceEvent++;


				if (sequenceEvent >= (events.Count-1))
				{

										
					if (flipOnWrap) 
					{
						print ("LAST SPACE");
						flipReady = true;
					}
				}


		if (sequenceEvent >= events.Count)
			{
//					print ("LAST SPACE");
//					if (flipOnWrap)
//					{
////						sequenceEvent = events[sequenceEvent];
//						//sequenceEvent = 0;
//						//sequenceEvent++;
//						sequenceEvent--;
//
//					}
//					else


				if (!lastStop) sequenceEvent = 0;

				else sequenceEvent = events.Count - 1;
			}
		}

		if (receivedMessage == msgB)
		{

				sequenceChange(false);
//				sequenceEvent--;

				if (sequenceEvent < 0) 
				
			    {
				if (!firstStop) sequenceEvent = events.Count - 1;
				else sequenceEvent = 0;
			}


		}




			if (receivedMessage == "end") 
			{
				// DOESN'T NEED sequenceChange I THINK..
				sequenceEvent = events.Count-1;
			}





			if (disableInactive) DisableAll ();
			events [sequenceEvent].SetActive (true);

		}

		DisplayCurrentActiveObject ();


	}

	

	void sequenceChange (bool directionFwd)
	{

		if (!flipReady) 
		{
			if (directionFwd)
				sequenceEvent++;
			else
				sequenceEvent--;
		}

		else

		{
			if (directionFwd)
				//sequenceEvent--;
				sequenceEvent = 0;
			else
				sequenceEvent++;
			flipReady = false;
		}
		DisplayCurrentActiveObject ();

	}




	void PickRandom()
	{
		currentObject = events [Random.Range (0, events.Count)];
		DisableAll ();

		currentObject.SetActive (true);

	}





	void RandomDo () 
	
	{
		if (emptyCheck == false) 
			
		{
			if (noRepeats) {
				if (events.Count == 0) {
					foreach (GameObject picked in finishedEvents) {
						events.Add (picked);	
					}
					finishedEvents.Clear ();
				}
				PickRandom ();
				finishedEvents.Add (currentObject);
				events.Remove (currentObject);
				DisableAllRandom();

			}  else
				PickRandom ();
		}
		DisplayCurrentActiveObject ();

	}








	void DisableAll()

	{

		if (disableInactive)
		{
			foreach (GameObject picked in events) {
					picked.SetActive (false);

			}
		}
		DisplayCurrentActiveObject ();

	}




	void DisableAllRandom()
		
	{
		
		if (disableInactive)
		{

			foreach (GameObject picked in finishedEvents) {
				if (picked != currentObject)	picked.SetActive (false);
				
			}
		}
		DisplayCurrentActiveObject ();

	}





}
