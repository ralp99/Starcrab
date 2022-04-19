using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniTankDirection { Forward, Reverse }
public enum MiniTankCannonHeight { Low, Medium, High }


public class BossBasherController : BossControllerGeneric
{


    public bool Debug_killMiniTanks;
    public GameObject[] IntactPanels;
    public GameObject[] DestroyedPanels;
    List<GameObject> activePanels = new List<GameObject>();
    
    public GameObject MainMeshHolder;
    public GameObject PlowMesh;
    public Animator animator;
    public GameObject Weakspot;
    public GameObject Garage;
    public GameObject GarageMesh;
    public GameObject GarageDoor;
    public GameObject[] GarageScreens;
    public GameObject MiniTankSource;
    public GameObject[] MiniTankDummies;
    public GameObject PlowExplodeMesh;

    private Animator garageDoorAnimator;

    public Dictionary<GameObject, GameObject> damagePanelLinks = new Dictionary<GameObject, GameObject>();

    public float DelayBasherAppear = 4.0f;

    public float DelayMiniTanksAppear = 11.0f;

    [Space]

    public float MiniTanksDriveSpeed = 1.0f;
    public float MiniTanksDriveTime = 2.5f;
    public float MiniTanksDriveTimeJitter = 1.0f;
    public float MiniTanksBackDistance = -0.25f;   //z
    public float MiniTanksFwdDistance = 0.5f;

    public float MiniTanksShotWait = 1.0f;
    public int MiniTanksVolleyCount = 3;
    public int RandomFireChance = 4;

    List<GameObject> miniTanksSetA = new List<GameObject>();
    List<GameObject> miniTanksSetB = new List<GameObject>();

    List<GameObject> miniTanksMoveForward = new List<GameObject>();
    List<GameObject> miniTanksMoveBackward = new List<GameObject>();

    public List<GameObject> preLaunchSideMissiles = new List<GameObject>();

    public bool skipIntroDelay;
    public bool miniTanksMovedOutOfGarage;
    public bool phase2AlreadyActivated;
    
    public bool BasherVulnerable;
    private float workingSpeed;
    public float WaitUntilChaseBegins = 5.0f;
    public float ChaseToOffscreenDuration = 30.0f;
    public float currentDistance;
    public float ChaseBehindDistance = 0.0f;
    // public int tileMoverBasherChaseLevel = 0;
    public List<SoTileMoverTileGroup> BasherChaseTilegroup;
    private bool garageDestroyed;

    [Space]

    public string Telegraph_attack = "telegraph";
    public string Plow_Attack = "attack_plow";
    public string Charge_Attack = "chargeSmall";
    public string Attack_Hatch = "attack_hatch";
    public string Attack_HatchPulse = "attack_hatchPulse";
    public string Attack_sideMissiles = "attack_sideMissiles";
    public string Attack_frontDoor = "attack_frontDoor";
    public string Attack_spread = "attack_spread";
    public string ToggleLeftSide = "leftSide";
    public string ToggleOpen = "open";

    

    [Space]

    public string WheelsPointLeft = "wheelsPointLeft";
    public string WheelsPointRight = "wheelsPointRight";
    public string WheelsPointStraight = "wheelsPointStraight";
    public string ExitChaseMode = "exitChase";
    public string ExitGarage = "basherExitGarage";
    public string ReturnBehindPlayer = "basherReturnBehindPlayer";
    public string ArriveFinalFight = "basherArriveFinalFight";

    public string finalFightBool = "finalFight";

    public enum BasherAttackMode { Plow, Charge, Hatch, SideMissiles, FrontDoor, SpreadGun, HatchPulse}

    public enum BasherAttackModifier { Left, Right }

    public BasherAttackMode basherAttackMode;
    private int basherAttackItemsLength;

    public BasherAttackModifier basherAttackModifier;

    public float DelayBetweenChaseAttacks = 5.0f;
    public float DelayBetweenChaseAttacksJitter = 2.0f;
    public float DelayBetweenHeadonAttacks = 3.0f;
    public float DelayBetweenHeadonAttacksJitter = 1.5f;

    public enum BasherMode { Hidden, Launch, Chase, HeadOn }
    public BasherMode basherMode;

    public bool debugSelectRandomBasherAttack;
    [Space]

    public GameObject BasherMissile;
    public GameObject BasherHatchBullet;
    public GameObject BasherProjectile;

    [Space]

    public float BasherLaunchBulletDelay = 1.0f;
    public float BasherLaunchProjectilesDelay = 1.0f;
    public float BasherLaunchMissilesInitDelay = 1.0f;
    public float BasherLaunchMissilesBetweenDelay = 0.2f;
    public float BasherSpreadBetweenDelay = 0.2f;
    public int BasherSpreadIterations = 2;
    public float BasherSpreadVertDegrees = 5.0f;
    public float BasherSpreadHorizDegrees = 3.0f;

    public float BasherHatchReturn = 1.3f;
    public float BasherRearHatchReturn = 0.67f;

