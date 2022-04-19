using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FollowObjects { SphereShield1, SphereSHield2 }
public enum HeroSpawnPoint { GeneralLevel, BasherChase, BasherEngage, FinalBoss }
public enum ControllerInteractionMode { Teleport, ObjectPlacementFloor, ObjectPlacementWall, MenuNav, NA }

public class StarGameManager : MonoBehaviour
{


    /// <summary>
    ///  VR JANK STUFF THAT NEEDS TO BE CLEANED UP
    /// </summary>

    public ControllerInteractionMode controllerInteractionMode =
        ControllerInteractionMode.Teleport;

    public float DistanceGrabThreshold = 4.0f;
    public GameObject CameraHolder;


    /// <summary>
    /// end of VR JANK STUFF THAT NEEDS TO BE CLEANED UP
    /// </summary>

    public static StarGameManager instance;

    void Awake()
    {
        instance = this;
        origTerminatorDistance = TerminatorDistance;
    }

    public enum GlobalActionButton { None, A, B, X, Y, LB, RB }
    public enum StartingLevel { Test, Title, Traditional, Showcase, FinalBoss, WallBoss, SurroundBoss, BasherBoss, Old }
    public enum GameplayMode { Showcase, Traditional, BossRush, Test }

    public GlobalActionButton globalActionButton;

    private string XbuttonString;
    private string YbuttonString;
    private string AbuttonString;
    private string BbuttonString;
    private string LBbuttonString;
    private string RBbuttonString;

    [HideInInspector] public string usingButtonString;
    public StartingLevel startingLevel;
    // [ShowOnly]  // this doesn't work unless i make a custom property drawer
    [HideInInspector]
    public GameplayMode gameplayMode;
    public bool ForceShowcaseFeatures;
    public bool ForceFinalBossPlacement;

    public int level, checkpoint;

    public bool CurrentlyHighScore;
    public int HighScore;
    public int currentScore, beginHealth, currentHealth, currentLives, beginLives;
    public int CurrentMultiplier = 1;
    public string currentWeapon;
    public float GameSpeed;
    private float origProjectileSpeed = 1;
    // [HideInInspector]
    public float ProjectileSpeed = 1;
    private float previousGameSpeed;
    private float previousProjectileSpeed;

    public GameObject TileMoverObject;
    public GameObject TileHolderObject;
    public GameObject DrawVolume;
    public GameObject DrawVolumeDebug;
    public bool UseDrawVolumeDebug;
    public GameObject weaponSelectIcon;

    public List<string> weaponInventoryList = new List<string>();

    public List<GameObject> leaderList = new List<GameObject>();

    public List<GameObject> EnemyPoolActive = new List<GameObject>();
    public List<GameObject> EnemyPoolInactive = new List<GameObject>();
    public List<GameObject> EnemyBulletPoolActive = new List<GameObject>();
    public List<GameObject> EnemyBulletPoolInactive = new List<GameObject>();
    public List<GameObject> weaponIcons = new List<GameObject>();
    public List<GameObject> weaponIconPrefabs = new List<GameObject>();
    public List<GameObject> HeroBulletPoolActive = new List<GameObject>();
    public List<GameObject> HeroBulletPoolInactive = new List<GameObject>();
    public List<GameObject> TilePoolActive = new List<GameObject>();
    public List<GameObject> TilePoolInactive = new List<GameObject>();
    public List<GameObject> SpecialPoolActive = new List<GameObject>();
    public List<GameObject> SpecialPoolInactive = new List<GameObject>();
    public List<GameObject> ShadowPoolActive = new List<GameObject>();
    public List<GameObject> ShadowPoolInactive = new List<GameObject>();



    /*
    [HideInInspector] public List<GameObject> explosionsPoolActive = new List<GameObject>();
    [HideInInspector] public List<GameObject> explosionsPoolInactive = new List<GameObject>();
    */

    public List<GameObject> explosionsPoolActive = new List<GameObject>();
    public List<GameObject> explosionsPoolInactive = new List<GameObject>();

    public List<GameObject> DistanceCheckResize = new List<GameObject>();
    public List<GameObject> TrailRendererResize = new List<GameObject>();



    public GameObject weaponIconHolder;
    Vector3 lastIconPosition;
    GameObject useObject;
    [HideInInspector] public bool PauseAvailable;
    public bool GamePaused;
    public bool EnemiesCanShoot;
    public bool invincibleHero;
    public bool WarpOnEveryRespawn;
    public bool DebugAutofire;
    public bool EnableTilemover = true;
    public bool EnableBossWarnings;
    [HideInInspector]
    public bool starmanMode;
    public GameObject ActorsParent;
    public UnityEvent PauseEvent;
    public UnityEvent ResumeEvent;
    [Header("Prefabs")]
    public GameObject HeroShipPrefab;
    public GameObject HeroShipExplosionPrefab;
    public GameObject HeroWarpInPrefab;
    public GameObject GenericShadowObject;
    public GameObject ExtraBodyBulletBlue;
    public GameObject ExtraBodyBulletRed;
    public GameObject ExtraBodyPrefab;
    [HideInInspector]
    public GameObject HeroShip;
    [HideInInspector]
    public GameObject ShipExplosion;
    [HideInInspector]
    public GameObject HeroShipWarp;
    private Collider heroShipCollider;
    [Space]
    private bool WarpEffectFinished;
    public float WarpEffectDelay = 3.5f;
    public float HeroRespawnTime = 1.7f;
    public float HeroShadowRespawnTime = 2.5f;
    public float StartMenuAppearDelay = 5.0f;
    bool startMakingTilesAtBegin = true;
    // public GameObject[] ExtraBody;
    public List<GameObject> ExtraBody = new List<GameObject>();
    [Range(0.0f, 1.0f)]
    public float AudioVolume = 0.5f;
    public bool AddPow;
    [Range(0, 5)]
    public int PowLevel;
    public Dictionary<string, int> EntityCounts;
    // private GameObject blueExtraBody;
    public GameObject blueExtraBody;
    starEnemy starEnemyComponentExtraBody1;
    starEnemy starEnemyComponentExtraBody2;
    bool extraBody1priorFiringState;
    bool extraBody2priorFiringState;
    bool canInhibitExtraBodyFiring = true;
    [HideInInspector]
    public List<GameObject> ExtraBodyProjectileSpawners = new List<GameObject>();
    public GameObject Caboose;
    public GameObject EnemySpawnManager;
    public StarShipController starShipController;
    public float BombInhibitSpawnsDuration = 1.0f;
    [HideInInspector]
    public bool InhibitEnemySpawns;
    public GameObject BombEffect;
    public float StarmanModeDuration = 3.0f;
    [HideInInspector]
    public Animator heroshipAnimator;
    public StarSpawnManager SpawnManager;
    public float TerminatorDistance = 0.52f;
    public GameObject StageSizeObject;
    // [HideInInspector]
    public float StageSize = 1;
    private float stageSizeInit;
    bool exceededObjectLimit;

