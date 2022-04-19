using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossPhase { BossPhase1, BossPhase2, BossPhase3, BossPhase4 }
public enum ShotDirection { NA, Forward, Backward, Left, Right, Above, Below }
public enum BossType { Wall, Surround, Basher, Final, NA}


public class BossControllerGeneric : MonoBehaviour {



    [HideInInspector]
    public BossType bossType;
    [HideInInspector] public bool BossDefeated;
    [HideInInspector] public StarGameManager starGameManagerRef;
    [HideInInspector] public BossPhase bossPhase = BossPhase.BossPhase1;
    [HideInInspector] public AudioSource audioSource;

    public List<GameObject> ActiveBodies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> DisabledSubObjects = new List<GameObject>();
    [HideInInspector]
    public bool ActiveBodyBeingCleared;
    [HideInInspector]
    public bool AllActiveBodiesCleared;
    [HideInInspector]
    public bool ChildCollisionAlert;

    [HideInInspector]
    public GameObject CurrentProjectile;

    public GameObject[] UserTargetReticleSpawnPoints;
    public GameObject[] UserTargetReticleSpawnPointsSecondary;

    public GameObject UserTargetReticlePrefab;
    public Vector3 ReticleOffset;
    public float DelayReticleInitTimer = 7.0f;
    public float DelayReticleSecondaryTimer = 0.5f;

    [HideInInspector]
    List<GameObject> ReticleMeshList = new List<GameObject>();

    public Dictionary<GameObject, GameObject> ReticleActiveDictionary =
      new Dictionary<GameObject, GameObject>();

    public string ReticleActivate = "reset";
    public string ReticleShrink = "shrink";

    public bool SpawnSupplies;
    public float InitDelaySupplySpawn = 10.0f;
    public float DelayBetweenSupplySpawns = 5.0f;
    public float DelayBetweenJitter = 2.0f;
    StarSpawnManager spawnManager;
    int spawnDepthLength;
    int spawnHorizLength;
    int spawnVertLength;

    public SoWarningMessage soWarningMessage;
    public float WarningMessageDelay = 0.5f;
    [HideInInspector]
    public bool RanGenericBossTasks;


    public void RunGenericBossTasks()
    {
        RanGenericBossTasks = true;
        if (SpawnSupplies)
        {
            RunSupplySpawner();
        }

        if (UserTargetReticleSpawnPoints.Length != 0)
        {
            SpawnReticles(UserTargetReticleSpawnPoints, DelayReticleInitTimer);
        }
       
    }


    public void RunBossWarningMessage()
    {
        StarGameManagerCheck(); // should likely only need to call this here

        if (starGameManagerRef.EnableBossWarnings)
        {
            if (soWarningMessage != null)
            {
                starGameManagerRef.BossWarningMessagePlaying = true;
                starGameManagerRef.UiManager.PlayWarningMessage(soWarningMessage);
            }
        }
        else
        {
            starGameManagerRef.BossWarningMessagePlaying = false;
        }
    }


