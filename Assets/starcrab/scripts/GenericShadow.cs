using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericShadow : MonoBehaviour {

    public enum YChangeUse {X, Y, Z}

    public GameObject HeightTracker;
    public GameObject ShadowMeshObject;
    public GameObject followObjectXZ;
    public GameObject followObjectY;

    public float SizeMultiplier = 1;
    public float MaximumSize = 0.04f;
    public float MinimumSize = 0.001f;

    float transX;
    float transY;
    float transZ;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float yChange = 0.0f;
    public float yChangeInit = 0.0f;

    StarGameManager starGameManagerRef;
    public float newScale = 1.0f;
    public YChangeUse yChangeUse = YChangeUse.Y;
    public bool KeepLinksOnDisable;

    public float HeightDifference;

    Transform ShadowCasterHeightPos;  
    Transform GroundHeightPos;
    
    void Start () {

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        //  yChangeInit = followObjectY.transform.localPosition.y;
          yChangeInit = 0;
      //  GroundHeight = starGameManagerRef.ShadowYObject.transform.position.y;

    }


    private void OnDisable()
    {
        if (KeepLinksOnDisable)
        {
            return;
        }

        followObjectXZ = null;
        followObjectY = null;
    }

    private void OnEnable()
    {
        // calculate current shadowSize

        if (followObjectXZ != null)
        {
            ShadowCasterHeightPos = followObjectXZ.transform;  // this shouldn't need a null check
        }

        if (starGameManagerRef != null)
        {
            GroundHeightPos = starGameManagerRef.ShadowYObject.transform;
        }

        ShadowMeshObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        // shdow won't be huge on init
    }


    private void CalculateCurrentShadowSize()
    {

        if (ShadowCasterHeightPos == null)
        {
            ShadowCasterHeightPos = followObjectXZ.transform;
        }

        HeightDifference = (HeightTracker.transform.localPosition.z - ShadowMeshObject.transform.localPosition.z);

        if (HeightDifference < 0)
        {
            HeightDifference = HeightDifference * -1;
        }
        
        newScale = SizeMultiplier / HeightDifference;

        
        if (newScale < MinimumSize)
        {
            newScale = MinimumSize;
        }

        if (newScale > MaximumSize)
        {
            newScale = MaximumSize;
        }
        

        ShadowMeshObject.transform.localScale = new Vector3(newScale, newScale, 1);
        
    }
    

    void Update () {


        if (StarGameManager.instance == null)
        {
            return;
        }


        if (starGameManagerRef != null)
        {
            if (starGameManagerRef.ShadowYObject != null)
            {
                followObjectY = starGameManagerRef.ShadowYObject;
            }
        }


        if (GroundHeightPos == null)
        {
            if (StarGameManager.instance.ShadowYObject != null)
            {
                GroundHeightPos = StarGameManager.instance.ShadowYObject.transform;
            }
        }

            if (followObjectXZ == null || followObjectY == null)
        {
            return;
        }

        transX = followObjectXZ.transform.position.x + offsetX;
        transZ = followObjectXZ.transform.position.z + offsetZ;
        transY = followObjectY.transform.position.y + offsetY;

        ShadowMeshObject.transform.position = new Vector3(transX, transY, transZ);
        HeightTracker.transform.position = followObjectXZ.transform.position;

        CalculateCurrentShadowSize();
    }
    

}