    public TapToPlaceParent FinalBossPlacementObject;
    public LocalSceneManagement localSceneManagement;
    private float origTerminatorDistance;
    private bool scalingInProgress;
    public GameObject GestureManager;
    public StarUiManager UiManager;
    [HideInInspector]
    public UiCommunicate uiCommunicate;
    public bool BossDefeated;
    [HideInInspector]
    public GameObject ShadowYObject;
    public float ShadowHeight = 0.04f;
    public AudioClip PowerupClip;
    public AudioClip PowerupPointClip;
    public AudioClip PlayerExplodeClip;
    public AudioClip PlayerRespawnClip;
    public AudioClip PlayerStarmanClip;
    public AudioClip PlayerPowLevelLowerClip;
    public AudioClip MegaBombClip;
    [Header("Speech Clips")]
    public AudioClip SpeechInvincibleClip;
    public AudioClip SpeechSatelliteClip;
    public AudioClip SpeechSatelliteDestroyedClip;
    public AudioClip SpeechSatelliteSpeedUpClip;
    public AudioClip SpeechSatelliteArmedClip;
    public AudioClip SpeechMegaBombClip;


    private AudioSource playerAudioSource;  // find a better way to define this
    public float RespawnAudioDelay = 2.0f;

    [HideInInspector]
    public bool CameraClippingPlaneNear = false;
    [HideInInspector]
    public string CameraNearTrigger = "clipNear";
    [HideInInspector]
    public string CameraFarTrigger = "clipFar";
    public GameObject MainCameraObject;
    public Material DamageFlashMaterial;
    [HideInInspector]
    public GameObject localSceneManager;
    [HideInInspector]
    public LocalSceneManagement localSceneManagementRef;
    public GameObject[] DestroyOnRestart;
    public float LoadFinalStageDelay = 3.35f;

    public List<GameObject> DestroyItems = new List<GameObject>();

    public List<Rigidbody> SleepPhysicsOnPause = new List<Rigidbody>();
    public List<Animator> PauseAnimList = new List<Animator>();
   // public Dictionary<Animator, float> AnimatorPrevMultipliers = new Dictionary<Animator, float>();


    private bool voicesEnabled = true;
    private bool playedSatelliteAudioClip;
    private bool playedSatelliteDestroyedAudioClip;
    private bool playedSatelliteArmedAudioClip;
    private bool playedSatelliteSpeedUpAudioClip;
    private bool playedInvincibleAudioClip;
    private bool playedMegaBombAudioClip;

    [HideInInspector]
    public bool BossWarningMessagePlaying = false;

    public GameObject HeroSpawnLevelPrefab;
    public GameObject HeroSpawnFinalLevelPrefab;
    public GameObject HeroSpawnBasherChasePrefab;
    public GameObject HeroSpawnBasherEngagePrefab;

    [HideInInspector]
    public GameObject HeroSpawnLevel;
    [HideInInspector]
    public GameObject HeroSpawnFinalLevel;
    [HideInInspector]
    public GameObject HeroSpawnBasherChase;
    [HideInInspector]
    public GameObject HeroSpawnBasherEngage;
  // [HideInInspector]
    public GameObject currentHeroSpawnPoint;
    public Vector3 WarpEffectRotOffset;

    void DestroyItemLists(List<GameObject> useList)
    {


        if (useList.Count == 0)
        {
            print("list empty");
            return;
        }

        print("adding to list");

        foreach (GameObject picked in useList)
        {
            DestroyItems.Add(picked);
        }

        useList.Clear();
    }


    void DestroyPoolContents()
    {

        DestroyItemLists(EnemyBulletPoolInactive);
        DestroyItemLists(HeroBulletPoolInactive);
        DestroyItemLists(SpecialPoolInactive);
        DestroyItemLists(explosionsPoolInactive);
        DestroyItemLists(TilePoolInactive);
        DestroyItemLists(ShadowPoolInactive);
        DestroyItemLists(EnemyPoolInactive);

        DestroyItemLists(EnemyBulletPoolActive);
        DestroyItemLists(EnemyPoolActive);
        DestroyItemLists(explosionsPoolActive);
        DestroyItemLists(HeroBulletPoolActive);
        DestroyItemLists(ShadowPoolActive);
        DestroyItemLists(SpecialPoolActive);
        DestroyItemLists(TilePoolActive);

        foreach (GameObject picked in DestroyItems)
        {
            Destroy(picked);
        }
    }


    public void BasherDefeated()
    {
        StartCoroutine(LoadFinalLevelTimer());
        // TileMoverObject.SetActive(false);
        // TileMoverObject.GetComponent<TileMover>().enabled = false;
    }

    IEnumerator LoadFinalLevelTimer()
    {
        yield return new WaitForSeconds(LoadFinalStageDelay);
        // startingLevel = StarGameManager.StartingLevel.FinalBoss;
        // uiCommunicate.LoadLevelFinalBoss();
        // -----------------------------------


        // DestroyPoolContents();  // doesn't appear to help
        // UiManager.SceneSwitch(StarUiManager.LevelSelection.FinalBoss);

        //  startingLevel = StarGameManager.StartingLevel.FinalBoss;
        // RestartGame();

        //   LoadLevel(StarUiManager.LevelSelection.Opening);
        //   LoadLevel(StarUiManager.LevelSelection.FinalBoss);
        //   UiManager.SceneSwitch(StarUiManager.LevelSelection.Opening);  // just keeps looping loading whatever was saved as the Load Level
        //

        UiManager.SceneSwitch(StarUiManager.LevelSelection.FinalBoss);  // was using this but keeps giving supply ship and track piece error after scene is placed
    }

