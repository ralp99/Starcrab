using UnityEngine;
using System.Collections;

public class mousePan : MonoBehaviour {

	bool bDragging;

	public bool lockX;
	public bool lockY;
	public bool lockZ;

	Vector3 panOrigin;
	Vector3 oldPos;
	public int panSpeed = 100;

	float originalxpos;
	float originalypos;
	float originalzpos;

	float useX;
	float useY;
	float useZ;

	float newxpos;
	float newypos;
	float newzpos;
	Vector3 newTransformPosition;

	void Start ()
	{

		originalxpos = transform.position.x;
		originalypos = transform.position.y;
		originalzpos = transform.position.z;

	}

	void Update()
		
	{
		MousePanDo ();
	}

	void MousePanDo ()

	{

	if(Input.GetMouseButtonDown(0))
	{
		bDragging = true;
		oldPos = transform.position;
		panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked

	}
	
	if(Input.GetMouseButton(0))
	{
		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;    //Get the difference between where the mouse clicked and where it moved



		//Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion

			newTransformPosition = oldPos + -pos * panSpeed;

			useX = newTransformPosition.x;
			useY = newTransformPosition.y;
			useZ = newTransformPosition.z;

			if (lockX) useX = originalxpos;
			if (lockY) useY = originalypos;
			if (lockZ) useZ = originalzpos;

			transform.position = new Vector3 (useX, useY, useZ);



	}
	
	if(Input.GetMouseButtonUp(0))
	{
		bDragging = false;
	}

}
}

