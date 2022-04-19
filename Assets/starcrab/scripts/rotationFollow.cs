using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationFollow : MonoBehaviour
{

    public GameObject followObject;
    public bool followX, followY, followZ;
    public float offsetX, offsetY, offsetZ;
    float transX, transY, transZ;

    public bool useLocal;


    void Update()
    {

        //transX = transform.position.x;
        //transY = transform.position.y;
        //transZ = transform.position.z;

        //if (followX) transX = followObject.transform.position.x;
        //if (followY) transY = followObject.transform.position.y;
        //if (followZ) transZ = followObject.transform.position.z;



        if (useLocal)

        //{
        //    transX = transform.localRotation.x;
        //    transY = transform.localRotation.y;
        //    transZ = transform.localRotation.z;

        //    if (followX) transX = followObject.transform.localRotation.x + offsetX;
        //    if (followY) transY = followObject.transform.localRotation.y + offsetY;
        //    if (followZ) transZ = followObject.transform.localRotation.z + offsetZ;


        //    //  transform.Rotate (transX, transY, transZ); // IT JUST SPINS


        //    //   transform.localRotation = new Vector3(transX, transY, transZ);
        //    // Quaternion.Euler(activerotationX,0,0);

        //    //    transform.localRotation = Quaternion.Euler(transX, transY, transZ);

        // transform.localRotation =  Quaternion.Euler(transX, transY, transZ);

        //}





        {
            transX = transform.localEulerAngles.x;
            transY = transform.localEulerAngles.y;
            transZ = transform.localEulerAngles.z;

               if (followX) transX = followObject.transform.localEulerAngles.x + offsetX;
               if (followY) transY = followObject.transform.localEulerAngles.y + offsetY;
               if (followZ) transZ = followObject.transform.localEulerAngles.z + offsetZ;

            transform.rotation = Quaternion.Euler(transX, transY, transZ);

        }




        else

        {

            transX = transform.rotation.x;
            transY = transform.rotation.y;
            transZ = transform.rotation.z;


            if (followX) transX = followObject.transform.position.x + offsetX;
            if (followY) transY = followObject.transform.position.y + offsetY;
            if (followZ) transZ = followObject.transform.position.z + offsetZ;

            transform.rotation = Quaternion.Euler(transX, transY, transZ);



        }


    }
}