    public void LocalSceneManagementChecker()
    {
        if (localSceneManager == null)
        {
            localSceneManager = GameObject.Find("LocalSceneManagement");
            if (localSceneManager != null)
            {
                localSceneManagementRef = localSceneManager.GetComponent<LocalSceneManagement>();
            }

        }
    }

    public void ToggleCameraClipPlane()
    {
        string newCameraClipSetting = CameraNearTrigger;

        if (CameraClippingPlaneNear)
        {
            newCameraClipSetting = CameraFarTrigger;
        }

        MainCameraObject.GetComponent<Animator>().SetTrigger(newCameraClipSetting);
        CameraClippingPlaneNear = !CameraClippingPlaneNear;
    }

    public void ToggleHeroInvincibilityDebug()
    {
        invincibleHero = !invincibleHero;
    }


    void ChangeVolume(float newVolume)
    {

        AudioListener.volume = newVolume;
    }

    public void BeginGameplay(HeroSpawnPoint heroSpawnPoint = HeroSpawnPoint.GeneralLevel)
    {

        GenerateHeroSpawnPoints(heroSpawnPoint);

        print("beginning gameplay in SGM");
        MakePauseAvailable(true);
        starShipController.canReceiveInput = true;

        TileMover tileMover = TileMoverObject.GetComponent<TileMover>();

        if (startMakingTilesAtBegin && tileMover.TileGroups.Count != 0)
        {
            TileMoverObject.GetComponent<TileMover>().StartCreatingTiles(true);
        }
    }


    public void BombEnemies()
    {
        StartCoroutine(InhibitSpawningEnemiesTimer());
        StartCoroutine(BombEffectTimer());


        foreach (GameObject picked in EnemyPoolActive)
        {
            if (picked.GetComponentInChildren<EnemyHealth>())
            {
                picked.GetComponentInChildren<EnemyHealth>().health = 0;
            }
        }

        List<GameObject> tempBulletList = new List<GameObject>();

        foreach (GameObject picked in EnemyBulletPoolActive)
        {
            tempBulletList.Add(picked);
        }

        foreach (GameObject picked in tempBulletList)
        {
            picked.SetActive(false);
        }


    }


    private IEnumerator BombEffectTimer()
    {
        BombEffect.transform.position = HeroShip.transform.position;
        BombEffect.SetActive(true);
        PlayAudio(playerAudioSource, MegaBombClip);
        PlayPowSpeechClip(playedMegaBombAudioClip, SpeechMegaBombClip);
        playedMegaBombAudioClip = true;

        yield return new WaitForSeconds(1.0f);

        while (GamePaused)
        {
            yield return null;
        }

        BombEffect.SetActive(false);

    }


    private IEnumerator InhibitSpawningEnemiesTimer()
    {
        InhibitEnemySpawns = true;

        yield return new WaitForSeconds(BombInhibitSpawnsDuration);

        while (GamePaused)
        {
            yield return null;
        }

        InhibitEnemySpawns = false;
    }



    public void EntityCounters(GameObject spawnObject)

    {

        int currentEntityLimit = spawnObject.GetComponent<PoolListMembership>().ItemLimit;
        int currentEntityIteration;
        string currentSpawnName = spawnObject.name;

        if (!EntityCounts.ContainsKey(currentSpawnName))
        {
            EntityCounts.Add(currentSpawnName, 1);
        }

        else

        {
            currentEntityIteration = EntityCounts[currentSpawnName];

            if (currentEntityIteration + 1 > currentEntityLimit)
            {
                //  return null;
                // tell it somehow to do nothing
                exceededObjectLimit = true;
                return;
            }

            EntityCounts[currentSpawnName] = currentEntityIteration + 1;
        }
    }

    public Vector3 AdjustShotDirection(ShotDirection shotdirection)
    {
        Vector3 newRotationCoords = new Vector3(0, 0, 0);

        switch (shotdirection)
        {
            case ShotDirection.NA:
                break;
            case ShotDirection.Forward:
                newRotationCoords = new Vector3(-90, 0, 0);
                break;
            case ShotDirection.Backward:
                newRotationCoords = new Vector3(90, 0, 0);
                break;
            case ShotDirection.Left:
                newRotationCoords = new Vector3(0, 90, 0);
                break;
            case ShotDirection.Right:
                newRotationCoords = new Vector3(0, -90, 0);
                break;
            case ShotDirection.Above:
                break;
            case ShotDirection.Below:
                newRotationCoords = new Vector3(0, 0, 0);
                break;
            default:
                break;
        }

        return newRotationCoords;
    }


    public GameObject SpawnedChecker(GameObject spawnObject)

    {
        // When instantiating pooled objects, they MUST be created from SpawnedChecker to track their EntityCount limits 
        // (how many bullets can be onscreen, etc). Otherwise when they are disabled, they won't be able to refer back to an
        // existing EntityCount dictionary entry as none will have ever been created. -- This is now fixed as otherwise-instantiated
        // objects will reference EntityCounters() when they are enabled if necessary

        // Pooled objects should run a check to ignore EntityCount dictionary if there is none in there previously,
        // in case they are simply turned on during testing

        EntityCounters(spawnObject);

        if (exceededObjectLimit)
        {
            exceededObjectLimit = false;
            return null;
        }

        List<GameObject> activeList;
        List<GameObject> inactiveList;
        PoolListMembership.UsingList usingList = spawnObject.GetComponent<PoolListMembership>().usingList;

        switch (usingList)

        {
            case PoolListMembership.UsingList.ExplosionsEnemy:
                activeList = explosionsPoolActive;
                inactiveList = explosionsPoolInactive;
                break;
            case PoolListMembership.UsingList.BulletsEnemy:
                activeList = EnemyBulletPoolActive;
                inactiveList = EnemyBulletPoolInactive;
                break;
            case PoolListMembership.UsingList.BulletsHero:
                activeList = HeroBulletPoolActive;
                inactiveList = HeroBulletPoolInactive;
                break;
            case PoolListMembership.UsingList.EnemyA:
                activeList = EnemyPoolActive;
                inactiveList = EnemyPoolInactive;
                break;
            case PoolListMembership.UsingList.EnemyB:
                activeList = EnemyPoolActive;
                inactiveList = EnemyPoolInactive;
                break;
            case PoolListMembership.UsingList.EnemyC:
                activeList = EnemyPoolActive;
                inactiveList = EnemyPoolInactive;
                break;
            case PoolListMembership.UsingList.Tile:
                activeList = TilePoolActive;
                inactiveList = TilePoolInactive;
                break;
            case PoolListMembership.UsingList.Special:
                activeList = SpecialPoolActive;
                inactiveList = SpecialPoolInactive;
                break;
            case PoolListMembership.UsingList.Shadow:
                activeList = ShadowPoolActive;
                inactiveList = ShadowPoolInactive;
                break;

            default:
                activeList = EnemyPoolActive;
                inactiveList = EnemyPoolInactive;
                break;
        }


        GameObject useObject = null;

        if (inactiveList.Count > 0)
        {

            for (int i = 0; i < inactiveList.Count; i++)

            {
                if (inactiveList[i] != null)
                {
                    if (spawnObject.name == inactiveList[i].name)

                    {
                        useObject = inactiveList[i];
                        break;
                    }
                }

            }
        } // END CHECKING POOL

        if (useObject == null)
        {
            useObject = Instantiate(spawnObject) as GameObject;

            // useObject = Instantiate(spawnObject, transform.position, Quaternion.identity) as GameObject;

            //   useObject = Instantiate(spawnObject, transform.position, transform.rotation * Quaternion.Euler(0, 0, 0)) as GameObject;
            //  useObject = Instantiate(spawnObject, transform.position, transform.rotation) as GameObject;


            //  transform.rotation* Quaternion.Euler(0, 0, 0)

            useObject.name = spawnObject.name;
        }


        if (!activeList.Contains(useObject))
        {
            activeList.Add(useObject);
            //  print("added to activeList - " + useObject.name);
        }

        if (inactiveList.Contains(useObject))
        {
            inactiveList.Remove(useObject);
            //   print("REMOVED from inactiveList - " + useObject.name);

        }


        return useObject;
    }



