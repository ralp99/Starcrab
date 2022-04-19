using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpawnItems { NA, OtherBullet, HeroBullet, SupplyBomb, SupplyLevelUp, SupplyStarman, EnemyPurple }
public enum PresetVert { Manual, A, B, C, D, E }
public enum PresetHoriz { Manual, A, B, C, D, E }
public enum PresetDepth { Manual, A, B, C, D, E }
public enum SpawnPositionPresetCoordinate { Depth, Horiz, Vert }


public class StarSpawnManager : MonoBehaviour {
    public enum OverrideObject { NA, StageShape, HeroShip }

    StarGameManager starGameManagerRef;
    public ShotDirection ShotDirection;
    public PresetDepth PresetDepth;
    public PresetHoriz PresetHoriz;
    public PresetVert PresetVert;

    public Vector3 V3Coords;
    public string AnimTrigger = "";
    public OverrideObject overrideObject;
    public GameObject ParentObject;



    [System.Serializable]

    public class instanceList

    {
        public string note = "";
        public SpawnItems SpawnItem;
        public ShotDirection ShotDirection;
        public string AnimTriggerA = "";
        public string AnimTriggerB = "";
       // [HideInInspector]
      //  public string ExtraAnimTrigger = "";
        public GameObject Prefab;
        public OverrideObject v3CoordsOverrideObject;
        public GameObject V3CoordsOverride;
        public bool BombInhibitable = true;
    }
    
    private float horizA, horizB, horizC, horizD, horizE;
    private float vertA, vertB, vertC, vertD, vertE;
    private float depthA, depthB, depthC, depthD, depthE;

    private float rangeA = 0.0f;
    private float rangeB = 0.25f;
    private float rangeC = 0.5f;
    private float rangeD = 0.75f;
    private float rangeE = 1.0f;

    public float HorizRangeLo = -0.084f;
    public float HorizRangeHi = 0.088f;
    public float VertRangeLo = -0.064f;
    public float VertRangeHi = 0.0925f;
    public float DepthRangeLo = 0.49f;
    public float DepthRangeHi = 1.0f;

    [HideInInspector]
    public int spawnDepthLength;
    [HideInInspector]
    public int spawnHorizLength;
    [HideInInspector]
    public int spawnVertLength;

    void Start()
    {
        starGameManagerRef = StarGameManager.instance;
        AssignPresetValues();
        spawnDepthLength = System.Enum.GetValues(typeof(PresetDepth)).Length;
        spawnHorizLength = System.Enum.GetValues(typeof(PresetHoriz)).Length;
        spawnVertLength = System.Enum.GetValues(typeof(PresetVert)).Length;
    }
    

    private void AssignPresetValues()
    {
        horizA = GetPresetPos(HorizRangeLo, HorizRangeHi, rangeA);
        horizB = GetPresetPos(HorizRangeLo, HorizRangeHi, rangeB);
        horizC = GetPresetPos(HorizRangeLo, HorizRangeHi, rangeC);
        horizD = GetPresetPos(HorizRangeLo, HorizRangeHi, rangeD);
        horizE = GetPresetPos(HorizRangeLo, HorizRangeHi, rangeE);

        vertA = GetPresetPos(VertRangeLo, VertRangeHi, rangeA);
        vertB = GetPresetPos(VertRangeLo, VertRangeHi, rangeB);
        vertC = GetPresetPos(VertRangeLo, VertRangeHi, rangeC);
        vertD = GetPresetPos(VertRangeLo, VertRangeHi, rangeD);
        vertE = GetPresetPos(VertRangeLo, VertRangeHi, rangeE);

        depthA = GetPresetPos(DepthRangeLo, DepthRangeHi, rangeA);
        depthB = GetPresetPos(DepthRangeLo, DepthRangeHi, rangeB);
        depthC = GetPresetPos(DepthRangeLo, DepthRangeHi, rangeC);
        depthD = GetPresetPos(DepthRangeLo, DepthRangeHi, rangeD);
        depthE = GetPresetPos(DepthRangeLo, DepthRangeHi, rangeE);
    }


