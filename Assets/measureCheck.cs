using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class measureCheck : MonoBehaviour {


    public GameObject CheckAgainstObject;
    public Vector3 min, max;
    MeshFilter meshFilter;
    Renderer renderer;
    public bool inBounds;
    GameObject cornerObject;
    public GameObject MinObj;
    public GameObject MaxObject;
    public float distanceVecA, distanceVecB, distanceVecStage;

	// Use this for initialization
	void Start () {
        meshFilter = CheckAgainstObject.GetComponent<MeshFilter>();
        renderer = CheckAgainstObject.GetComponent<Renderer>();

        cornerObject = new GameObject();
        SpawnCornerObject();

        AttachToCorners();






    }

    void SpawnCornerObject()
    {
        Instantiate(cornerObject);
    }


	// Update is called once per frame
	void Update () {
        //   min = CheckAgainstObject.GetComponent<MeshFilter>().mesh.bounds.min;
        //   max = CheckAgainstObject.GetComponent<MeshFilter>().mesh.bounds.max;

        /*

        float zPos = gameObject.transform.position.z;

        if (zPos < max.z && zPos > min.z)
        {
            inBounds = true;
        }

        else
        {
            inBounds = false;
        }
        */

//AttachToCorners();

        //  float checkZ = transform.localPosition.z;
        Vector3 checkZ = transform.position;

         distanceVecA = Vector3.Distance(MinObj.transform.position, transform.position);
        distanceVecB = Vector3.Distance(MaxObject.transform.position, transform.position);
        distanceVecStage = Vector3.Distance(CheckAgainstObject.transform.position, transform.position);

        /*
        if (checkZ < MaxObject.transform.localPosition.z && checkZ > MinObj.transform.localPosition.z)
        {
            inBounds = true;
        }

        else
        {
            inBounds = false;
        }
        */




    }


    void AttachToCorners()
    {
        min = renderer.bounds.min;
        max = renderer.bounds.max;


        MinObj.transform.position = min;
        MaxObject.transform.position = max;
    }
}
