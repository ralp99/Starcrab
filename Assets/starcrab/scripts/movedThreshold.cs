using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movedThreshold : MonoBehaviour
{

    [System.Serializable]

    public class instanceList

    {


        public GameObject objectChecking;
        public GameObject[] objectsHide;
        public GameObject[] objectsShow;
        public MonoBehaviour[] monoHide;
        public MonoBehaviour[] monoShow;
        public bool useWorldSpace;
        // public bool checkX0, checkY0, checkZ0;
        public float value;
        public bool checkZero;
        public bool lessThan;
        public bool X, Y, Z;


        [HideInInspector]
        public Vector3 objectPosition;


        public bool display;
        public float printValue;
        // public float printY;
        // public float printZ;
        [HideInInspector]
        public bool canDo;
        public bool mute;
    }

    public List<instanceList> InstanceList;




    void checkStuff(float objCheck)
    {

        foreach (instanceList inList in InstanceList)
        {
            if (inList.display)
                inList.printValue = objCheck;
            if (inList.lessThan)
            {
                if (objCheck < inList.value)
                    inList.canDo = true;
            }
            else if (objCheck > inList.value)
                inList.canDo = true;
        }

    }





    void Update()
    {

        foreach (instanceList inList in InstanceList)
        {

            if (!inList.mute)
            {
                inList.canDo = false;
                if (inList.useWorldSpace)
                    inList.objectPosition = inList.objectChecking.transform.position;
                else
                    inList.objectPosition = inList.objectChecking.transform.localPosition;

                float objX = inList.objectPosition.x;
                float objY = inList.objectPosition.y;
                float objZ = inList.objectPosition.z;

                if (inList.X)
                    checkStuff(objX);
                if (inList.Y)
                    checkStuff(objY);
                if (inList.Z)
                    checkStuff(objZ);

                if (inList.canDo)
                {
                    foreach (GameObject picked in inList.objectsHide)
                    {
                        picked.SetActive(false);
                    }

                    foreach (GameObject picked in inList.objectsShow)
                    {
                        picked.SetActive(true);
                    }

                    foreach (MonoBehaviour picked in inList.monoHide)
                    {
                        picked.enabled = false;
                    }

                    foreach (MonoBehaviour picked in inList.monoShow)
                    {
                        picked.enabled = true;
                    }
                }
            }



        }
    }





}
