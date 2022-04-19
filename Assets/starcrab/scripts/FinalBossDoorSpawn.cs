using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossDoorSpawn : MonoBehaviour {


    public GameObject[] DamagedModels;
    public GameObject SpawnPointTop;
    public GameObject SpawnPointBottom;
    public GameObject WatchExtLimitObject;
    public GameObject[] CannonDummies;
    public GameObject Cannon;
    public float[] cannonThresholds;
    List<GameObject> cannonList = new List<GameObject>();

    StarGameManager starGameManagerRef;

    public List<GameObject> ActiveBodies = new List<GameObject>();

    void Start () {

        starGameManagerRef = StarGameManager.instance;

        foreach (GameObject picked in CannonDummies)
        {
            GameObject newCannon = Instantiate(Cannon) as GameObject;
            newCannon.transform.parent = picked.transform;
            newCannon.transform.localPosition = Vector3.zero;
            cannonList.Add(newCannon);
            newCannon.SetActive(true);
            picked.SetActive(false);
        }
    }

    public void SpawnBrokenMeshes()
    {

        int topModelValue = Random.Range(0, DamagedModels.Length);
        int bottomModelValue = 0;

        while (bottomModelValue == 0 || bottomModelValue == topModelValue)
        {
            bottomModelValue = Random.Range(0, DamagedModels.Length);
        }

        GameObject topModel = DamagedModels[topModelValue];
        GameObject bottomModel = DamagedModels[bottomModelValue];

        SpawnDamagedDoor(topModel, SpawnPointTop);
        SpawnDamagedDoor(bottomModel, SpawnPointBottom);
    }


    void SpawnDamagedDoor(GameObject spawnObject, GameObject spawnPoint)
    {
        GameObject useObject = Instantiate(spawnObject) as GameObject;

        useObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, false);
        useObject.transform.position = spawnPoint.transform.position;
        useObject.transform.rotation = spawnPoint.transform.rotation;

        if (Random.Range(0, 2) == 1)
        {
            useObject.transform.localScale = new Vector3(useObject.transform.localScale.x * -1, useObject.transform.localScale.y, useObject.transform.localScale.z);
        }

        useObject.SetActive(true);
    }
    
    
    private void Update()
    {
        // would love to check this with a WHILE instead
        for (int i = 0; i < CannonDummies.Length; i++)
        {
            bool activeState = false;

            if (WatchExtLimitObject.transform.localPosition.y < cannonThresholds[i])
            {
                activeState = true;
            }
            CannonDummies[i].SetActive(activeState);
        }
        
    }

}
