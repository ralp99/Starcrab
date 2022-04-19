using UnityEngine;
using System.Collections;

public class textBillboard : MonoBehaviour {
	
	public GameObject faceObject;

	void Update()
	{
//		if ((Input.deviceOrientation == DeviceOrientation.Portrait)||(Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown ))
//
//		{
//			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
//		                  faceObject.transform.rotation * Vector3.right);
//		} else {
//			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
//			                  faceObject.transform.rotation * Vector3.up);
//		}

		switch (Input.deviceOrientation) {
		
		case DeviceOrientation.Portrait:
			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
			                  faceObject.transform.rotation * Vector3.left);
			break;

		case DeviceOrientation.PortraitUpsideDown:
			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
			                  faceObject.transform.rotation * Vector3.right);
			break;

		case DeviceOrientation.LandscapeLeft:
			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
			                  faceObject.transform.rotation * Vector3.up);
			break;

		case DeviceOrientation.LandscapeRight:
			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
			                  faceObject.transform.rotation * Vector3.down);
			break;

		default:
			transform.LookAt (transform.position + faceObject.transform.rotation * Vector3.back,
			                  faceObject.transform.rotation * Vector3.up);
			break;
		}





	}
}
