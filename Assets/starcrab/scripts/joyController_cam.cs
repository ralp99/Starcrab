using UnityEngine;
using System.Collections;

public class joyController_cam : MonoBehaviour {

	Vector3 movementVector;
	//CharacterController characterController;
	public float movementSpeed = 8f;
	//float gravity = 40;
	public bool XControl;
	public bool YControl;
	public bool flipx;
	public bool flipy;
	int xdir = 1;
	int ydir = 1;

	void Start () {
	//	characterController = GetComponent<CharacterController>();
		if (flipx)
			xdir = -1;
		if (flipy)
			ydir = -1;
	}




	
	void Update () {
		float xCoords = 0f;
		float yCoords = 0f;

	//	movementVector.x = Input.GetAxis ("360_LeftJoystickX") * movementSpeed;
	//	movementVector.z = Input.GetAxis ("360_LeftJoystickY") * movementSpeed;

		//movementVector.y -= gravity * Time.deltaTime;

	//	characterController.Move (movementVector * Time.deltaTime);
	
		if (XControl)
			xCoords = Input.GetAxis ("360_LeftJoystickX") * movementSpeed * xdir * Time.deltaTime;

		if (YControl)
			yCoords = Input.GetAxis ("360_LeftJoystickY") * movementSpeed * ydir * Time.deltaTime;

	
		transform.Rotate(yCoords,xCoords,0);



		
		if(Input.GetButtonDown("360_X"))
		{
			// RESET ONLY Z TO 0

				transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,0);

		}



		if(Input.GetButtonDown("360_B"))
		{
			// RESET ALL TO 0,0,0

			transform.rotation = Quaternion.identity;
		}

	}
}