    /*
    public bool weapon01_normal_active,
        weapon02_shotgun_active,
        weapon03_swirl_active,
        weapon04_box_active,
        weapon05_spread_active,
        weapon06_side_active,
        weapon07_vertical_active,
        weapon08_ball_active,
        weapon09_drill_active,
        weapon10_scatter_active,
        weapon11_lockon_active,
        weapon12_charge_active,
        weapon13_heatseeker_active;
        */

    public void QuitGame()
    {
        Application.Quit();
    }

    public void WeaponIncrement(bool direction)
    {

        for (int i = 0; i < weaponInventoryList.Count; i++)
        {

            if (weaponInventoryList[i] == currentWeapon)
            {

                if (direction == true && (i + 1 < weaponInventoryList.Count))
                {
                    currentWeapon = weaponInventoryList[i + 1];
                    weaponSelectIcon.transform.position = weaponIcons[i + 1].transform.position;

                }


                if (direction == false && (i - 1 > -1))
                {
                    currentWeapon = weaponInventoryList[i - 1];
                    weaponSelectIcon.transform.position = weaponIcons[i - 1].transform.position;

                }
                break;
            }

        }

    }


    public void SetNewWeaponInGame(string addingWeapon)

    {

        //   print("addingWeapon = "+addingWeapon);

        AddActiveWeapon(addingWeapon);


        switch (addingWeapon)
        {
            case "01_normal":
                MakeNewIcon(0);
                break;

            case "02_shotgun":
                MakeNewIcon(1);
                break;

            case "03_swirl":
                MakeNewIcon(2);
                break;

            case "04_box":
                MakeNewIcon(3);
                break;

            case "05_spread":
                MakeNewIcon(4);
                break;

            case "06_side":
                MakeNewIcon(5);
                break;

            case "07_vertical":
                MakeNewIcon(6);
                break;

            case "08_ball":
                MakeNewIcon(7);
                break;

            case "09_drill":
                MakeNewIcon(8);
                break;

            case "10_scatter":
                MakeNewIcon(9);
                break;

            case "11_lockon":
                MakeNewIcon(10);
                break;

            case "12_charge":
                MakeNewIcon(11);
                break;

            case "13_heatseeker":
                MakeNewIcon(12);
                break;

        }


    }





    //   void MakeNewIcon(int listNumber, string addingWeapon)
    void MakeNewIcon(int listNumber)

    {

        lastIconPosition = lastIconPosition + new Vector3(0, 0.12f, 0);

        useObject = Instantiate(weaponIconPrefabs[listNumber], lastIconPosition, Quaternion.identity) as GameObject;
        useObject.transform.SetParent(weaponIconHolder.transform, false);
        weaponIcons.Add(useObject);
    }







    public void AddActiveWeapon(string addWeapon)

    {

        if (!weaponInventoryList.Contains(addWeapon))
        {
            weaponInventoryList.Add(addWeapon);
        }


    }

    /*
    void WeaponBoolCheck()

    {
        if (weapon01_normal_active) SetNewWeaponInGame("01_normal");
        if (weapon02_shotgun_active) SetNewWeaponInGame("02_shotgun");
        if (weapon03_swirl_active) SetNewWeaponInGame("03_swirl");
        if (weapon04_box_active) SetNewWeaponInGame("04_box");
        if (weapon05_spread_active) SetNewWeaponInGame("05_spread");
        if (weapon06_side_active) SetNewWeaponInGame("06_side");
        if (weapon07_vertical_active) SetNewWeaponInGame("07_vertical");
        if (weapon08_ball_active) SetNewWeaponInGame("08_ball");
        if (weapon09_drill_active) SetNewWeaponInGame("09_drill");
        if (weapon10_scatter_active) SetNewWeaponInGame("10_scatter");
        if (weapon11_lockon_active) SetNewWeaponInGame("11_lockon");
        if (weapon12_charge_active) SetNewWeaponInGame("12_charge");
        if (weapon13_heatseeker_active) SetNewWeaponInGame("13_heatseeker");
    }
    */

    public void SetHeroInvincibility(bool state)  // probably delete this!!!
    {
        invincibleHero = state;
    }

    public void SetHeroStarmanMode()
    {
        StartCoroutine(StarmanModeTimer());
        PlayAudio(playerAudioSource, PlayerStarmanClip);
        PlayPowSpeechClip(playedInvincibleAudioClip, SpeechInvincibleClip);
        playedInvincibleAudioClip = true;
    }

