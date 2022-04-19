using UnityEngine;
using System.Collections;

public class positionFollow : MonoBehaviour {

    public GameObject followObject;
    public bool followX, followY, followZ;
    public float offsetX, offsetY, offsetZ;
    float transX, transY, transZ;



	
	void Update () {

        //transX = transform.position.x;
        //transY = transform.position.y;
        //transZ = transform.position.z;

        //if (followX) transX = followObject.transform.position.x;
        //if (followY) transY = followObject.transform.position.y;
        //if (followZ) transZ = followObject.transform.position.z;

        transX = transform.position.x;
        transY = transform.position.y;
        transZ = transform.position.z;

        if (followX) transX = followObject.transform.position.x + offsetX;
        if (followY) transY = followObject.transform.position.y + offsetY;
        if (followZ) transZ = followObject.transform.position.z + offsetZ;




        transform.position = new Vector3 (transX, transY, transZ);
    }
}
