using UnityEngine;
using System.Collections;

public class raycastTimer : MonoBehaviour {

    public GameObject hittingNow;
	public Camera head_Camera;
	public float distanceShot = 8200f;

	GameObject lastHitByRay;

	public bool attachToCamera = false;
    public Vector3 TeleportTarget;

    public Vector3 TeleportNormal;


    void Start()

	{
		if (attachToCamera == true && head_Camera != null)
        {
			transform.parent = head_Camera.GetComponent<Camera> ().transform;
		}
	}



    void SendRayCheck(GameObject target)
    {
        if (target.GetComponent<Sensor_RAM2>())
        {
            //  currentHitByRay.GetComponent<Sensor_RAM2>().raycastTimerThatHitMe
            //   = gameObject.GetComponent<raycastTimer>();

            target.GetComponent<Sensor_RAM2>().RaycastFilterCheck(gameObject.GetComponent<raycastTimer>());

            if (lastHitByRay == null)
            {
                lastHitByRay = target;
            }
        }
    }



    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceShot))
        //		Mathf.Infinity)) 

        {
            TeleportTarget = hit.point;
            TeleportNormal = hit.normal;

            GameObject currentHitByRay = hit.collider.gameObject;
            hittingNow = currentHitByRay;
            
            if (lastHitByRay == null)
            {
                SendRayCheck(currentHitByRay);
            }
            else
            {
                if (currentHitByRay != lastHitByRay)
                {
                     lastHitByRay.GetComponent<Sensor_RAM2>().BeingHit(false);
                      lastHitByRay = null;
                      SendRayCheck(currentHitByRay);

                  //  DetachLastTarget(currentHitByRay, lastHitByRay);

                }
            }
        }
        else
        {
            //print("nothing?");
            hittingNow = null;

            if (lastHitByRay != null)
            {
                lastHitByRay.GetComponent<Sensor_RAM2>().BeingHit(false);
                lastHitByRay = null;
            }

        }
    }


    void DetachLastTarget(GameObject currentHitByRay, GameObject lastHitByRay)
    {
        lastHitByRay.GetComponent<Sensor_RAM2>().BeingHit(false);
        lastHitByRay = null;
        SendRayCheck(currentHitByRay);
    }

    
}