    void StarGameManagerCheck()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
    }

    public void RunSupplySpawner()
    {

        if (starGameManagerRef.gameplayMode != StarGameManager.GameplayMode.Showcase)
        {
            return;
        }

        StartCoroutine(BeginSupplySpawnTimer());
    }


    IEnumerator BeginSupplySpawnTimer()
    {
        spawnManager = starGameManagerRef.SpawnManager;
        spawnDepthLength = spawnManager.spawnDepthLength;
        spawnHorizLength = spawnManager.spawnHorizLength;
        spawnVertLength = spawnManager.spawnVertLength;
        
        yield return new WaitForSeconds(InitDelaySupplySpawn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        CreateSupplySpawns();
    }

    
    IEnumerator ContinueSupplySpawn()
    {
        yield return new WaitForSeconds(DelayBetweenSupplySpawns + Random.Range(0,DelayBetweenJitter));

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        CreateSupplySpawns();
    }

    void CreateSupplySpawns()
    {

        // spawnManager.PresetDepth = (PresetDepth)Random.Range(0,spawnDepthLength);
        spawnManager.PresetDepth = PresetDepth.E;
        spawnManager.PresetHoriz = (PresetHoriz)Random.Range(0, spawnHorizLength);
        spawnManager.PresetVert = (PresetVert)Random.Range(0, spawnVertLength);

        float randomPick = Random.Range(0.0f, 1.0f);

        if (randomPick > 0.5)
        {
            spawnManager.SpawnItem(SpawnItems.SupplyLevelUp);
        }
        else
        {
            spawnManager.SpawnItem(SpawnItems.SupplyStarman);
        }
        StartCoroutine(ContinueSupplySpawn());
    }

    public void SpawnReticles(GameObject[] reticleList, float delayTimer)
    {

        if (starGameManagerRef.gameplayMode != StarGameManager.GameplayMode.Showcase)
        {
            return;
        }

        for (int i = 0; i < reticleList.Length; i++)

        {
            GameObject currentSpawn = Instantiate(UserTargetReticlePrefab) as GameObject;
            GameObject currentSpawnPoint = reticleList[i];
            currentSpawn.transform.parent = currentSpawnPoint.transform.parent.transform;
            currentSpawn.transform.localPosition = currentSpawnPoint.transform.localPosition + ReticleOffset;
            currentSpawn.SetActive(true);

            ReticleMeshList.Add(currentSpawn);

            if (ActiveBodies.Count != 0)
            {
                ReticleActiveDictionary.Add(ActiveBodies[i], currentSpawn);
            }
        }

        StartCoroutine(ReticleInitAnim(delayTimer));
    }

    IEnumerator ReticleInitAnim(float delayTimer)
    {
        yield return new WaitForSeconds(delayTimer);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        AnimateReticles(ReticleActivate);
    }


    public void AnimateReticles(string message)
    {
        foreach (GameObject picked in ReticleMeshList)
        {
            picked.GetComponentInChildren<Animator>().SetTrigger(message);
        }
    }

    public void ActiveBodiesUpdater(GameObject disabledObject)
    {
        ActiveBodyBeingCleared = false;

        if (ActiveBodies.Contains(disabledObject))
        {
            ActiveBodies.Remove(disabledObject);
            ActiveBodyBeingCleared = true;

        if (ReticleActiveDictionary.ContainsKey(disabledObject))
            {
                //   damageSwapList currentDamageListItem = damageObjectLinks[picked];
                GameObject currentReticle = ReticleActiveDictionary[disabledObject];
                currentReticle.SetActive(false);
                if (ReticleMeshList.Contains(currentReticle))
                    {
                        ReticleMeshList.Remove(currentReticle);
                    }

                AnimateReticles(ReticleActivate);
            }
        }

        if (ActiveBodies.Count == 0)
        {
            AllActiveBodiesCleared = true;
        }
    }

    public void ResetBossPhase(bool state)
    {
        ActiveBodies.Clear();
        bossPhase = BossPhase.BossPhase1;
    }


    public void DoBossDefeated(bool state)
    {
        print("boss defeated in generic controller");
        gameObject.SetActive(state);
        BossDefeated = true;
        starGameManagerRef.BossDefeated = true;
        if (bossType == BossType.Basher)
        {
            starGameManagerRef.BasherDefeated();
        }
    }


   



    public void AlertedSubObjectDisabled(GameObject disabledSubObject)
    {
        DisabledSubObjects.Add(disabledSubObject);
    }

    public void PopulateDummies(GameObject[] array, GameObject sourceFbx, GameObject parent,
        bool useLocalPos, bool addToActiveBodies, Transform scaleSource, List<GameObject> addToList)
    {
        for (int i = 0; i < array.Length; i++)
        {
            GameObject currentSpawn = Instantiate(sourceFbx) as GameObject;

            currentSpawn.transform.parent = array[0].transform.parent.transform;
          if (useLocalPos)
            {
                currentSpawn.transform.localPosition = array[i].transform.localPosition;
            }
            else
            {
                currentSpawn.transform.position = array[i].transform.position;
            }

            currentSpawn.transform.localEulerAngles = array[i].transform.localEulerAngles;

            if (scaleSource != null)
            {
                currentSpawn.transform.localScale = scaleSource.localScale;
            }

            if (currentSpawn.GetComponentInChildren<starEnemy>())
            {
                currentSpawn.GetComponentInChildren<starEnemy>().parent = currentSpawn;
                currentSpawn.GetComponentInChildren<starEnemy>().AlertControllerOnDisable = gameObject;
            }

            if (addToActiveBodies)
            {
                ActiveBodies.Add(currentSpawn);
            }

            if (addToList != null)
            {
                addToList.Add(currentSpawn);
            }
        }
    }

    public void GenericProjectileFire(GameObject useProjectile, GameObject spawnPoint,
        ShotDirection shotDirection)
    {

        CurrentProjectile = null;
        CurrentProjectile = starGameManagerRef.SpawnedChecker(useProjectile);

        if (CurrentProjectile == null)
        {
            return;
        }

        CurrentProjectile.transform.SetParent(starGameManagerRef.ActorsParent.transform, false);

        // useObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, true);
        //  useObject.transform.localPosition = spawnPoint.transform.localPosition;
        CurrentProjectile.transform.position = spawnPoint.transform.position;

        CurrentProjectile.transform.rotation = spawnPoint.transform.rotation;


        if (CurrentProjectile.GetComponent<StarProjectileAnim>())
        {
            CurrentProjectile.GetComponent<StarProjectileAnim>().shotDirection = shotDirection;
        }
        CurrentProjectile.SetActive(true);

    }
    
}
