
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnHolder2 : MonoBehaviour
{

    GameObject picked;

    StarGameManager starGameManagerRef;

    [System.Serializable]

    public class instanceList

    {
        public string activateMessage = "";

        public GameObject source;

        public GameObject parentObject;
        [HideInInspector] public GameObject cloned;
        public string useName = "";
        public bool BombInhibitable;

    }

    public List<instanceList> InstanceList;


    public void Activate(object[] parms)




    {
        if (starGameManagerRef != null)
        {
            if (starGameManagerRef.InhibitEnemySpawns)
            {
                return;
            }
        }

        string received = (string)parms[0];
        string type = (string)parms[1];

        bool sendLocation = (bool)parms[3];
        Vector3 location = (Vector3)parms[4];
        bool useLocalCoordinates = (bool)parms[5];
        string extra = (string)parms[6];

        int i = 0;

        foreach (instanceList inList in InstanceList)
        {

            if (received == inList.activateMessage)
            {
                inList.useName = inList.source.name;

                  SpawnedCheckerLocal(i, type, extra, inList.parentObject, location, inList.useName, useLocalCoordinates);

            }

            i++;

        } // END FOREACH

    }
    void Start()
    {

        starGameManagerRef = StarGameManager.instance;

    }




     public void SpawnedCheckerLocal(int selectedSpawn, string animTrigger, string extra, GameObject newParent, Vector3 transmittedLocation,

string usedName, bool useLocalCoordinates)
    {


        GameObject useObject;
        useObject = null;
        
        useObject = starGameManagerRef.SpawnedChecker(InstanceList[selectedSpawn].source);
        

        // want to cut this if possible rma816


        if (useObject == null)
        {
            return;
        }
        

        //    useObject.transform.parent = InstanceList[selectedSpawn].parentObject.transform;

        if (InstanceList[selectedSpawn].parentObject != null)
        {
            useObject.transform.SetParent(InstanceList[selectedSpawn].parentObject.transform, false); // trying this now
        }

        // ----------------------------------------------------------------------------


        if (useLocalCoordinates) useObject.transform.localPosition = new Vector3(transmittedLocation.x, transmittedLocation.y, transmittedLocation.z);
        else
            useObject.transform.position = new Vector3(transmittedLocation.x, transmittedLocation.y, transmittedLocation.z);

        // this was orig localPosition


        if (useObject.GetComponentInChildren<starEnemy>())
        {
            useObject.GetComponentInChildren<starEnemy>().leader = true;

            useObject.GetComponentInChildren<starEnemy>().animPattern = animTrigger;
            useObject.GetComponentInChildren<starEnemy>().grandParent = newParent;
            useObject.GetComponentInChildren<starEnemy>().Extra = extra;

            starGameManagerRef.leaderList.Add(useObject); // CAN DELETE LATER 

        }

        useObject.SetActive(true);



    } //end spawnedChecker



}