    [Space]
    
    public GameObject[] MissileSpawnPoints_L;
    public GameObject[] MissileSpawnPoints_R;
    public GameObject BulletSpawnPoint;
    public GameObject SideDoorSpreadBulletSpawnPoint_L;
    public GameObject SideDoorSpreadBulletSpawnPoint_R;
    public GameObject FrontPivotDoorBulletSpawnPoint_L;
    public GameObject FrontPivotDoorBulletSpawnPoint_R;

    [Space]

    public GameObject[] FrontLaserHolders;

    [Space]

    public bool DebugBasherWeaponTestOverrideMode;
    public bool DebugMissile_L;
    public bool DebugMissile_R;
    public bool DebugHatchBullet;
    public bool DebugHatchPulse;
    public bool DebugSpread_L;
    public bool DebugSpread_R;
    public bool DebugFrontProj;
    public bool DebugCharge;
    public bool DebugPlow;
    
    public Vector3 newBossCoords;
    private Vector3 savedSpreadCoords;
    private int spreadFireCounter;
    
    private IEnumerator attackingCoroutine;

    public bool UserAbove;
    public bool UserLeft;
    public bool UserRight;

    private bool deathPulseActive;
    private bool swerveLeftActive;
    private bool swerveRightActive;
    
    public GameObject ColliderCheckAbove;
    public GameObject ColliderCheckRight;
    public GameObject ColliderCheckLeft;

    public CollisionCheck collisionCheckAbove;
    public CollisionCheck collisionCheckLeft;
    public CollisionCheck collisionCheckRight;

    private StarShipController starShipController;

    [Space]

    public float attackAudioDelay = 0.25f;
    public AudioClip GarageShutterAudio;
    public AudioClip GarageLowerAudio;

    public AudioClip EngineStartAudio;
    public AudioClip DrivingAudio;
    public AudioClip EngineIdle;
    public AudioClip AttackTelegraphAudio;
    public AudioClip AttackHatchDoorOpen;
    public AudioClip AttackHatchDoorClose;

    public AudioClip AttackDoorsOpen;
    public AudioClip AttackDoorsClose;
    public AudioClip AttackPulseAudio;
    public AudioClip AttackMissilesAudio;
    public AudioClip AttackSpreadAudio;
    public AudioClip AttackPlowAudio;
    public AudioClip AttackChargeAudio;
    public AudioClip AttackFrontProjAudio;
    public AudioClip AttackHatchMissile;

    public AudioClip DestroyPlowFragmentAudio;
    public AudioClip DestroyPlowFullAudio;
    public AudioSource AudioSourceGarage;

    public bool ActivateExplodeMesh;

    void Start()
    {

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        ResetBossPhase(true);
        audioSource = GetComponentInChildren<AudioSource>();

        int panelCounter = 0;
        basherAttackItemsLength = System.Enum.GetValues(typeof(BasherAttackMode)).Length;
        MainMeshHolder.SetActive(false);  //hides later-phase main basher if not already hidden

        if (GarageDoor != null)
        {
            garageDoorAnimator = GarageDoor.GetComponent<Animator>();
        }

        foreach (GameObject picked in IntactPanels)
        {
            activePanels.Add(picked);
            damagePanelLinks.Add(picked, DestroyedPanels[panelCounter]);
            picked.GetComponentInChildren<starEnemy>().parent = picked;
            picked.GetComponentInChildren<starEnemy>().AlertControllerOnDisable = gameObject;
            panelCounter++;
        }

        Weakspot.GetComponent<Collider>().enabled = false;
        collisionCheckAbove = ColliderCheckAbove.GetComponent<CollisionCheck>();
        collisionCheckLeft = ColliderCheckLeft.GetComponent<CollisionCheck>();
        collisionCheckRight = ColliderCheckRight.GetComponent<CollisionCheck>();

        if (MiniTankDummies[0] == null)
        {
            return;
        }

        /*
        for (int i = 0; i < MiniTankDummies.Length; i++)
        {
            GameObject currentMiniTank = Instantiate(MiniTankSource) as GameObject;

            currentMiniTank.transform.parent = MiniTankDummies[0].transform.parent.transform;
            currentMiniTank.transform.localPosition = MiniTankDummies[i].transform.localPosition;
            currentMiniTank.transform.localEulerAngles = MiniTankDummies[i].transform.localEulerAngles;

            currentMiniTank.GetComponentInChildren<starEnemy>().parent = currentMiniTank;
            currentMiniTank.GetComponentInChildren<starEnemy>().AlertControllerOnDisable = gameObject;
            ActiveBodies.Add(currentMiniTank);
        }
        */

        PopulateDummies(MiniTankDummies, MiniTankSource, MiniTankDummies[0], true, true, null, null);



        bool alternate = false;

        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            if (!alternate)
            {
                miniTanksSetA.Add(ActiveBodies[i]);
            }
            else
            {
                miniTanksSetB.Add(ActiveBodies[i]);
            }

            alternate = !alternate;
        }

