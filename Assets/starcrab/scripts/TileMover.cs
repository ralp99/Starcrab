using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMover : MonoBehaviour
{
    // instead of using an external object, have caboose spawn after current object moves X units

    


    public enum CheckAxis { XAxis, YAxis, ZAxis }
    public enum MoveAxis { XAxis, YAxis, ZAxis }
    public enum Bounds { Min, Max }

    public enum PerformWhen { Anytime, MoverIsNegative, MoverIsPositive }
    public enum Randomize { Off, RepeatsOK, NoRepeats };

    public MoveAxis moveAxis;
    public Bounds bounds;  // not currently used
    public float moveSpeed = 1.0f;
    public float TileScale = 1.0f;
    StarGameManager starGameManagerRef;
    StarSpawnManager spawnManager;
    private Vector3 currentPos;
    public bool useWorldSpace;
    private GameObject caboose;
    public CheckAxis checkAxis; // not currently used
    public PerformWhen performWhen;
    public Randomize tileGroupRandomize;
    public int tileGroupIterations = 1;
    private int completedIterations;
    public bool loopTileGroups, scrollWhenFinished;
    GameObject newParentObject = null;
    private bool canResetTilesetToNewLevel;

    SoTileObjectCreate useObject;

    Mesh mesh;
    Vector3 maxSide;

    [HideInInspector] public List<GameObject> tilePool = new List<GameObject>();

    public bool canMove = true;
    public bool createOnStart;
    public bool inheritPrefabRotation = true;
    public bool zeroMeshPosToHolder = true;
    int activateTileNumber;
    int currentCheckingNumber;

    // CUT OFF!!!! cut more above

    public int[] OrderOfTileGroups;

    public List<SoTileMoverTileGroup> TileGroups;
    
    public int drawLimit;
    private List<GameObject> builtList = new List<GameObject>();
    private List<GameObject> reverseList = new List<GameObject>();

    private List<int> dynamicTileGroupMembers = new List<int>();
    private int nextTileGroupCounter;
    //private SoTileObjectCreate nextTileListMember;
    private SoTileMoverTileGroup.TileSublist nextTileListMember;
    private bool advanceToNextGroup;
    private int currentActiveGroupID;
    private float cachedPos;
    private float checkingDistValue;
    // public int countIterations = 1; not currently used
    public GameObject tilesHolder;
    public Dictionary<string, float> spawnEdgeValues;
    private bool movingForward;
    // private float workingSpeed;
    public float workingSpeed;
    public bool PreBlastTilesAtInit;

    public bool TileSpeedSlowHalt;
    public bool TileSpeedResume;
    public float SmoothingTransition = 2.0f;
    private float prevSpeed;
    private bool canSetPrevSpeed = true;


    public void StartCreatingTiles(bool makeTiles)
    {

        if (makeTiles)
        {
            CreateNewChild();
        }

        else
        {
            // stop them somehow
        }
    }

    public void SetTileSpeedSlowHalt()
    {
        TileSpeedSlowHalt = true;
    }

    public void SetTileSpeedResume()
    {
        TileSpeedResume = true;
    }

    public void ReplaceTileMoverGroup(List<SoTileMoverTileGroup> tileMoverGroup)
    {
        // set this up to be utilized locally as well
        TileGroups.Clear();
        for (int i = 0; i < tileMoverGroup.Count; i++)
        {
            SoTileMoverTileGroup clonedTileGroup = Instantiate(tileMoverGroup[i]);
            TileGroups.Add(clonedTileGroup);
        }
    }
    

    public void ChangeCurrentLevel(int newLevel)
    {
        OrderOfTileGroups[0] = newLevel;

        canResetTilesetToNewLevel = true;

        // will start spawning from new tileSet only when next tileset should be built.
        // put it into tileCreate update
 
    }

     void ResetTileListToNewLevel()
    {
        
        canResetTilesetToNewLevel = false;

        starGameManagerRef.TilePoolActive.Clear();

        
        List<GameObject> DeleteObsoleteTilesList = new List<GameObject>();

        if (!PreBlastTilesAtInit)
        {
            foreach (GameObject obsoleteTile in starGameManagerRef.TilePoolInactive)
            {
                DeleteObsoleteTilesList.Add(obsoleteTile);
            }
        }
        // currently only working if preblast isn't enabled, as we don't want to destroy prebuilt tiles - since
        // preblasting builds from ALL sets


        starGameManagerRef.TilePoolInactive.Clear();

        if (!PreBlastTilesAtInit)
        {
            foreach (GameObject obsoleteTile in DeleteObsoleteTilesList)
            {
                Destroy(obsoleteTile);
            }
        }

        //  BeginBuildingTiles();  // not sure why but this is giving grief when new tileset begins, doesn't cut out old ones correctly. 
        // Temp hiding for now until I can debug it some more
        
    }




    void PreBlastTiles()
    {
        foreach (SoTileMoverTileGroup inList in TileGroups)
        {
            // foreach (SoTileObjectCreate currentTile in inList.tileList)
            foreach (SoTileMoverTileGroup.TileSublist currentTile in inList.tileList)

            {
                // print(" -- "+currentTile);
                GameObject blastedObject = null;
                
                Spawn(currentTile.TileSo);

                blastedObject = newParentObject;

                blastedObject.name = currentTile.TileSo.ParentTile.name;

                blastedObject.SetActive(true);

                if (!spawnEdgeValues.ContainsKey(blastedObject.name))
                {
                    spawnEdgeValues.Add(blastedObject.name, GetObjectBoundary(blastedObject));
                }

                blastedObject.SetActive(false);
            }
        }
    }
    
    

    void Start()

    {
        
        spawnEdgeValues = new Dictionary<string, float>();

        starGameManagerRef = StarGameManager.instance;
        spawnManager = starGameManagerRef.SpawnManager;
        
        for (int i = 0; i < TileGroups.Count; i++)
        {
            SoTileMoverTileGroup clonedTileGroup = Instantiate(TileGroups[i]);
            TileGroups[i] = clonedTileGroup;
        }
       

        if (starGameManagerRef == null)
        {
            workingSpeed = moveSpeed;
        }

        if (PreBlastTilesAtInit)
        {
            PreBlastTiles();  

            // maybe mod this to work with only "orderOfTileGroups"
            // as it currently builds temp objects from everything that's listed in all tileMover groups
        }


        BeginBuildingTiles();

    }  //END START

     void BeginBuildingTiles()
    {

        spawnEdgeValues.Clear();

        int instanceCounter = 0;

        if (tileGroupIterations < 1)
        {
            tileGroupIterations = 1;
        }

        if ((tileGroupRandomize == Randomize.NoRepeats || tileGroupRandomize == Randomize.RepeatsOK) && OrderOfTileGroups.Length < 2)
        {
            tileGroupRandomize = Randomize.Off;
        }

        if (tileGroupRandomize != Randomize.Off)
        {
           // ChooseRandomTileGroup();
        }




        foreach (SoTileMoverTileGroup inList in TileGroups)
        {
            inList.groupId = instanceCounter;
            instanceCounter++;

            if ((inList.randomize == Randomize.RepeatsOK || inList.randomize == Randomize.NoRepeats)
                && inList.tileList.Count < 2)
            {
                inList.randomize = Randomize.Off;
            }
        }

        GenerateDynamicGroupLists();

        advanceToNextGroup = true;

        if (createOnStart) CreateNewChild();

    }
    

    void GenerateDynamicGroupLists()
    {

        dynamicTileGroupMembers.Clear();

        for (int i = 0; i < OrderOfTileGroups.Length; i++)
            {

                int tempRandom = Random.Range(0, OrderOfTileGroups.Length);

                while (dynamicTileGroupMembers.Contains(tempRandom))
                {
                    tempRandom = Random.Range(0, OrderOfTileGroups.Length);
                }

                dynamicTileGroupMembers.Add(tempRandom);
            }
    }



    void BuiltListOrderReversal()
    {
        caboose = builtList[0];
        starGameManagerRef.Caboose = caboose;

        for (int i = builtList.Count-1; i >= 0; i--)
        {
            reverseList.Add(builtList[i]);
        }

        builtList.Clear();

        for (int i = 0; i < reverseList.Count; i++)
        {
            builtList.Add(reverseList[i]);
        }

        reverseList.Clear();
    }


    void ChangingTileSpeed(bool slower)
    {

        if (slower && canSetPrevSpeed)
        {
            canSetPrevSpeed = false;
            prevSpeed = moveSpeed;
        }

        float range = 0.01f;
        float destinationSpeed = 0.00001f;  // if set to 0, tiles change upon resume

        if (!slower)
        {
            destinationSpeed = prevSpeed;
            canSetPrevSpeed = true;
        }


        if (slower && moveSpeed <= range)

        {
            moveSpeed = destinationSpeed;
            TileSpeedSlowHalt = false;
            return;
        }

        if (!slower && moveSpeed >= prevSpeed - range)
        {
            moveSpeed = destinationSpeed;
            TileSpeedResume = false;
            return;
        }

          moveSpeed = Mathf.Lerp(moveSpeed, destinationSpeed, Time.deltaTime*SmoothingTransition);
    }
    

    IEnumerator TileSpeedBandaid()
    {
        yield return new WaitForSeconds(6);
        TileSpeedResume = false;
        TileSpeedSlowHalt = false;

    }

    void TilesHolderCheck()
    {
        if (tilesHolder == null)
        {
            tilesHolder = starGameManagerRef.TileHolderObject;
        }
    }

    void Update()

    {
   
        if (starGameManagerRef.GamePaused)
        {
            return;
        }

        TilesHolderCheck();

        if (TileSpeedSlowHalt)
        {
            TileSpeedResume = false;
            ChangingTileSpeed(true);
            // TileSpeedSlowHalt = false;
            StartCoroutine(TileSpeedBandaid());
        }

        if (TileSpeedResume)
        {
            TileSpeedSlowHalt = false;
            ChangingTileSpeed(false);
            //  TileSpeedResume = false;
            StartCoroutine(TileSpeedBandaid());
        }


        if (builtList.Count > 1)

        {
            if ((movingForward && workingSpeed < 0) || (!movingForward && workingSpeed > 0))
            {
                BuiltListOrderReversal();
            }
        }

        if (workingSpeed > 0)
        {
            movingForward = true;
        }

        else

        {
            movingForward = false;
        }

        if (starGameManagerRef != null)
        {
            workingSpeed = moveSpeed * starGameManagerRef.GameSpeed;
        }


        Vector3 useVector = new Vector3 (0,0,0);

     //   if (OrderOfTileGroups.Length > 0 && caboose != null) // un comment this

        {
            if (canMove)

            {

                if (moveAxis == MoveAxis.XAxis)
					useVector = new Vector3(workingSpeed, 0, 0);

                if (moveAxis == MoveAxis.YAxis)
					useVector = new Vector3(0, workingSpeed, 0);

                if (moveAxis == MoveAxis.ZAxis)
					useVector = new Vector3(0, 0, workingSpeed);

                // also try up, back, etc
                
            if (builtList.Count > 0)

                {
                    foreach (GameObject picked in builtList)
                    {
                        //  picked.transform.Translate(Vector3.up * moveSpeed);
                        //     picked.transform.localPosition(0,1,0 ); // WONT LET ME DO

                        //   picked.transform.localPosition += transform.forward * moveSpeed;
                        //                          picked.transform.localPosition += transform.localPosition = new Vector3 (1*moveSpeed,0,0);

                        if (picked != null)
                        {
                            picked.transform.localPosition += transform.localPosition = new Vector3(0, 0, 1 * workingSpeed) * Time.deltaTime;
                        }
                        else
                        {
                            canMove = false;
                        }


                    }
                }

            }

            //  if ( caboose != null && caboose.transform.localPosition.z < (cachedPos - checkingDistValue))  
			if ((!movingForward && caboose != null && caboose.transform.localPosition.z < (cachedPos - checkingDistValue))
                
                ||

				(movingForward && caboose != null && caboose.transform.localPosition.z > (cachedPos + checkingDistValue)))


            {
                cachedPos = 0;
              //  countIterations++;
                CreateNewChild();
            }
        }
    }  // end update

    

    void CreateNewChild()
    {

        if (canResetTilesetToNewLevel)
        {
            ResetTileListToNewLevel();
        }
        // checks if tileSet has been changed and clears old info out. 
        // This should only check as a new one is about to be spawned.
        //todo - maybe n clear out some values? only if some erratic things happen?


        if (advanceToNextGroup)
        {
            if (nextTileGroupCounter < OrderOfTileGroups.Length)
            {

                advanceToNextGroup = false;

                if (tileGroupRandomize == Randomize.Off)
                {
                    currentActiveGroupID = OrderOfTileGroups[nextTileGroupCounter];
                }

                if (tileGroupRandomize == Randomize.RepeatsOK)
                {
                    currentActiveGroupID = OrderOfTileGroups[Random.Range(0, OrderOfTileGroups.Length)];
                }

                if (tileGroupRandomize == Randomize.NoRepeats)
                {
                    currentActiveGroupID = OrderOfTileGroups[dynamicTileGroupMembers[nextTileGroupCounter]];
                }

            //    print("---------------------currentActiveGroupID " + currentActiveGroupID);

                nextTileGroupCounter++;

            }
            

            else

            {


                if (!loopTileGroups)

                {
                  //  print("SET COMPLETED");
          
                    if (tileGroupIterations-1 > completedIterations)
                    {
                        completedIterations++;
                        LoopIteration();
                        return;
                    }

                    if (!scrollWhenFinished)

                    {
                        canMove = false;
                    }

                    return;
                }


                else


                {
                    // does this only if looping
                    if (caboose != null)

                    {
                        LoopIteration();
                    }
                   
                    return;
                }
            }
        }


        // pick next tile object



        foreach (SoTileMoverTileGroup inList in TileGroups)
        {

            if (!inList.culledToBeginingElementAlready && inList.BeginningElement > 0)
            {
                inList.culledToBeginingElementAlready = true;

                for (int i = 0; i < inList.BeginningElement; i++)
                {
                       inList.tileList.RemoveAt(0);
                }
            }
            
                if (currentActiveGroupID != inList.groupId)
                { }

                else

                {

                if (inList.randomize == Randomize.NoRepeats && !inList.didRandomRoll)
                {
                    inList.dynamicTileListMembers.Clear();
                    inList.didRandomRoll = true;
                  //  /*
                    // rma89
                for (int i = 0; i < inList.tileList.Count; i++)
                    {
                        int tempRandom = Random.Range(0, inList.tileList.Count);

                          while (inList.dynamicTileListMembers.Contains(inList.tileList[tempRandom]))

                        {
                            tempRandom = Random.Range(0, inList.tileList.Count);
                        }

                          inList.dynamicTileListMembers.Add(inList.tileList[tempRandom]);


                    }
                 //   */
                }


                if (inList.currentIteration > inList.tileListIterations)

                    {
                    
                        advanceToNextGroup = true;
                        inList.currentListPosition = 0;
                        inList.currentIteration = 0;

                        // repopulate local tileList for subsequent use
                        if (inList.randomize == Randomize.NoRepeats)
                        {
                           inList.didRandomRoll = false;
                        }

                    CreateNewChild();
                        return;
                    }


                    if (inList.currentListPosition >= inList.tileList.Count)
                    {
                        inList.currentIteration++;
                        inList.currentListPosition = 0;
                        CreateNewChild();
                        return;

                    }



                // rma89
                if (inList.randomize == Randomize.Off)
                {
                    nextTileListMember = inList.tileList[inList.currentListPosition];
                }



                if (inList.randomize == Randomize.NoRepeats)
                {
                    nextTileListMember = inList.dynamicTileListMembers[inList.currentListPosition];
                }

                // rma89
                if (inList.randomize == Randomize.RepeatsOK)
                {
                nextTileListMember = inList.tileList[Random.Range(0, inList.tileList.Count)];
                }
                inList.currentListPosition++;

                GenerateNewMesh();
                    return;
                }
            }
    } // end CreateNewChild

    void LoopIteration()
    {
        nextTileGroupCounter = 0;

        if (tileGroupRandomize == Randomize.NoRepeats)
        {
            GenerateDynamicGroupLists();
        }

        advanceToNextGroup = true;

        CreateNewChild();
    }


    
      float  GetObjectBoundary(GameObject currentObject)

    {
        //         Mesh mesh = newParentObject.GetComponent<MeshFilter>().mesh;

        mesh = currentObject.GetComponent<MeshFilter>().mesh;
        maxSide = mesh.bounds.max;
        return maxSide.y;
    }
    
    public void SpawnResetAll(GameObject thisObject)
    {
        if (thisObject == null)
        {
            return;
        }

        thisObject.transform.parent = null;
        thisObject.transform.localPosition = Vector3.zero;

        thisObject.transform.localScale = Vector3.one;

       // rma2019_01_07 - not sure if it is cool to hide this, might need to stick a bool or find an alternate solution. I'll test it a bit first
       // either way, it needs to be addressed! (localScale) - scale should be able to inherit from a prefab instead of forcibly setting to 1

    }

    public void SpawnMakeChild(GameObject thisObject, GameObject parentObject)
    {
        if (thisObject == null)
        {
            return;
        }

        thisObject.transform.parent = parentObject.transform;

        PoolListMembership poolListMembership = parentObject.GetComponent<PoolListMembership>();
        poolListMembership.disableChildren.Add(thisObject.transform);

        if (!poolListMembership.unparentChildren.Contains(thisObject.transform))
        {
            poolListMembership.unparentChildren.Add(thisObject.transform);
        }
    }


    public void Spawn (SoTileObjectCreate currentSoTile)
    {
        newParentObject = null;
        GameObject newChildObject = null;

        newParentObject = starGameManagerRef.SpawnedChecker(currentSoTile.ParentTile);
        newChildObject = starGameManagerRef.SpawnedChecker(currentSoTile.ChildTile);

        List<GameObject> featureObjectList = new List<GameObject>();

        foreach (SoTileObjectCreate.FeatureList inList in currentSoTile.featureList)

        {
            GameObject featureObject = null;
            featureObject = starGameManagerRef.SpawnedChecker(inList.FeatureObject);
            SpawnResetAll(featureObject);

            // If Feature object already has a parent, it needs to disassociate from it
            // otherwise when the other parent is deactivated, this will still be in its
            // list and deactivate as well

            GameObject pooledParent = featureObject.GetComponent<PoolListMembership>().PooledParent;

            if (pooledParent != null)
            {
                pooledParent.GetComponent<PoolListMembership>().PooledChildWasDisabled(featureObject);
                pooledParent = null;
            }

            featureObjectList.Add(featureObject);
            featureObject.GetComponent<PoolListMembership>().PooledParent = newParentObject;
        }

        SpawnResetAll(newParentObject);
        SpawnResetAll(newChildObject);

        newParentObject.transform.localRotation = Quaternion.Euler(currentSoTile.ParentTileRotOffset);
        newChildObject.transform.localRotation = Quaternion.Euler(currentSoTile.ChildTileRotOffset);

        int iterationCount = 0;
        foreach (SoTileObjectCreate.FeatureList inList in currentSoTile.featureList)

        {
            featureObjectList[iterationCount].transform.localRotation = Quaternion.Euler(inList.FeatureRotOffset);
            iterationCount++;
        }


        SpawnMakeChild(newChildObject, newParentObject);

        iterationCount = 0;

        foreach (SoTileObjectCreate.FeatureList inList in currentSoTile.featureList)

        {
         
            GameObject currentFeatureObject = featureObjectList[iterationCount];
            SpawnMakeChild(currentFeatureObject, newParentObject);
            currentFeatureObject.transform.localPosition = inList.FeaturePosOffset;
            currentFeatureObject.SetActive(true);
            iterationCount++;
        }


        Mesh mesh = newParentObject.GetComponent<MeshFilter>().mesh;

        Vector3 maxSide = mesh.bounds.max;

        newChildObject.transform.localPosition = new Vector3(
         (maxSide.x * newParentObject.transform.localScale.x), newChildObject.transform.localPosition.y,
         newChildObject.transform.localPosition.z);

        newChildObject.SetActive(true);
        
    }





    void GenerateNewMesh()
    {

        // print("creating " + nextTileListMember.name);

       GameObject generatedObject = null;

        // this might need to be reintroduced somehow

      //  /*
        for (int i = 0; i < tilePool.Count; i++)
        {

            if (tilePool[i] != null)
            {
                if (tilePool[i].name == nextTileListMember.TileSo.name && generatedObject == null)
                {
                    generatedObject = tilePool[i];
                    tilePool.Remove(generatedObject);
                }
            }
        }
       // */

        if (generatedObject == null)
        {
            //  generatedObject = Instantiate(nextTileListMember.ParentTile) as GameObject;

            // duping the below at blast, maybe consolidate??

            Spawn(nextTileListMember.TileSo);
            generatedObject = newParentObject;

            generatedObject.name = nextTileListMember.TileSo.ParentTile.name;

            if (nextTileListMember.specialAction == SoTileMoverTileGroup.SpecialAction.HaltTiles)
            {
                SetTileSpeedSlowHalt();
            }



            if (spawnManager != null)
            {

                foreach (SoTileMoverTileGroup.TileSublist.instanceSpawnList inList in nextTileListMember.InstanceSpawnList)
                {
                    // assign depth, horiz, and vert values

                    if (inList.PresetDepth == PresetDepth.Manual)

                    {
                        spawnManager.SetManualCoords(SpawnPositionPresetCoordinate.Depth,
                            inList.SpawnItemCoords.y);
                    }

                    else
                    {
                        spawnManager.PresetDepth = inList.PresetDepth;
                    }
                    
                    if (inList.PresetHoriz == PresetHoriz.Manual)
                    {
                        spawnManager.SetManualCoords(SpawnPositionPresetCoordinate.Horiz,
                            inList.SpawnItemCoords.x);
                    }

                    else
                    {
                        spawnManager.PresetHoriz = inList.PresetHoriz;
                    }

                    if (inList.PresetVert == PresetVert.Manual)
                    {
                        spawnManager.SetManualCoords(SpawnPositionPresetCoordinate.Vert,
                            inList.SpawnItemCoords.z);
                    }

                    else
                    {
                        spawnManager.PresetVert = inList.PresetVert;
                    }

                    // assign other values

                    if (inList.ShotDirection != ShotDirection.NA)
                    {
                        spawnManager.ShotDirection = inList.ShotDirection;
                    }

                    if (inList.spawnItems != SpawnItems.NA)
                    {
                        spawnManager.SpawnItem(inList.spawnItems);
                    }



                }
            }

            
            if (!spawnEdgeValues.ContainsKey(generatedObject.name))
            {
                   spawnEdgeValues.Add(generatedObject.name, GetObjectBoundary(generatedObject));

            }
        }


        // generatedObject.SetActive(false);

        TilesHolderCheck();

        if (tilesHolder != null)

        {
            generatedObject.transform.parent = tilesHolder.transform;   // need to reenable
        }

        if (caboose != null)

        {

	 		if (!movingForward)

            {
				mesh = caboose.GetComponent<MeshFilter>().mesh;
				maxSide = mesh.bounds.max;

                generatedObject.transform.localPosition = new Vector3(caboose.transform.localPosition.x, caboose.transform.localPosition.y,
                 caboose.transform.localPosition.z + (maxSide.y * caboose.transform.localScale.y));
            }

            else

            {
				mesh = generatedObject.GetComponent<MeshFilter>().mesh;
				maxSide = mesh.bounds.max;

                generatedObject.transform.localPosition = new Vector3(caboose.transform.localPosition.x, caboose.transform.localPosition.y,
				(caboose.transform.localPosition.z - (maxSide.y* caboose.transform.localScale.y) ));
				
            }


            generatedObject.transform.localScale = caboose.transform.localScale;

			if (inheritPrefabRotation)
			{
                generatedObject.transform.localRotation = caboose.transform.localRotation;
			}

        }

        else

        {

            // init part - there is no caboose, THIS IS THE FIRST ROTATION APPLICATION

            TilesHolderCheck();

            if (tilesHolder != null)

            {
                //useObject.transform.localScale = tilesHolder.transform.localScale;
                generatedObject.transform.localScale = new Vector3(TileScale, TileScale, TileScale);


                if (inheritPrefabRotation)
				
				{
					Transform baseTransform = generatedObject.transform;
					Quaternion baseRotation = baseTransform.rotation;
					generatedObject.transform.localRotation = baseRotation;
				}

				if (zeroMeshPosToHolder)
					
				{
					generatedObject.transform.localPosition = new Vector3 (0,0,0); 
				}

            }

            
        }

        caboose = generatedObject;
        starGameManagerRef.Caboose = caboose;

        checkingDistValue = spawnEdgeValues[caboose.name];

        generatedObject.SetActive(true);

        if (drawLimit != 0)

        {
            builtList.Add(generatedObject);
            if (builtList.Count > drawLimit)

            {
                GameObject deactivateTile = builtList[0];
                deactivateTile.SetActive(false);
                tilePool.Add(deactivateTile);
                builtList.Remove(deactivateTile);
            }

        }

    } // end GenerateNewMesh
}