    private float GetPresetPos(float rangeLo, float rangeHi, float rangePosition)
    {
        return Mathf.Lerp(rangeLo, rangeHi, rangePosition);
    }

    void AssignPresetPositions()
    {
        

        float spawnYcoords = V3Coords.y;

        switch (PresetDepth)
        {
            case PresetDepth.Manual:
                break;
            case PresetDepth.A:
                spawnYcoords = depthA;
                break;
            case PresetDepth.B:
                spawnYcoords = depthB;
                break;
            case PresetDepth.C:
                spawnYcoords = depthC;
                break;
            case PresetDepth.D:
                spawnYcoords = depthD;
                break;
            case PresetDepth.E:
                spawnYcoords = depthE;
                break;
            default:
                break;
        }
      

        float spawnXcoords = V3Coords.x;

        switch (PresetHoriz)
        {
            case PresetHoriz.Manual:
                break;
            case PresetHoriz.A:
                spawnXcoords = horizA;
                break;
            case PresetHoriz.B:
                spawnXcoords = horizB;
                break;
            case PresetHoriz.C:
                spawnXcoords = horizC;
                break;
            case PresetHoriz.D:
                spawnXcoords = horizD;
                break;
            case PresetHoriz.E:
                spawnXcoords = horizE;
                break;
            default:
                break;
        }

        float spawnZcoords = V3Coords.z;


        switch (PresetVert)
        {
            case PresetVert.Manual:
                break;
            case PresetVert.A:
                spawnZcoords = vertA;
                break;
            case PresetVert.B:
                spawnZcoords = vertB;
                break;
            case PresetVert.C:
                spawnZcoords = vertC;
                break;
            case PresetVert.D:
                spawnZcoords = vertD;
                break;
            case PresetVert.E:
                spawnZcoords = vertE;
                break;
            default:
                break;
        }
        
        V3Coords = new Vector3(spawnXcoords, spawnYcoords, spawnZcoords);
    }

    
    public List<instanceList> InstanceList;

   /* public void SetManualCoords(Vector3 v3coords)
    {
       V3Coords = v3coords;
    }
    */

        public void SetManualCoords (SpawnPositionPresetCoordinate spawnPositionPresetCoordinate, float coordValue)
    {
        switch (spawnPositionPresetCoordinate)
        {
            case SpawnPositionPresetCoordinate.Depth:
                V3Coords.y = coordValue;
                break;
            case SpawnPositionPresetCoordinate.Horiz:
                V3Coords.x = coordValue;
                break;
            case SpawnPositionPresetCoordinate.Vert:
                V3Coords.z = coordValue;
                break;
            default:
                break;
        }
    }

    

    void CheckOverrideAssignment(instanceList overrideList)
    {
        if (overrideList.V3CoordsOverride == null)
        {
            if (overrideList.v3CoordsOverrideObject == OverrideObject.HeroShip)
            {
                overrideList.V3CoordsOverride = starGameManagerRef.HeroShip;
            }

            if (overrideList.v3CoordsOverrideObject == OverrideObject.StageShape)
            {
                overrideList.V3CoordsOverride = starGameManagerRef.ActorsParent;
            }

        }
    }

    void CheckParentOverrideAssignment()
    {
        if (ParentObject == null)
        {
            if (overrideObject == OverrideObject.StageShape)
            {
                ParentObject = starGameManagerRef.ActorsParent;
            }
        }
    }




    public void SimpleSpawn(int spawnInt)
    {
        instanceList currentList = InstanceList[spawnInt];
        CheckOverrideAssignment(currentList);
        
        DeploySpawn(currentList.Prefab, currentList.ShotDirection, currentList.AnimTriggerA,
            currentList.AnimTriggerB, currentList.SpawnItem, currentList.V3CoordsOverride, currentList.v3CoordsOverrideObject);
    }