        bossType = BossType.Basher;

        RunBossWarningMessage();
    }


    void RunAfterWarningMessage()
    {
        RunGenericBossTasks();
        StartCoroutine(MiniTanksAppear());
    }

    IEnumerator MiniTanksAppear()
    {


        if (skipIntroDelay)
        {
            DelayMiniTanksAppear = 0;
        }

        yield return new WaitForSeconds(DelayMiniTanksAppear);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            ActiveBodies[i].SetActive(true);
        }

        GarageDoorAnimation(true);
        StartCoroutine(MiniTankShootingCounter());
        StartCoroutine(AssignMiniTanksRollingGroups());
    }

    void GarageDoorAnimation(bool doorOpenState)
    {
        if (garageDoorAnimator != null)
        {
            garageDoorAnimator.SetBool("doorOpen", doorOpenState);
        }

        AudioSourceGarage.Play();
    }



    IEnumerator MiniTankShootingCounter()
    {
        yield return new WaitForSeconds(MiniTanksShotWait);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        MiniTanksFire();
        if (bossPhase == BossPhase.BossPhase1)
        {
            StartCoroutine(MiniTankShootingCounter());
        }
    }


    IEnumerator AssignMiniTanksRollingGroups()
    {
        yield return new WaitForSeconds(0);

        List<GameObject> AssignedMiniTankList = miniTanksSetA;
        bool DirectionAssignForward = true;


        if (Random.Range(0, 2) == 1)
        {
            AssignedMiniTankList = miniTanksSetB;
        }

        if (Random.Range(0, 2) == 1)
        {
            DirectionAssignForward = false;
        }

        if (!miniTanksMovedOutOfGarage)
        {
            DirectionAssignForward = true;
            AssignedMiniTankList = ActiveBodies;
        }
        else
        {
            if (AssignedMiniTankList[AssignedMiniTankList.Count - 1].transform.localPosition.z < MiniTanksBackDistance)
            {
                //too far back, reassigning to FWD
                DirectionAssignForward = true;
            }

            if ((AssignedMiniTankList[0].transform.localPosition.z > MiniTanksFwdDistance))
            {

                //too far fwd, reassigning to BACK
                DirectionAssignForward = false;
            }
        }

        StartCoroutine(RollTanksInFormation(AssignedMiniTankList, DirectionAssignForward, MiniTanksDriveTime));
        
    }


    IEnumerator RollTanksInFormation(List<GameObject> miniTanks, bool forward, float duration)
    {

        for (int i = 0; i < miniTanks.Count; i++)
        {
            if (forward)
            {
                miniTanksMoveForward.Add(miniTanks[i]);
            }
            else
            {
                miniTanksMoveBackward.Add(miniTanks[i]);
            }
        }

        duration = duration + Random.Range(0, MiniTanksDriveTimeJitter);

        yield return new WaitForSeconds(duration);

        miniTanksMoveForward.Clear();
        miniTanksMoveBackward.Clear();

        if (bossPhase == BossPhase.BossPhase1)
        {
            StartCoroutine(AssignMiniTanksRollingGroups());
        }

    }


    public void MiniTanksFire()
    {

        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            int randomFire = Random.Range(0, RandomFireChance);
            if (randomFire == 1 && ActiveBodies[i].activeSelf == true)

            {
                starEnemy currentStarEnemy = ActiveBodies[i].GetComponentInChildren<starEnemy>();
              //  currentStarEnemy.IncrementCurrentProjectileSpawnPointCounter();
                currentStarEnemy.FireProjectileNoParent();
            }
        }
    }

    

    // END MINI TANKS // ------------------------------------------------------------

        
    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER CONTROLLERS - ALL
    /// </summary>
    // --------------------------------------------------------------------------

        void BasherWeakpointVulnerable()
    {
        PlowMesh.SetActive(false);
        Weakspot.GetComponent<Collider>().enabled = true;
        GameObject explodingPlowMesh = Instantiate(PlowExplodeMesh) as GameObject;
        explodingPlowMesh.transform.parent = PlowMesh.transform.parent.transform;
        explodingPlowMesh.transform.localPosition = Vector3.zero;
        explodingPlowMesh.transform.localScale = Vector3.one;
        audioSource.PlayOneShot(DestroyPlowFullAudio);
    }

    IEnumerator ChaseToOffscreenCounter()
    {

        yield return new WaitForSeconds(ChaseToOffscreenDuration);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }
        
        if (attackingCoroutine != null)
        {
            StopCoroutine(attackingCoroutine);
            attackingCoroutine = null;
        }
        // exits mode, rushes ahead, then inverts, and player can engage head-on

        animator.SetTrigger(ExitChaseMode);
        deathPulseActive = true; // so he doesn't blast you while going past you;
        StartCoroutine(DriveToEndBattle());
        StartCoroutine(AudioFadeController(true, 3));
    }


    IEnumerator DriveToEndBattle()
    {
        starGameManagerRef.SwitchHeroSpawnPoint(starGameManagerRef.HeroSpawnBasherEngage);
        starShipController.usingRespawnSpeedMultiplier = starShipController.RespawnSpeedMultiplier;

        yield return new WaitForSeconds(10);
        // this is just how long he sits and stares at you facing you, before attacking

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        animator.SetBool(finalFightBool, true);
        animator.SetTrigger(ArriveFinalFight);

        //reparent to tilemover
        //stop tilemover
        //re enable animator?
        //re enable attacks
        //enemy can be hurt
        // stop BG moving

        StartCoroutine(EndBattle());
        audioSource.clip = EngineIdle;
        audioSource.Play();
        StartCoroutine(AudioFadeController(false, 3));


    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(5);
        
        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

       // bossPhase = BossPhase.BossPhase4;  // might reassign this to be after core is exposed

        starGameManagerRef.TileMoverObject.GetComponent<TileMover>().SetTileSpeedSlowHalt();
        deathPulseActive = false; // Can attack you with death pulse again
        SetPlowPanelsInvulnerable(false);
        attackingCoroutine = BasherChaseAttack();
        StartCoroutine(attackingCoroutine);
    }
    
    

    IEnumerator BasherChaseAttack()
    {

        float currentDelay = 0.0f;

        if (!DebugBasherWeaponTestOverrideMode)
        {
            currentDelay = DelayBetweenChaseAttacks + Random.Range(0, DelayBetweenChaseAttacksJitter);
        }

        yield return new WaitForSeconds(currentDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }



        animator.SetTrigger(Telegraph_attack);

        if (!DebugBasherWeaponTestOverrideMode)
        {
            SelectCurrentRandomBasherAttack();
        }

        bool useLeftSide = false;
        if (basherAttackModifier == BasherAttackModifier.Left)
        {
            useLeftSide = true;
        }


      //  basherAttackMode = BasherAttackMode.Hatch; // just for debug, only fires hatch bullet for any attack

        switch (basherAttackMode)
        {
            case BasherAttackMode.FrontDoor:
                BasherFrontDoorAttack();
                break;

            case BasherAttackMode.Hatch:
                BasherHatchBulletAttack();
                break;

            case BasherAttackMode.Plow:
                animator.SetTrigger(Plow_Attack);
                audioSource.PlayOneShot(AttackPlowAudio);
                break;

            case BasherAttackMode.Charge:
                animator.SetTrigger(Charge_Attack);
                audioSource.PlayOneShot(AttackChargeAudio);
                break;

            case BasherAttackMode.SideMissiles:
                BasherSideMissilesAttack(useLeftSide);
                break;

            case BasherAttackMode.SpreadGun:
                BasherSpreadAttack(useLeftSide);
                break;

            case BasherAttackMode.HatchPulse:
                   BasherHatchPulse();
                break;
        }

        if (!DebugBasherWeaponTestOverrideMode)
        {
            attackingCoroutine = null;
            attackingCoroutine = BasherChaseAttack();
            StartCoroutine(attackingCoroutine);
        }

    }




    IEnumerator BossBasherAppear()
    {
        // boss first appears in garage
        //  starGameManagerRef.TileMoverObject.GetComponent<TileMover>().ChangeCurrentLevel(tileMoverBasherChaseLevel);
        // tell SO to be switched into tilemover over here

        //  starGameManagerRef.TileMoverObject.GetComponent<TileMover>().ReplaceTileMoverGroup(0, BasherChaseTilegroup);
        starGameManagerRef.TileMoverObject.GetComponent<TileMover>().ReplaceTileMoverGroup(BasherChaseTilegroup);


        yield return new WaitForSeconds(DelayBasherAppear);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        if (starGameManagerRef != null)
        {
            Garage.gameObject.transform.parent = gameObject.transform.parent;
            gameObject.transform.parent = starGameManagerRef.ActorsParent.transform;
            gameObject.GetComponent<PoolListMembership>().PooledParent.GetComponent<PoolListMembership>().PooledChildWasDisabled(gameObject);
            gameObject.transform.localPosition = newBossCoords;
        }

        GarageDoorAnimation(true);

        if (MainMeshHolder != null)
        {
            MainMeshHolder.SetActive(true);
        }

        audioSource.clip = EngineStartAudio;
        audioSource.Play();
        StartCoroutine(BasherExitGarage());
    }



    IEnumerator BasherExitGarage()
    {


        yield return new WaitForSeconds(DelayBasherAppear);
        
        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        animator.SetTrigger(ExitGarage);
        StartCoroutine(BeginChasing());
        starGameManagerRef.TileMoverObject.GetComponent<TileMover>().SetTileSpeedResume();
        audioSource.loop = true;  // set this before assigning clip
        audioSource.clip = DrivingAudio;
        audioSource.Play();
        StartCoroutine(AudioFadeController(true, 4));   // fade audio to 0
        StartCoroutine(LowerGarageMeshIntoGround());

    }


    IEnumerator LowerGarageMeshIntoGround()
    {
        Transform newRiserParent = GarageMesh.transform;
        GameObject floorRiserL = GameObject.Find("sc_lvl1_floortile_riser_L");
        GameObject floorRiserR = GameObject.Find("sc_lvl1_floortile_riser_R");

        floorRiserR.transform.parent = newRiserParent;
        floorRiserL.transform.parent = newRiserParent;

        yield return new WaitForSeconds(3.0f);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        GarageMesh.GetComponent<Animator>().SetTrigger("garageMeshLower");
        
        StartCoroutine(ChangeDrawVolumeScale());
        // play garage receding audio on garageMesh or w/e source
        Garage.GetComponent<AudioSource>().clip = GarageLowerAudio;
        Garage.GetComponent<AudioSource>().Play();
    }


    IEnumerator ChangeDrawVolumeScale()
    {
        GameObject drawVolumeObject = starGameManagerRef.DrawVolume;
        Vector3 newDrawVolumeScale = new Vector3(drawVolumeObject.transform.localScale.x, drawVolumeObject.transform.localScale.y, 0.75f);
        Vector3 revertDrawVolumeScale = new Vector3(drawVolumeObject.transform.localScale.x, drawVolumeObject.transform.localScale.y, drawVolumeObject.transform.localScale.z);

        starGameManagerRef.DrawVolumeScale(newDrawVolumeScale);

        yield return new WaitForSeconds(7);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        starGameManagerRef.DrawVolumeScale(revertDrawVolumeScale);

    }


    IEnumerator BeginChasing()
    {
        yield return new WaitForSeconds(WaitUntilChaseBegins);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        bossPhase = BossPhase.BossPhase3;
        animator.SetTrigger(ReturnBehindPlayer);
        starGameManagerRef.SwitchHeroSpawnPoint(starGameManagerRef.HeroSpawnBasherChase);
        starShipController.usingRespawnSpeedMultiplier = starShipController.ChasedRespawnSpeedMultiplier;

        attackingCoroutine = BasherChaseAttack();
        StartCoroutine(attackingCoroutine);
        StartCoroutine(ChaseToOffscreenCounter());
        StartCoroutine(AudioFadeController(false, 3)); // bring vol back up

    }



    IEnumerator AudioFadeController(bool audioOff, float fadeDuration)
    {
        float currentTime = 0;

        if (audioOff)
        {
            while (audioSource.volume > 0)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(1, 0, currentTime / fadeDuration);
                yield return null;
            }
        }
        else
        {
            while (audioSource.volume < 1)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, 1, currentTime / fadeDuration);
                yield return null;
            }
        }

    }



    void SelectCurrentRandomBasherAttack()
    {

        if (!DebugBasherWeaponTestOverrideMode)
        {
            basherAttackMode = (BasherAttackMode)Random.Range(0, basherAttackItemsLength);
            basherAttackModifier = (BasherAttackModifier)Random.Range(0, 2);
        }

        // don't want to randomly select this defense
        //  if (basherAttackMode == BasherAttackMode.HatchPulse ||
        //   basherAttackMode == BasherAttackMode.FrontDoor)  // restore frontDoor after it's been implemented

        if (basherAttackMode == BasherAttackMode.HatchPulse)
        {
            SelectCurrentRandomBasherAttack();
        }




        if (Weakspot.GetComponent<Collider>().enabled == true)
        {
            if (basherAttackMode == BasherAttackMode.Plow)
            {
                SelectCurrentRandomBasherAttack();
            }
        }
    }


    void BasherWeaponTesting()
    {
        // these are just for debugging and testing

        bool shouldTestAttack = false;

        if (DebugHatchBullet)
        {
            basherAttackMode = BasherAttackMode.Hatch;
            shouldTestAttack = true;
        }

        if (DebugCharge)
        {
            basherAttackMode = BasherAttackMode.Charge;
            shouldTestAttack = true;
        }

        if (DebugMissile_L)
        {
            basherAttackMode = BasherAttackMode.SideMissiles;
            basherAttackModifier = BasherAttackModifier.Left;
            shouldTestAttack = true;
        }

        if (DebugMissile_R)
        {
            basherAttackMode = BasherAttackMode.SideMissiles;
            basherAttackModifier = BasherAttackModifier.Right;
            shouldTestAttack = true;
        }

        if (DebugSpread_L)
        {
            basherAttackMode = BasherAttackMode.SpreadGun;
            basherAttackModifier = BasherAttackModifier.Left;
            shouldTestAttack = true;
        }

        if (DebugSpread_R)
        {
            basherAttackMode = BasherAttackMode.SpreadGun;
            basherAttackModifier = BasherAttackModifier.Right;
            shouldTestAttack = true;
        }

        if (DebugPlow)
        {
            basherAttackMode = BasherAttackMode.Plow;
            shouldTestAttack = true;
        }

        if (DebugFrontProj)
        {
            basherAttackMode = BasherAttackMode.FrontDoor;
            shouldTestAttack = true;
        }

        if (DebugHatchPulse)
        {
            basherAttackMode = BasherAttackMode.HatchPulse;
            shouldTestAttack = true;
        }

        DebugHatchBullet = false;
        DebugMissile_L = false;
        DebugMissile_R = false;
        DebugCharge = false;
        DebugSpread_L = false;
        DebugSpread_R = false;
        DebugPlow = false;
        DebugFrontProj = false;
        DebugHatchPulse = false;

        if (shouldTestAttack)
        {
            shouldTestAttack = false;

            if (attackingCoroutine != null)
            {
                StopCoroutine(attackingCoroutine);
            }
            attackingCoroutine = (BasherChaseAttack());
            StartCoroutine(attackingCoroutine);
        }
    }


    IEnumerator AudioAttack(AudioClip audioClip, IEnumerator nextCoroutine)
    {
        yield return new WaitForSeconds(attackAudioDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        audioSource.PlayOneShot(audioClip);
        StartCoroutine(nextCoroutine);
    }


    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER HATCH ATTACK
    /// </summary>
    // --------------------------------------------------------------------------


    void BasherHatchBulletAttack()
    {
        animator.SetBool(ToggleOpen, true);
        animator.SetTrigger(Attack_Hatch);
        StartCoroutine(BasherHatchLaunchBullet());
        audioSource.PlayOneShot(AttackHatchDoorOpen);
    }


    IEnumerator BasherHatchLaunchBullet()
    {
        yield return new WaitForSeconds(BasherLaunchBulletDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        // shotdirection is irrelevant

        audioSource.PlayOneShot(AttackHatchMissile);
        GenericProjectileFire(BasherHatchBullet, BulletSpawnPoint, ShotDirection.Right);
        Transform cachedParent = CurrentProjectile.transform.parent.transform;
        CurrentProjectile.transform.parent = BulletSpawnPoint.transform.parent;  // delete this?
        CurrentProjectile.transform.parent = cachedParent;  // why are there two?

        string hatchBulletLength = "short";
        int randomHatchBulletLength = Random.Range(0, 3);

        switch (randomHatchBulletLength)
        {
            case 1:
                hatchBulletLength = "long";
                break;
            case 2:
                hatchBulletLength = "straight";
                break;
            default:
                break;
        }

        CurrentProjectile.GetComponentInChildren<Animator>().SetTrigger(hatchBulletLength);
        StartCoroutine(BasherHatchReplace());
    }


    IEnumerator BasherHatchReplace()
    {
        yield return new WaitForSeconds(BasherHatchReturn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        animator.SetBool(ToggleOpen, false);
        animator.SetTrigger(Attack_Hatch);
        audioSource.PlayOneShot(AttackHatchDoorClose);
    }



    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER HATCH PULSE ATTACK
    /// </summary>
    // --------------------------------------------------------------------------

    void BasherHatchPulse()
    {
        if (deathPulseActive)
        {
            return;
        }

        deathPulseActive = true;
        animator.SetBool(ToggleOpen, true);
        animator.SetTrigger(Attack_HatchPulse);
        audioSource.PlayOneShot(AttackHatchDoorOpen);
        StartCoroutine(AudioAttack(AttackPulseAudio, BasherHatchPulseReplace()));
    }
    
    IEnumerator BasherHatchPulseReplace()
    {
        yield return new WaitForSeconds(BasherRearHatchReturn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        deathPulseActive = false; // might need a separate coroutine for this
        animator.SetBool(ToggleOpen, false);
        animator.SetTrigger(Attack_HatchPulse);
        audioSource.PlayOneShot(AttackHatchDoorClose);
    }

    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER SPREAD ATTACK
    /// </summary>
    // --------------------------------------------------------------------------

    void BasherSpreadAttack(bool leftSide)
    {
        animator.SetBool(ToggleOpen, true);
        animator.SetBool(ToggleLeftSide, leftSide);
        animator.SetTrigger(Attack_spread);

        GameObject spawnPoint = SideDoorSpreadBulletSpawnPoint_R;

        if (leftSide)
        {
            spawnPoint = SideDoorSpreadBulletSpawnPoint_L;
        }

        savedSpreadCoords = spawnPoint.transform.localEulerAngles;
        spreadFireCounter = 0;
        StartCoroutine(BasherSpreadFireIteration(leftSide, spawnPoint));
        audioSource.PlayOneShot(AttackDoorsOpen);
    }


    IEnumerator BasherSpreadFireIteration(bool leftSide, GameObject spawnPoint)
    {
        yield return new WaitForSeconds(BasherSpreadBetweenDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        int shotDirectionMultiplier = 1;

        if (!leftSide)
        {
            shotDirectionMultiplier = -1;
        }

        GenericProjectileFire(BasherProjectile, spawnPoint, ShotDirection.Right);
        audioSource.PlayOneShot(AttackSpreadAudio);
        spreadFireCounter++;
        spawnPoint.transform.eulerAngles = new Vector3(spawnPoint.transform.eulerAngles.x+BasherSpreadVertDegrees,
            spawnPoint.transform.eulerAngles.y+BasherSpreadHorizDegrees*shotDirectionMultiplier,
            spawnPoint.transform.eulerAngles.z);

        if (spreadFireCounter < BasherSpreadIterations)
        {
            StartCoroutine(BasherSpreadFireIteration(leftSide, spawnPoint));
        }
        else
        {
            spawnPoint.transform.localEulerAngles = savedSpreadCoords;
            StartCoroutine(BasherSpreadReplace(leftSide));
        }
    }
    

    IEnumerator BasherSpreadReplace(bool leftSide)
    {
        yield return new WaitForSeconds(BasherHatchReturn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        animator.SetBool(ToggleOpen, false);
        animator.SetTrigger(Attack_spread);
        audioSource.PlayOneShot(AttackDoorsClose);
    }



    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER FRONT DOOR ATTACK
    /// </summary>
    // --------------------------------------------------------------------------

    void BasherFrontDoorAttack()

    {
        animator.SetBool(ToggleOpen, true);
        animator.SetTrigger(Attack_frontDoor);
        audioSource.PlayOneShot(AttackDoorsOpen);
        StartCoroutine(BasherFrontDoorLasers());
    }
    


    IEnumerator BasherFrontDoorLasers()
    {
        yield return new WaitForSeconds(0.35f);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        foreach (GameObject picked in FrontLaserHolders)
        {
            picked.SetActive(true);
            picked.GetComponentInChildren<Animator>().SetTrigger("extend");
        }

        StartCoroutine(AudioAttack(AttackFrontProjAudio, BasherFrontDoorReplace()));
    }





    IEnumerator BasherFrontDoorReplace()
    {
        yield return new WaitForSeconds(BasherHatchReturn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        foreach (GameObject picked in FrontLaserHolders)
        {
            picked.GetComponentInChildren<Animator>().SetTrigger("retract");
            picked.SetActive(false);
        }

        animator.SetBool(ToggleOpen, false);
        animator.SetTrigger(Attack_frontDoor);
        audioSource.PlayOneShot(AttackDoorsClose);
    }



    // --------------------------------------------------------------------------
    /// <summary>
    ///  BASHER SIDE MISSILES ATTACK
    /// </summary>
    // --------------------------------------------------------------------------


    void BasherSideMissilesAttack(bool leftSide)
    {
        animator.SetBool(ToggleOpen, true);
        animator.SetBool(ToggleLeftSide, leftSide);
        animator.SetTrigger(Attack_sideMissiles);
        audioSource.PlayOneShot(AttackDoorsOpen);


        List<GameObject> inheritFromList = new List<GameObject>();

        if (leftSide)
        {
            inheritFromList = new List<GameObject>(MissileSpawnPoints_L);
        }
        else
        {
            inheritFromList = new List<GameObject>(MissileSpawnPoints_R);
        }

        preLaunchSideMissiles.Clear();

        foreach (GameObject currentSpawnPoint in inheritFromList)

        {
            GenericProjectileFire(BasherMissile, currentSpawnPoint, ShotDirection.Right);
            CurrentProjectile.GetComponent<StarProjectileAnim>().DelayFiring = true;
            preLaunchSideMissiles.Add(CurrentProjectile);
            CurrentProjectile.transform.parent = currentSpawnPoint.transform;
        }

        StartCoroutine(BasherSideMissilesIncrementallyFire(leftSide));
    }


    


    IEnumerator BasherSideMissilesIncrementallyFire(bool leftSide)
    {
        yield return new WaitForSeconds(BasherLaunchMissilesBetweenDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        GameObject currentSideMissile = preLaunchSideMissiles[0];
        currentSideMissile.transform.parent = starGameManagerRef.ActorsParent.transform;

        currentSideMissile.GetComponent<StarProjectileAnim>().DelayFiring = false;
        audioSource.PlayOneShot(AttackMissilesAudio);
        currentSideMissile.GetComponent<StarExhaust>().ExhaustListSetting(true);
        preLaunchSideMissiles.Remove(currentSideMissile);

        if (preLaunchSideMissiles.Count > 0)
        {
            StartCoroutine(BasherSideMissilesIncrementallyFire(leftSide));
        }
        else
        {
            StartCoroutine(BasherSideMissilesReplace(leftSide));
        }

    }


    

    IEnumerator BasherSideMissilesReplace(bool leftSide)
    {
        yield return new WaitForSeconds(BasherHatchReturn);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        animator.SetBool(ToggleOpen, false);
        animator.SetTrigger(Attack_sideMissiles);
        audioSource.PlayOneShot(AttackDoorsClose);

    }

    
/// <summary>
/// UPDATE
/// </summary>
        

    void Update()
    {

       // print("bossphase is " + bossPhase);

        /*
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
        */

        if (starGameManagerRef != null)
        {
            if (starShipController == null)
            {
                starShipController = starGameManagerRef.starShipController;
            }

            if (starGameManagerRef.GamePaused)
            {
                return;
            }
        }

        if (!starGameManagerRef.BossWarningMessagePlaying && !RanGenericBossTasks)
        {
            RunAfterWarningMessage();
        }

        if (debugSelectRandomBasherAttack)
        {
            debugSelectRandomBasherAttack = false;
            SelectCurrentRandomBasherAttack();
        }

        if (DebugBasherWeaponTestOverrideMode)
        {
            BasherWeaponTesting();
        }
        

        if (Garage != null)
        {
            if (phase2AlreadyActivated && !garageDestroyed)
            {
                if (!Garage.transform.parent.gameObject.activeSelf)
                {
                    Destroy(Garage);
                    garageDestroyed = true;
                }
            }
        }

        

        if (DisabledSubObjects.Count != 0)
        {
            
            List<GameObject> removeObjectList = new List<GameObject>();
            foreach (GameObject picked in DisabledSubObjects)
            {
                removeObjectList.Add(picked);
                GameObject currentSubObject = picked;
                ActiveBodiesUpdater(picked);

                if (bossPhase == BossPhase.BossPhase3)
                {
                    if (damagePanelLinks.ContainsKey(currentSubObject))
                    {
                        damagePanelLinks[currentSubObject].SetActive(true);
                        audioSource.PlayOneShot(DestroyPlowFragmentAudio);
                    }

                    if (AllActiveBodiesCleared)
                    {
                        BasherWeakpointVulnerable();
                    }
                }
            }

            foreach (GameObject picked in removeObjectList)
            {
                   DisabledSubObjects.Remove(picked);
            }

        }



        if ((bossPhase == BossPhase.BossPhase1 && !phase2AlreadyActivated && AllActiveBodiesCleared) ||
                Debug_killMiniTanks)

            {
                Debug_killMiniTanks = false;
                bossPhase = BossPhase.BossPhase2;
                phase2AlreadyActivated = true;
                AllActiveBodiesCleared = false;

                if (!miniTanksMovedOutOfGarage)
                {
                    miniTanksMovedOutOfGarage = true;
                    GarageDoorAnimation(false);
                }

                for (int i = 0; i < IntactPanels.Length; i++)
                {
                    ActiveBodies.Add(IntactPanels[i]);
                }

                SetPlowPanelsInvulnerable(true);

                StartCoroutine(BossBasherAppear());
            }


            if (bossPhase == BossPhase.BossPhase1)
            {

                if (!miniTanksMovedOutOfGarage)
                {
                    if (ActiveBodies[ActiveBodies.Count - 1].transform.localPosition.z > MiniTanksBackDistance)
                    {
                        miniTanksMovedOutOfGarage = true;
                        GarageDoorAnimation(false);
                    }
                }

                for (int i = 0; i < miniTanksMoveForward.Count; i++)
                {
                    miniTanksMoveForward[i].transform.Translate(0, MiniTanksDriveSpeed * Time.deltaTime, 0);
                }


                if (miniTanksMoveBackward.Count > 0)
                {
                    if (miniTanksMoveBackward[miniTanksMoveBackward.Count - 1].transform.localPosition.z > MiniTanksBackDistance)
                    {
                        for (int i = 0; i < miniTanksMoveBackward.Count; i++)
                        {
                            miniTanksMoveBackward[i].transform.Translate(0, -1 * MiniTanksDriveSpeed * Time.deltaTime, 0);
                        }
                    }
                }
            }

        /*
       // restore this if necessary for debug
        if (phase2AlreadyActivated)
        {
            // this might cause trouble??
            bossPhase = BossPhase.BossPhase2;
            phase2AlreadyActivated = false;
        }
        */



        if (bossPhase == BossPhase.BossPhase3)
        {


            if (!deathPulseActive)
            {
                CheckForUserTooClose();
            
                if (UserAbove)
                {
                    BasherHatchPulse();
                    ResetUserPositionSensors();
                }
                else
                {
                    if (UserLeft)
                    {
                        animator.SetTrigger(WheelsPointLeft);
                        ResetUserPositionSensors();

                    }
                    else
                    {
                        if (UserRight)
                        {
                            animator.SetTrigger(WheelsPointRight);
                            ResetUserPositionSensors();
                        }
                    }
                }
            }
        }



            if (ActivateExplodeMesh)  // delete after debugging
        {
            ActivateExplodeMesh = false;
            BasherWeakpointVulnerable();
        }

    }  //end UPDATE

 
    void SetPlowPanelsInvulnerable (bool invulnerable)
    {
        foreach (GameObject panel in IntactPanels)
        {
            panel.GetComponent<EnemyHealth>().cantTakeDamage = invulnerable;
        }
    }

    void CheckForUserTooClose()
    {
        UserAbove = collisionCheckAbove.IsHittingTag;
        UserLeft = collisionCheckLeft.IsHittingTag;
        UserRight = collisionCheckRight.IsHittingTag;
    }


    void ResetUserPositionSensors()
    {
        UserAbove = false;
        UserLeft = false;
        UserRight = false;
    }
    
}
