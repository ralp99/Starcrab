using UnityEngine;
using System.Collections;

public class goto1Position2 : MonoBehaviour {

	public Transform patrolPoint;
	public GameObject movingObject;
	public float moveSpeed;
	int currentPoint;
	public float offsetX, offsetY, offsetZ;
	public bool closeAtArrive;
	public float threshold = 0.1f;

    public bool ignoreX, ignoreY, ignoreZ;

	Vector3 useNewPosition;

    float origPosX, origPosY, origPosZ;
    float workingPosX, workingPosY, workingPosZ;
    Vector3 goalPos;

	void Start () {
        //movingObject.transform.position = patrolPoint.position;
        //print (patrolPoint.position);
        //		patrolPoint.position.x = patrolPoint.position.x + offsetX;
        //		patrolPoint.position.y = patrolPoint.position.x + offsetY;
        //		patrolPoint.position.z = patrolPoint.position.x + offsetZ;

        //origPosX = movingObject.transform.position.x;
        //origPosY = movingObject.transform.position.y;
        //origPosZ = movingObject.transform.position.z;



    }


    void Update () {


        if (ignoreX) workingPosX = movingObject.transform.position.x;
        else workingPosX = patrolPoint.position.x;

        if (ignoreY) workingPosY = movingObject.transform.position.y;
        else workingPosY = patrolPoint.position.y;

        if (ignoreZ) workingPosZ = movingObject.transform.position.z;
        else workingPosZ = patrolPoint.position.z;


        //     useNewPosition = new Vector3 (patrolPoint.position.x+offsetX,
        //patrolPoint.position.y+offsetY, patrolPoint.position.z+offsetZ);

        useNewPosition = new Vector3(workingPosX + offsetX,
     workingPosY + offsetY, workingPosZ + offsetZ);

        goalPos = new Vector3 (workingPosX, workingPosY, workingPosZ);



        if (closeAtArrive) 
		{
            //	if (movingObject.transform.position == useNewPosition) // VERY OLD


            //if ((movingObject.transform.position == useNewPosition) || (Vector3.Distance (movingObject.transform.position, patrolPoint.position) < threshold))
            if ((movingObject.transform.position == useNewPosition) || (Vector3.Distance(movingObject.transform.position, goalPos) < threshold))


                gameObject.SetActive (false);
		}

		movingObject.transform.position = Vector3.MoveTowards (movingObject.transform.position, useNewPosition, moveSpeed * Time.deltaTime);
		//movingObject.transform.position = patrolPoint.position;


	}
}
