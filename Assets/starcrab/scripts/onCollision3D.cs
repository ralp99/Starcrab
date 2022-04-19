using UnityEngine;
using System.Collections;

public class onCollision3D : MonoBehaviour {
	
	public string collideWithTag = "";
	public GameObject collideWithObject;
    public string messageA = "";
	public bool inheritX;
	public bool inheritY;
	public bool inheritZ;
	public float offsetX;
	public float offsetY;
	public float offsetZ;
	public GameObject[] showObject;
	public GameObject hideObject;
	public bool checkAbove;
	public bool checkBelow;
	public GameObject originObject;
	public float bouncePlatformDistance;
	public float bouncePlatformTime;
	public bool bounceReturn;
	public GameObject[] dontBounce;
	public bool muteBounce;
	
	bool collideValid;
	bool proceed;
	
	float previousPosX;
	float previousPosY;
	float previousPosZ;
	
	float usePosX;
	float usePosY;
	float usePosZ;
	
	bool moving;
	bool bounceDoneYet;

    //void OnCollisionEnter2D(Collision2D col)
    void OnCollisionEnter(Collision col)

    {

        if ((col.gameObject.tag == collideWithTag)||(col.gameObject == collideWithObject) )	
		{
			collideValid = true;

		}
		
		
		
		
		
		
		
		if (collideValid)
		{

            // lastHitByRay.SendMessage("animateNoticed", false, SendMessageOptions.DontRequireReceiver);
            col.gameObject.SendMessage("collideNoticed", messageA, SendMessageOptions.DontRequireReceiver);




            if (checkAbove)
			{
				if (transform.position.y >= col.gameObject.transform.position.y) proceed = true;
			}
			
			if (checkBelow)
				
			{
				if (transform.position.y <= col.gameObject.transform.position.y) proceed = true;
			}
			
			if ((!checkAbove) && (!checkBelow)) proceed = true;
			//			print ("DOING ONE TIME NOW");
			
			// IT CHECKS ALL THE ABOVE FOR COLLISIONS
			
			
			
			
			
			
			if (proceed) 
				
			{
				//				print ("DOING ONE TIME NOW");
				
				previousPosX = col.gameObject.transform.position.x;
				previousPosY = col.gameObject.transform.position.y;
				previousPosZ = col.gameObject.transform.position.z;
				
				foreach (GameObject picked in showObject) 
				{
					picked.SetActive (true);
					if (originObject != null) 
					{
						MoveToObject();
						
						if (!muteBounce) BouncePlatform(col.gameObject);
						
						
						
					}
				}	
				proceed = false;
				collideValid = false;
			}
		}
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	void MoveToObject ()
		
	{
		
		float moveSpeed = 111f;
		float threshold = 0.1f;
		
		float newposx = originObject.transform.position.x;
		float newposy = originObject.transform.position.y;
		float newposz = originObject.transform.position.z;
		
		
		if (!inheritX)
			usePosX = newposx;
		else 
			usePosX = previousPosX+offsetX;
		
		if (!inheritY)
			usePosY = newposy;
		else 
			usePosY = previousPosY+offsetY;
		
		if (!inheritZ)
			usePosZ = newposz;
		else 
			usePosZ = previousPosZ+offsetZ;
		
		//	originObject.transform.position =  Vector3.MoveTowards (originObject.transform.position, new Vector3 (usePosX, usePosY,usePosZ), moveSpeed * Time.deltaTime);
		originObject.transform.position = new Vector3 (usePosX, usePosY, usePosZ);
	}
	
	
	
	void BouncePlatform (GameObject platform)
	{
		float sourcePlatformX = platform.transform.position.x;
		float sourcePlatformY = platform.transform.position.y;
		float sourcePlatformZ = platform.transform.position.z;
		
		float destinationPlatformY = sourcePlatformY + bouncePlatformDistance;
		
		StartCoroutine(MoveFromTo(platform, platform.transform.position, new Vector3 (sourcePlatformX, destinationPlatformY, sourcePlatformZ), bouncePlatformTime));
		
		
	}
	
	
	
	private IEnumerator MoveFromTo(GameObject platform, Vector3 pointA, Vector3 pointB, float time) 
	{
		
		bool returning = false;
		bool dontBounceBool = false; 
		foreach (GameObject picked in dontBounce) {
			if (platform == picked)
				dontBounceBool = true;
		}
		
		
		
		if ((!moving) && (!dontBounceBool)) 
		
		{  // do nothing if already moving

			moving = true; // signals "I'm moving, don't bother me!"

			if (!returning)
			{
			float t = 0f;
			while (t < 1f) {
				t += Time.deltaTime / time; // sweeps from 0 to 1 in time seconds
				//platform.transform.position = Vector3.Lerp (pointA, pointB, t); // set position proportional to t
				LerpObject (platform, pointA, pointB, t);


				yield return 0; // leave the routine and return here in the next frame
				//print ("can bounce back");
				}
			}

			if (bounceReturn)
				{
					returning  = true;

					float t2 = 0f;
				while (t2 < 1f) {
					t2 += Time.deltaTime / time; // sweeps from 0 to 1 in time seconds
					//platform.transform.position = Vector3.Lerp (pointA, pointB, t); // set position proportional to t
					LerpObject (platform, pointB, pointA, t2);
					
					
					yield return 0; // leave the routine and return here in the next frame
						returning  = false;

					}
				}




//			print ("BOUNCED BACK DONE");
				moving = false; // finished moving
			
			}
		
		}



	void LerpObject(GameObject platform, Vector3 pointA, Vector3 pointB, float t)
	{
		platform.transform.position = Vector3.Lerp (pointA, pointB, t); // set position proportional to t

	}




	
//	void BounceOLD (GameObject platform, Vector3 pointA, Vector3 pointB, float time)
//	{
//		float t = 0f;
//		while (t < 1f){
//			t += Time.deltaTime / time; // sweeps from 0 to 1 in time seconds
//			platform.transform.position = Vector3.Lerp(pointA, pointB, t); // set position proportional to t
//			//yield return 0; // leave the routine and return here in the next frame
//		}
//	}
	
	
	
	
}