    public void PlayAudio(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    public void ReassignItemsOnSceneLoad()
    {

        // do at start as well -- PERHAPS

        if (ExtraBody.Count != 0)
        {
            if (ExtraBody[1] != null)
            {
                blueExtraBody = ExtraBody[1];
            }

            if (ExtraBody[0] != null)
            {
                if (ExtraBody[0].GetComponentInChildren<starEnemy>())
                {
                    starEnemyComponentExtraBody1 = ExtraBody[0].GetComponentInChildren<starEnemy>();
                }
            }

            if (ExtraBody[1] != null)
            {
                if (ExtraBody[1].GetComponentInChildren<starEnemy>())
                {
                    starEnemyComponentExtraBody2 = ExtraBody[1].GetComponentInChildren<starEnemy>();
                }
            }
        }
    }

    public void ExtraBodiesFireProjectile()
    {
        ExtraBodyComponentCheck();
        starEnemyComponentExtraBody1.fireProjectile(ActorsParent);
        starEnemyComponentExtraBody2.fireProjectile(ActorsParent);
    }

    void Start()

    {
        if (StageSizeObject != null)
        {
            stageSizeInit = StageSizeObject.transform.localScale.x;
        }

        if (UseDrawVolumeDebug && DrawVolumeDebug != null)
        {
            DrawVolume = DrawVolumeDebug;
        }

        if (!EnableTilemover && TileMoverObject != null)
        {
            TileMoverObject.GetComponent<TileMover>().enabled = false;
        }

        ChangeVolume(AudioVolume);

        EntityCounts = new Dictionary<string, int>();


        //    weapon01_normal_active = true; // THIS WORKS FINE, IF NEEDED
        //  WeaponBoolCheck();

        if (weaponSelectIcon != null)
        {
            weaponSelectIcon.transform.position = weaponIcons[0].transform.position;
        }

        /*
        if (HeroShip != null)
        {
            heroshipAnimator = HeroShip.GetComponent<Animator>();
            playerAudioSource = HeroShip.GetComponent<AudioSource>();
        }
        */

        currentLives = beginLives;
        currentHealth = beginHealth;

        if (weaponInventoryList.Count > 0)
            currentWeapon = weaponInventoryList[0];

        XbuttonString = "360_X";
        YbuttonString = "360_Y";
        AbuttonString = "360_A";
        BbuttonString = "360_B";
        LBbuttonString = "360_LB";
        RBbuttonString = "360_RB";

        switch (globalActionButton)
        {
            case GlobalActionButton.None:
                break;

            case GlobalActionButton.A:
                usingButtonString = AbuttonString;
                break;

            case GlobalActionButton.B:
                usingButtonString = BbuttonString;
                break;

            case GlobalActionButton.X:
                usingButtonString = XbuttonString;
                break;

            case GlobalActionButton.Y:
                usingButtonString = YbuttonString;
                break;

            case GlobalActionButton.LB:
                usingButtonString = LBbuttonString;
                break;

            case GlobalActionButton.RB:
                usingButtonString = RBbuttonString;
                break;
        }

        StartingLevelCheck();
    }

    void StartingLevelCheck()
    {


        if (startingLevel == StartingLevel.Title)
        {
            StartCoroutine(RunTitleSequence());
            return;
        }

        switch (startingLevel)
        {
            case StartingLevel.Test:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.Test);
                break;
            case StartingLevel.Title:
                break;
            case StartingLevel.Traditional:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.Traditional);
                break;
            case StartingLevel.Showcase:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.Showcase);
                break;
            case StartingLevel.FinalBoss:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.FinalBoss);
                //  LoadFinalBossUi();
                break;
            case StartingLevel.BasherBoss:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.BasherBoss);
                break;
            case StartingLevel.SurroundBoss:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.SurroundBoss);
                break;
            case StartingLevel.WallBoss:
                UiManager.SceneSwitch(StarUiManager.LevelSelection.WallBoss);
                break;
            case StartingLevel.Old:
                startMakingTilesAtBegin = false;
                break;
            default:
                break;
        }
    }

    public void LoadFinalBossUi()
    {
        StartCoroutine(LoadFinalBossUiTimer());
    }

    IEnumerator LoadFinalBossUiTimer()
    {
        yield return new WaitForSeconds(1.0f);
        UiManager.OpenFinalBossPlaceMenu();
        ResetManagerForBossSetting(); // should find a better place to put this
    }

    void ResetManagerForBossSetting()
    {
        print("resetting for boss level");
        WarpOnEveryRespawn = true;
        PowLevel = 0;
        noObjectRefsMissing = false;
    }

    public void ActivateBossLevelGameplay(bool firstLaunch = true)
    {
        if (firstLaunch)
        {
            localSceneManagement.ActivateTheElements();  // don't do this if resuming
            CreateNewHeroShip();
        }
        else
        {
            SetHeroHidden();
            StartCoroutine(WarpInHeroShipTimer());
        }

        localSceneManagement.BossPlacementObject.SetActive(false);  // hopefully not troublesome  //rma2019_12_09
     
        MakePauseAvailable(true);
    }

    public void RestartGame()
    {
        GameObject destroyParentObject = new GameObject();
        foreach (GameObject picked in DestroyOnRestart)
        {
            picked.transform.parent = destroyParentObject.transform;
        }
    }

    
    IEnumerator RunTitleSequence()
    {
        UiManager.OpenTitlePage();
        yield return new WaitForSeconds(StartMenuAppearDelay);
        UiManager.OpenStartMenu();
    }


    public void ObjectIsEnabled(GameObject thisObject, bool state)
    {
        thisObject.SetActive(state);
    }

    public void MakePauseAvailable(bool state)
    {
        PauseAvailable = state;
    }

    public void TogglePause()
    {


        if (!PauseAvailable)

        {
            return;
        }

        if (!GamePaused)
        {
            previousGameSpeed = GameSpeed;
            previousProjectileSpeed = ProjectileSpeed;
        }

        GamePaused = !GamePaused;

        if (GamePaused)
        {
            GameSpeed = 0;
            ProjectileSpeed = 0;
            PauseEvent.Invoke();
            PausePhysics(true);
            ObjectIsEnabled(UiManager.UIScore, true);
        }
        else
        {
            PausePhysics(false);
            GameSpeed = previousGameSpeed;
            ProjectileSpeed = previousProjectileSpeed;
            ObjectIsEnabled(UiManager.UIScore, false);

            ResumeEvent.Invoke();  // this needs to be removed

            if (UiManager.PerimeterBoxOutlineAnim != null)
            {
                if (UiManager.PerimeterBoxDim != null)
                {
                    UiManager.PerimeterBoxOutlineAnim.gameObject.SetActive(false);
                }
                if (UiManager != null)
                {
                    UiManager.Unpaused();
                }
            }
        }

        ChangeAllAnimSpeeds(GameSpeed);


    } // END PAUSE

    

    void ChangeAllAnimSpeeds(float currentSpeed)
    {

        for (int i = 0; i < PauseAnimList.Count; i++)
        {
            float newSpeed = 0;

            if (currentSpeed != 0)
            {
                newSpeed = 1;
            }

            PauseAnimList[i].speed = newSpeed;
        }
    }

    void PausePhysics(bool state)
    {
        if (SleepPhysicsOnPause.Count != 0)
        {
            foreach (Rigidbody picked in SleepPhysicsOnPause)
            {
                picked.isKinematic = state;
            }
        }
    }


    void SetAnimBool(GameObject picked, string trigger, bool value)
    {
        picked.GetComponent<Animator>().SetBool(trigger, value);
    }

    void SetAnimTrigger(GameObject picked, string trigger)  // do I need this?
    {
        picked.GetComponent<Animator>().SetTrigger(trigger);
    }

    void BlueExtraBodyCheck(GameObject checkBody)
    {
        if (blueExtraBody == checkBody)
        {
            SetAnimBool(checkBody, "blue", true);
        }
    }


    void ExtraBodyComponentCheck()
    {

        if (starEnemyComponentExtraBody1 == null)
        {
            starEnemyComponentExtraBody1 = ExtraBody[0].GetComponentInChildren<starEnemy>();
            starEnemyComponentExtraBody1.projectile = ExtraBodyBulletRed;
        }

        if (starEnemyComponentExtraBody2 == null)
        {
            starEnemyComponentExtraBody2 = ExtraBody[1].GetComponentInChildren<starEnemy>();
            starEnemyComponentExtraBody2.projectile = ExtraBodyBulletBlue;
        }
    }


    void ExtraBodyTempInhibitFiring(bool state)
    {
        ExtraBodyComponentCheck();

        if (state == false && canInhibitExtraBodyFiring == true)

        {
            extraBody1priorFiringState = starEnemyComponentExtraBody1.ProjectileFiringEnabled;
            extraBody2priorFiringState = starEnemyComponentExtraBody2.ProjectileFiringEnabled;

            starEnemyComponentExtraBody1.ProjectileFiringEnabled = false;
            starEnemyComponentExtraBody2.ProjectileFiringEnabled = false;

            canInhibitExtraBodyFiring = false;
        }
        else
        {
            starEnemyComponentExtraBody1.ProjectileFiringEnabled = extraBody1priorFiringState;
            starEnemyComponentExtraBody2.ProjectileFiringEnabled = extraBody2priorFiringState;

            canInhibitExtraBodyFiring = true;
        }
    }

    void PowCheck()
    {
        /*
        if (ExtraBody.Count == 0)
        {
            return;
        }
        */
        if (ExtraBody[1] != null && ExtraBody[0] != null)
        {
            switch (PowLevel)
            {
                case 0:
                    foreach (GameObject picked in ExtraBody)
                    {
                        picked.SetActive(false);
                        picked.GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = false;
                    }
                    break;

                case 1:
                    ExtraBody[0].SetActive(true);
                    BlueExtraBodyCheck(ExtraBody[0]);
                    PlayPowSpeechClip(playedSatelliteAudioClip, SpeechSatelliteClip);
                    playedSatelliteAudioClip = true;

                    break;

                case 2:
                    ExtraBody[0].GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = true;
                    PlayPowSpeechClip(playedSatelliteArmedAudioClip, SpeechSatelliteArmedClip);
                    playedSatelliteArmedAudioClip = true;

                    break;

                case 3:
                    ExtraBody[1].SetActive(true);
                    BlueExtraBodyCheck(ExtraBody[1]);
                    break;

                case 4:
                    ExtraBody[1].GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = true;
                    break;

                case 5:


                    foreach (GameObject picked in ExtraBody)
                    {
                        SetAnimBool(picked, "fast", true);
                    }

                    PlayPowSpeechClip(playedSatelliteSpeedUpAudioClip, SpeechSatelliteSpeedUpClip);
                    playedSatelliteSpeedUpAudioClip = true;

                    break;
            }
        }
    }


    void PlayPowSpeechClip(bool checkPlayedClip, AudioClip audioClip)
    {
        if (!checkPlayedClip && voicesEnabled)
        {
            playerAudioSource.PlayOneShot(audioClip);
        }
    }


    public void ExtraBodyDied() // TRASH? //rma1205
    {
        foreach (GameObject picked in ExtraBody)
        {
            SetAnimBool(picked, "fast", false);
        }

        if (PowLevel > 2)
        {
            PowLevel = 2;

            GameObject activeEB = null;
            GameObject deadEB = null;

            foreach (GameObject picked in ExtraBody)
            {
                if (picked.activeSelf)
                {
                    activeEB = picked;
                }
                else
                {
                    deadEB = picked;
                    picked.GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = false;
                }

                //ExtraBody = new GameObject[] {activeEB, deadEB};  // not sure if I can just disable this // rma1205
            }
        }

        else

        {
            PowLevel = 0;
            foreach (GameObject picked in ExtraBody)
            {
                picked.GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = false;
            }
        }
    }


    public void IncreasePowLevel()
    {
        if (PowLevel < 5)
        {
            PowLevel++;
        }

        heroshipAnimator.SetTrigger("upgradeBlink");

        PowCheck();

        PlayAudio(playerAudioSource, PowerupClip);
    }


    private IEnumerator StarmanModeTimer()
    {
        starmanMode = true;
        heroshipAnimator.SetTrigger("starmanFlash");
        yield return new WaitForSeconds(StarmanModeDuration);

        while (GamePaused)
        {
            yield return null;
        }

        starmanMode = false;
        heroshipAnimator.SetTrigger("starmanOver");

    }


    public void AdjustTerminatorDistance()
    {
        if (StageSizeObject != null)
        {
            StageSize = StageSizeObject.transform.localScale.x;
            TerminatorDistance = origTerminatorDistance * StageSize;
            ProjectileSpeed = origProjectileSpeed * StageSize;

            // for popupCannons etc
            foreach (GameObject picked in DistanceCheckResize)
            {
                picked.GetComponent<DistanceCheckAnimTrigger>().AdjustDistanceCheckForResize();
            }

            foreach (GameObject picked in TrailRendererResize)
            {
                picked.GetComponent<TrailRenderMultiplier>().AdjustTrailRenderMultiplier();
            }


        }
    }


    public void DrawVolumeScale(Vector3 newScale)
    {
        DrawVolume.transform.localScale = newScale;
    }




    [HideInInspector]
    public bool noObjectRefsMissing;  // reset this to false on scene reload

    /// <summary>
    ///  replaces object refs that got destroyed in level swap 
    /// </summary>
    /// 



    IEnumerator WarpInHeroShipTimer()
    {
        WarpEffectFinished = false;

        if (HeroShipWarp == null)
        {
            HeroShipWarp = InstantiatePlayerElement(HeroWarpInPrefab, localSceneManagement.ShipSpawnPoint, true);
        }
        else
        {
            HeroShipWarp.SetActive(true);
            // might be necessary to replace respawn point for final level. Find a relative zone that the 
            // player ship is in or something, or even from last position
        }
        HeroShipWarp.transform.localEulerAngles = WarpEffectRotOffset + ActorsParent.transform.localEulerAngles;


        yield return new WaitForSeconds(WarpEffectDelay);

        while (GamePaused)
        {
            yield return null;
        }

        HeroShipWarp.SetActive(false);
        WarpEffectFinished = true;

        if (HeroShip == null)
        {
            CreateNewHeroShip();
        }
        else
        {
            HeroShip.SetActive(true);
        }

        starShipController.returnShipToPlayfield = true;
    }

    public void SwitchHeroSpawnPoint(GameObject newHeroSpawnPoint)
    {
        currentHeroSpawnPoint = newHeroSpawnPoint;
        starShipController.usingBasePos = currentHeroSpawnPoint.transform.localPosition;
    }

    void CreateNewHeroShip()
    {

        StartCoroutine(WarpInHeroShipTimer());
        HeroShip = InstantiatePlayerElement(HeroShipPrefab, localSceneManagement.ShipSpawnPoint, WarpEffectFinished);
        heroshipAnimator = HeroShip.GetComponent<Animator>();

        if (HeroShip != null)
        {
            playerAudioSource = HeroShip.GetComponent<AudioSource>();
            heroShipCollider = HeroShip.GetComponent<Collider>();
        }

        // make extrabodies
        ExtraBody.Clear();

        for (int i = 0; i < 2; i++)
        {
            GameObject newExtraBody = InstantiatePlayerElement(ExtraBodyPrefab, HeroShip, false);
            newExtraBody.transform.parent = HeroShip.transform;
            ExtraBody.Add(newExtraBody);
        }

        blueExtraBody = ExtraBody[1];
    }


    void ReplaceMissingObjects()
    {
        bool objectsVisibleAtStart = localSceneManagement.BeginGameplayAtSceneLoad;
        LocalSceneManagementChecker();

        if (ActorsParent == null)
        {
            ActorsParent = localSceneManagementRef.ActorsParent;
        }
        else
        {
            noObjectRefsMissing = true;
        }

        if (objectsVisibleAtStart)
        {
            if (HeroShip == null)
            { 
                CreateNewHeroShip();
            }
            else
            {
                noObjectRefsMissing = true;
            }
        }

        if (ShadowYObject == null)
        {
            ShadowYObject = InstantiatePlayerElement(GenericShadowObject, HeroShip, objectsVisibleAtStart);
            
            if (ShadowYObject == null)

            {
                noObjectRefsMissing = false;
            }
            else
            {
                ShadowYObject.transform.localScale = new Vector3(1,1,1);
                ShadowYObject.transform.localEulerAngles = new Vector3(0, 0, -90);
                ShadowYObject.transform.localPosition = new Vector3(ShadowYObject.transform.localPosition.x, ShadowYObject.transform.localPosition.y, ShadowHeight);

                GenericShadow genericShadow = ShadowYObject.GetComponent<GenericShadow>();
                genericShadow.followObjectXZ = HeroShip;
                genericShadow.yChangeUse = GenericShadow.YChangeUse.Z;
                genericShadow.KeepLinksOnDisable = true;

            }
            
        }
        else
        {
            noObjectRefsMissing = true;
        }
    }

    private GameObject InstantiatePlayerElement(GameObject instantiatedPrefab, GameObject targetPositionObject, bool activate)
    {

        if (targetPositionObject == null)
        {
            return null;
        }

        GameObject currentObject = Instantiate(instantiatedPrefab) as GameObject;
        if (localSceneManagement.ShipSpawnPoint != null)
        {
            currentObject.transform.parent = targetPositionObject.transform.parent;
            currentObject.transform.localPosition = targetPositionObject.transform.localPosition;
            currentObject.transform.localScale = targetPositionObject.transform.localScale;
            if (activate)
            {
                currentObject.SetActive(true);
            }
        }

        return currentObject;
    }

        
    void SetHeroHidden()
    {
        starShipController.ShipCanReceiveInput(false);
        ShadowYObject.SetActive(false);
        heroshipAnimator.SetTrigger("hideShip");
        heroShipCollider.enabled = false;
    }

    IEnumerator HeroDeath()
    {

        SetHeroHidden();

        if (ShipExplosion == null)
        {
            ShipExplosion = InstantiatePlayerElement(HeroShipExplosionPrefab, HeroShip, true);  // probabky can roll this into hero ship creation, along w ExtraBody - just keep it false
        }
        else
        {
            ShipExplosion.transform.localPosition = HeroShip.transform.localPosition;
            ShipExplosion.SetActive(true);
        }

        yield return new WaitForSeconds(HeroRespawnTime);

        while (GamePaused)
        {
            yield return null;
        }

        if (WarpOnEveryRespawn)
        {
           // print("warp from heroDeath");  // not calling here  //rma200105
            StartCoroutine(WarpInHeroShipTimer());
            starShipController.ResetShipPosition(false);
        }

        else
        {
            starShipController.ResetShipPosition(true);
        }

        if (LastHeroDeathCoroutine == null)
        {
            LastHeroDeathCoroutine = HeroDeathAnnounce();
            StartCoroutine(LastHeroDeathCoroutine);
        }

            }
    
    IEnumerator HeroDeathAnnounce()
    {
        HeroKilled = !HeroKilled;
        yield return new WaitForSeconds(0.1f);
        while (GamePaused)
        {
            yield return null;
        }
        HeroKilled = !HeroKilled;
        LastHeroDeathCoroutine = null;
    }


    IEnumerator LastHeroDeathCoroutine;
    [HideInInspector]
    public bool HeroKilled;

     void GenerateHeroSpawnPoints(HeroSpawnPoint heroSpawnPoint)
    {
        HeroSpawnLevel = SpawnLevelPoint(HeroSpawnLevelPrefab);
        HeroSpawnBasherChase = SpawnLevelPoint(HeroSpawnBasherChasePrefab);
        HeroSpawnBasherEngage = SpawnLevelPoint(HeroSpawnBasherEngagePrefab);
        HeroSpawnFinalLevel = SpawnLevelPoint(HeroSpawnFinalLevelPrefab, true);

        SelectNewHeroSpawnPoint(heroSpawnPoint);
    }

    public void SelectNewHeroSpawnPoint(HeroSpawnPoint heroSpawnPoint)
    {
        switch (heroSpawnPoint)
        {
            case HeroSpawnPoint.GeneralLevel:
                currentHeroSpawnPoint = HeroSpawnLevel;
                break;
            case HeroSpawnPoint.BasherChase:
                currentHeroSpawnPoint = HeroSpawnBasherChase;
                break;
            case HeroSpawnPoint.BasherEngage:
                currentHeroSpawnPoint = HeroSpawnBasherEngage;
                break;
            case HeroSpawnPoint.FinalBoss:
                currentHeroSpawnPoint = HeroSpawnFinalLevel;
                break;
            default:
                break;
        }
    }

    GameObject SpawnLevelPoint(GameObject spawnPoint, bool useChild = false)
    {
        GameObject newSpawnPoint = Instantiate(spawnPoint) as GameObject;
        newSpawnPoint.transform.parent = ActorsParent.transform;
        if (useChild)
        {
            newSpawnPoint.transform.localPosition = Vector3.zero;
            newSpawnPoint = newSpawnPoint.transform.GetChild(0).gameObject;
        }
        
        return newSpawnPoint;
    }




    void Update()
    {
        /*
        // this is necessary if hero respawn should occur at last position.
        // which will likely not be the case

        if (HeroShipWarp != null)
        {
            if (HeroShipWarp.activeSelf)
            {
                HeroShipWarp.transform.localPosition = HeroShip.transform.localPosition;
            }
        }
        */

        if (localSceneManagementRef != null && !noObjectRefsMissing)

        {
            ReplaceMissingObjects();
          //  print("xcxcxcxcxcxcx -- replace missingCalled from Update");
        }

        if (BossDefeated)
        {
            print("bossDefeated in SM");
            if (TileMoverObject != null)
            {
                TileMoverObject.GetComponent<TileMover>().TileSpeedResume = true;
            }
            BossDefeated = false;
        }
        
        if (GestureManager != null)
        {
            if (GestureManager.GetComponent<GestureManager>().IsScaling)
            {
                scalingInProgress = true;
            }

            else

            {
                // checks if not scaling

                if (scalingInProgress)
                {
                    scalingInProgress = false;
                    AdjustTerminatorDistance();
                }
            }
        }


        if (AddPow)
        {
            AddPow = false;
            IncreasePowLevel();
        }


        if (PowLevel != 0)
        {
            if (HeroBulletPoolInactive.Count == 0)
            {
                ExtraBodyTempInhibitFiring(false);
            }

            else

            {
                if (!canInhibitExtraBodyFiring)
                {
                    ExtraBodyTempInhibitFiring(true);
                }
            }
        }


        if (HeroShip != null)  // take this out of update and perform at level load //rma2019_10_10
        {
            if (heroshipAnimator == null)
            {
                heroshipAnimator = HeroShip.GetComponent<Animator>();
            }

            if (playerAudioSource == null)
            {
                playerAudioSource = HeroShip.GetComponent<AudioSource>();
            }
        }

        if (heroshipAnimator != null)
        {
            heroshipAnimator.SetFloat("multiplier", GameSpeed);
        }

        if (invincibleHero || starmanMode)
        {
            return;
        }

        if (currentHealth > beginHealth) currentHealth = beginHealth;

        if (gameObject.GetComponent<EnemyHealth>())
            currentHealth = gameObject.GetComponent<EnemyHealth>().health;


        if (currentHealth <= 0)
        {

            if (gameplayMode == GameplayMode.Showcase)
            {

                foreach (GameObject picked in ExtraBody)
                {
                    SetAnimBool(picked, "fast", false);
                }

                if (PowLevel == 1 || PowLevel == 2)
                {
                    PowLevel = 0;
                    ExtraBody[0].SetActive(false);
                    ExtraBody[0].GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = false;
                    StartCoroutine(RecoverFromHit());
                    // destroy current EB
                    return;
                }

                if (PowLevel > 2)
                {
                    PowLevel = 2;
                    blueExtraBody.SetActive(false);
                    blueExtraBody.GetComponentInChildren<starEnemy>().ProjectileFiringEnabled = false;
                    StartCoroutine(RecoverFromHit());
                    // remove 2nd EB
                    return;
                }
            }

            currentLives = currentLives - 1;

            PowLevel = 0;
            PowCheck();
            PlayAudio(playerAudioSource, PlayerExplodeClip);
            if (currentLives > 0)
            {
                StartCoroutine(Respawn());
            }

            {
                StartCoroutine(HeroDeath());
            }

        }

        if (currentLives <= 0)
        {
            //  gameOver.Invoke();  no longer exists, but GAAME OVER stuff goes here
        }

    }

    public float HeroRecoveryTime = 1.0f;

    IEnumerator RecoverFromHit()
    {
        PlayAudio(playerAudioSource, PlayerPowLevelLowerClip);
        PlayPowSpeechClip(playedSatelliteDestroyedAudioClip, SpeechSatelliteDestroyedClip);
        playedSatelliteDestroyedAudioClip = true;

        heroshipAnimator.SetBool("recover", true);
        SetHeroInvincibility(true);
        yield return new WaitForSeconds(HeroRecoveryTime);

        while (GamePaused)
        {
            yield return null;
        }
        heroshipAnimator.SetBool("recover", false);

        SetHeroInvincibility(false);
    }



    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RespawnAudioDelay);

        while (GamePaused)
        {
            yield return null;
        }
        PlayAudio(playerAudioSource, PlayerRespawnClip);

    }

}
