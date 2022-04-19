using UnityEngine;
using System.Collections;

public class sendValueToObject2 : MonoBehaviour
{

    public GameObject manager;
    public string method;
    //	public int value;
    public string message;
    public string type;
    public string extra;
    //public bool locateHere;
    public GameObject[] subject;
    public GameObject inheritLocation;
    public float offsetX, offsetY, offsetZ;
    public bool localPosition;
    Vector3 location;
    bool sendLocation;
    bool sendUseLocalCoordinates;
    float posX, posY, posZ;
    bool didTransformOnce;
    StarGameManager starGameManagerRef;


    //string objectName;


    void Start()
    {


        //if (inheritLocation != null)
        //{
        //    posX = inheritLocation.transform.position.x;
        //    posY = inheritLocation.transform.position.y;
        //    posZ = inheritLocation.transform.position.z;
        //}

        //posX = posX + offsetX;
        //posY = posY + offsetY;
        //posZ = posZ + offsetZ;

        //location = new Vector3(posX, posY, posZ);
        //sendLocation = true;




    }



    void OnEnable()
    {


        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (manager == null)
        {
            manager = starGameManagerRef.EnemySpawnManager;
        }


        posX = 0;
        posY = 0;
        posZ = 0;


        //		if (subject == null)
        //			manager.SendMessage (method, message);
        //		else 
        //		
        //		{


        // -----------------------

        //  if (!didTransformOnce)
        //  {
        if (inheritLocation != null)
        {
            posX = inheritLocation.transform.position.x;
            posY = inheritLocation.transform.position.y;
            posZ = inheritLocation.transform.position.z;
        }

        posX = posX + offsetX;
        posY = posY + offsetY;
        posZ = posZ + offsetZ;

        location = new Vector3(posX, posY, posZ);
        sendLocation = true;
        if (localPosition) sendUseLocalCoordinates = true;
        //  didTransformOnce = true;
        //    }

        // --------------





        // --------------






        //  if (sendLocation)  print("location = "+location);

        object[] parms = new object[7] { message, type, subject, sendLocation, location, sendUseLocalCoordinates, extra };

        //		manager.SendMessage (method, message, subject);

        if (!starGameManagerRef.GamePaused)
        {
            manager.SendMessage(method, parms);
        }

        //		}

        gameObject.SetActive(false);

        //	}

        // METHOD = ADVANCE
        // MESSAGE = NEXT
    }
}