    void DeploySpawn(GameObject currentObject, ShotDirection shotDirection, string animTriggerA, string animTriggerB, SpawnItems spawnItems, GameObject v3coordsOverride, 
        OverrideObject v3CoordsOverrideObject)
    {
        GameObject currentSpawn = null;

        currentSpawn = starGameManagerRef.SpawnedChecker(currentObject);
        if (currentSpawn == null)
        {
            return;
        }

        CheckParentOverrideAssignment();
        currentSpawn.transform.parent = ParentObject.transform;


        if (v3coordsOverride == null)
        {
            AssignPresetPositions();
        }
        else
        {
            V3Coords = new Vector3(v3coordsOverride.transform.localPosition.x, v3coordsOverride.transform.localPosition.y,
                v3coordsOverride.transform.localPosition.z);
        }

        currentSpawn.transform.localPosition = V3Coords;
        ShotDirection currentShotDirection = shotDirection;

        if (ShotDirection != ShotDirection.NA)
        {
            currentShotDirection = ShotDirection;
        }


        currentSpawn.transform.localEulerAngles =
            starGameManagerRef.AdjustShotDirection(currentShotDirection);


        if (currentSpawn.GetComponentInChildren<starEnemy>())  // these are for starEnemy followers
        {
            currentSpawn.GetComponentInChildren<starEnemy>().leader = true;
            currentSpawn.GetComponentInChildren<starEnemy>().grandParent = currentSpawn.transform.parent.gameObject;
            starGameManagerRef.leaderList.Add(currentSpawn); // CAN DELETE LATER 
        }

        currentSpawn.SetActive(true);

        //    print("spawned item activated");

        if (currentSpawn.GetComponentInChildren<Animator>())
        {
            /*
            if (inList.AnimTriggerA != "")
            {
                currentSpawn.GetComponentInChildren<Animator>().SetTrigger(inList.AnimTriggerA);
            }
            */

            RunAnimTrigger(currentSpawn, animTriggerA);
            RunAnimTrigger(currentSpawn, animTriggerB);
            // RunAnimTrigger(currentSpawn, inList.ExtraAnimTrigger);
        }

        RunSupplySpecialTriggers(currentSpawn, spawnItems);
        
    }  // end deploySpawn




    public void SpawnItem(SpawnItems thisSpawn)
    {
        foreach (instanceList inList in InstanceList)
        {
            if (thisSpawn == inList.SpawnItem)
            {
                CheckOverrideAssignment(inList);
                DeploySpawn(inList.Prefab, inList.ShotDirection, inList.AnimTriggerA, inList.AnimTriggerB, inList.SpawnItem, inList.V3CoordsOverride, inList.v3CoordsOverrideObject);
            }
        }
    }


    void RunSupplySpecialTriggers(GameObject currentObject, SpawnItems spawnItem)
    {

        // can replace all this stuff, and inside of starEnemy, with reference to eNum
        string currentString = "";
        switch (spawnItem)
        {
            case SpawnItems.NA:
                break;
            case SpawnItems.OtherBullet:
                break;
            case SpawnItems.HeroBullet:
                break;
            case SpawnItems.SupplyBomb:
                currentString = "Bomb";
                    break;
            case SpawnItems.SupplyLevelUp:
                currentString = "LevelUp";
                break;
            case SpawnItems.SupplyStarman:
                currentString = "Starman";
                break;
            case SpawnItems.EnemyPurple:
                break;
            default:
                break;
        }

        if (currentString != "")
        {
            if (currentObject.GetComponentInChildren<starEnemy>())
            {
                currentObject.GetComponentInChildren<starEnemy>().Extra = currentString;
            }
        }
    }


    void RunAnimTrigger(GameObject currentObject, string currentTrigger)
    {
        if (currentTrigger != "")
        {
            currentObject.GetComponentInChildren<Animator>().SetTrigger(currentTrigger);

            if (currentObject.GetComponentInChildren<starEnemy>()) // if enemySpawn has followers, assign them to use same anim
            {
                currentObject.GetComponentInChildren<starEnemy>().animPattern = currentTrigger;
            }
        }
    }
	
}
