using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarExhaust : MonoBehaviour {


    public GameObject[] ExhaustSpawnPoint;
    public GameObject ExhaustPrefab;
    public Material ExhaustMaterial;
    public float Scale;
    public Vector3 Rotation;
    public bool ActivateAtLaunch = true;

    private List<GameObject> exhaustList = new List<GameObject>();

    StarGameManager starGameManagerRef;

    public void CreateExhausts(bool state)
    {
        exhaustList.Clear();

        for (int i = 0; i < ExhaustSpawnPoint.Length; i++)
        {
            GameObject useObject = null;
            useObject = starGameManagerRef.SpawnedChecker(ExhaustPrefab);
            if (useObject == null)
            {
                return;
            }

            exhaustList.Add(useObject);
            useObject.transform.SetParent(ExhaustSpawnPoint[i].transform, false);
            useObject.transform.localEulerAngles = Rotation;

            if (Scale != 0)
            {
                Vector3 newScale = new Vector3(Scale, Scale, Scale);
                useObject.transform.localScale = newScale;
            }

            if (ExhaustMaterial != null)
            {
                useObject.GetComponentInChildren<Renderer>().material = ExhaustMaterial;
            }

            useObject.SetActive(state);
        }
    }

    private void OnDisable()
    {
        ExhaustListSetting(false);
    }

    private void OnEnable()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        CreateExhausts(ActivateAtLaunch);
    }

    public void ExhaustListSetting (bool state)
    {
        for (int i = 0; i < exhaustList.Count; i++)
        {
            exhaustList[i].SetActive(state);
        }
    }

}
