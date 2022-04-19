using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Boss01Controller : BossControllerGeneric {

    public GameObject BossNodePrefab;
    public GameObject BossMiniCannonPrefab;
    public GameObject[] NodePlaceDummy;
    public GameObject[] CannonPlaceDummy;
    public GameObject[] bulletSpreadSpawnPoints;
    public GameObject[] EnableRenderers;
    // public StarProjectileAnim.ShotDirection BulletSpreadDirection = StarProjectileAnim.ShotDirection.Left;
    public ShotDirection BulletSpreadDirection = ShotDirection.Left;

    public GameObject MainHitbox;
    public Animator BossAnimator;
    private List<GameObject> activeMiniCannons = new List<GameObject>();

    public GameObject ProjectileSpawnPoint;
    public GameObject ProjectileA;
    public GameObject OuterDoorsCollision;
   
    public float InitialWaitTime;
    public float NodesExposedDuration;
    public float MinicannonFireRate = 0.35f;
    public float Phase1CannonAttackDurationSetup = 3.0f;
    public float Phase1CannonAttackDurationShoot = 2.0f;
    //public float WaitBetweenSweeps;
    public string MainDoorsBool = "mainDoorsOpen";
    public string SmallDoorsBool = "smallDoorsOpen";
    public string CannonExtendTrgr = "cannonExtended";
    public string CannonSweepTrgr = "cannonSweep";
    private IEnumerator currentCoroutine;
    private int currentMiniCannonFiring;
    
    [Space]
    public Color InactiveColor;
    public Color Phase1ChargeColor;
    public Color Phase1ShootColor;
    public Color Phase2ChargeColor;
    public Color Phase2AttackColor;
    public float PipeChargeOff = 0.2f;
    public float PipeChargeFull = 0.42f;
    public float PipeChargeThrough = 0.79f;

    public float PipeChargeGlobalDelay = 1.0f;
    public float PipeChargeTime = 0.75f;

    public float PipeDischargeShotTime = 1.3f;
    public float PipeLaserSpeedMultiplier = 0.5f;

    public bool FlipTiling;
    bool laserActive;
    bool ranPipesDischarge;

    public AudioClip ChargeUp;
    public AudioClip BulletsFireClip;
    public AudioClip LaserSwordClip;
    public AudioClip DoorsOpenClip;
    public AudioClip DoorsCloseClip;

    [System.Serializable]

    public class damageSwapList
    {
        public GameObject[] ObjectsHide;
        public GameObject[] ObjectsShow;
    }

    public List<damageSwapList> DamageSwapList;

    
     public Dictionary<GameObject, damageSwapList> damageObjectLinks =
         new Dictionary<GameObject, damageSwapList>();



    [System.Serializable]

    public class instanceList
    {
        public string note = "";
        public GameObject PipeObject;
       // [Range(0.0f, 1.0f)]
        // public float Yslider;
        public float PipeChargeTime;
        public float Delay;
        [HideInInspector]
        public Color LastPipeColor;
        [HideInInspector]
        public Material material;
    }

    public List<instanceList> InstanceList;

    public UnityEvent BossDamageEvent;

    public void LaserShotAudio()
    {
        starGameManagerRef.PlayAudio(audioSource, LaserSwordClip);
    }

    public void ChargePipesLaser()
    {
        StartCoroutine(ChargePipes(0.0f));
    }

    IEnumerator ChargePipes(float duration)
    {
        ranPipesDischarge = false;

        yield return new WaitForSeconds(duration);
        starGameManagerRef.PlayAudio(audioSource, ChargeUp);

        foreach (instanceList inList in InstanceList)
        {
            StartCoroutine(ChargeUpLerpDelay(inList.material, inList.PipeChargeTime, inList.Delay, inList.LastPipeColor));
        }
        
    }






    IEnumerator PipesShootStateCounter()
    {
 
        yield return new WaitForSeconds(PipeDischargeShotTime);

        foreach (instanceList inList in InstanceList)
        {
            StartCoroutine(PipesTexLerp(inList.material, inList.PipeChargeTime, inList.LastPipeColor, false));
        }
    }
    

    IEnumerator ChargeUpLerpDelay(Material material, float pipeChargeTime, float delay, Color lastPipeColor)
    {

        yield return new WaitForSeconds(delay);
        StartCoroutine(PipesTexLerp(material, pipeChargeTime, lastPipeColor, true));

    }


    IEnumerator PipesTexLerp(Material material, float pipeChargeTime, Color lastPipeColor, bool animStarting)

    {
        float pipeBeginPoint = 0.0f;
        float pipeEndPoint = 0.0f;
        Color goalColor = Color.white;

        if (animStarting)
        {
            pipeEndPoint = PipeChargeFull;
            goalColor = Phase1ChargeColor;
            pipeBeginPoint = PipeChargeThrough;

        }

        
        else

        {
            // for shot discharge

            pipeBeginPoint = PipeChargeFull;
            goalColor = Phase1ShootColor;
            pipeEndPoint = PipeChargeOff;

            pipeChargeTime = pipeChargeTime * 0.5f;
        }
        

       if (laserActive)
        {
            // forces it to skip hold & go all the way through

            pipeChargeTime = pipeChargeTime * PipeLaserSpeedMultiplier;
            goalColor = Phase2AttackColor;
            pipeEndPoint = PipeChargeOff;

        }

        for (float i = 0; i < pipeChargeTime; i += 0.1f)
        {
            // material.SetFloat("_rangeSliderY", Mathf.Lerp(pipeBeginPoint, pipeEndPoint, i / pipeChargeTime));
            //  material.SetFloat("_rangeSliderY", Mathf.Lerp(pipeBeginPoint, pipeEndPoint, Time.deltaTime));

            //  material.SetFloat("_rangeSliderY", Mathf.Lerp(pipeBeginPoint, pipeEndPoint, (i / pipeChargeTime)*10  ));
           // material.SetFloat("_rangeSliderY", Mathf.Lerp(pipeBeginPoint, pipeEndPoint, (i / pipeChargeTime) * 5));
            material.SetFloat("_rangeSliderY", Mathf.Lerp(pipeBeginPoint, pipeEndPoint, (i / pipeChargeTime) * 2));



            material.SetColor("_EmissiveColor", Color.Lerp(lastPipeColor, goalColor, i / pipeChargeTime));

            yield return null;

            foreach (instanceList inList in InstanceList)
            {

                if (animStarting)
                {
                    inList.LastPipeColor = Phase1ChargeColor;
                }

                else

                {
                    inList.LastPipeColor = InactiveColor;
                }
            }
        }


        // cannot do this here, runs too many times
        
        if (animStarting && !laserActive && !ranPipesDischarge)
        {
            ranPipesDischarge = true;
            StartCoroutine(PipesShootStateCounter());
        }
        
    }










    public void ProjectileFire(GameObject useProjectile, GameObject spawnPoint)
    {
    
        GameObject useObject = null;
        useObject = starGameManagerRef.SpawnedChecker(useProjectile);

        if (useObject == null)
        {
            return;
        }

        useObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, false);

        // useObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, true);
        //  useObject.transform.localPosition = spawnPoint.transform.localPosition;
        useObject.transform.position = spawnPoint.transform.position;

        useObject.transform.rotation = spawnPoint.transform.rotation;
        useObject.GetComponent<StarProjectileAnim>().shotDirection = BulletSpreadDirection;
        useObject.SetActive(true);
        


        /*
        GameObject activeProjectile = ProjectileA;
        if (!useProjectileA)
        {
            activeProjectile = ProjectileB;
        }

        GameObject useObject = null;
        useObject = starGameManagerRef.SpawnedChecker(activeProjectile);

        if (useObject == null)
        {
            return;
        }


        useObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, false);
        useObject.transform.position = ProjectileSpawnPoint.transform.position;
        useObject.transform.rotation = ProjectileSpawnPoint.transform.rotation;
        */
    }


    void ChangeInnerMeshRenderStates(bool state)
    {
        foreach (GameObject picked in EnableRenderers)
        {
            picked.GetComponent<Renderer>().enabled = state;
        }
    }




    void Start()
    {

        ResetBossPhase(true);
        ChangeInnerMeshRenderStates(false);
        audioSource = GetComponent<AudioSource>();
        
        for (int i = 0; i < NodePlaceDummy.Length; i++)
        {
            InstantiateNodes(i, NodePlaceDummy, BossNodePrefab, gameObject);
        }

        for (int i = 0; i < CannonPlaceDummy.Length; i++)
        {
            InstantiateNodes(i, CannonPlaceDummy, BossMiniCannonPrefab, gameObject);
        }
        


        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            if (ActiveBodies[i].GetComponentInChildren<starEnemy>().projectile != null)
            {
               activeMiniCannons.Add(ActiveBodies[i]);
            }

            damageObjectLinks.Add(ActiveBodies[i], DamageSwapList[i]);
           // ActiveBodies[i].SetActive(true); 
        }

    
        
        foreach (instanceList inList in InstanceList)
        {
            inList.material = inList.PipeObject.GetComponent<Renderer>().material;
            inList.material.SetColor("_EmissiveColor", InactiveColor);
            inList.LastPipeColor = inList.material.GetColor("_EmissiveColor");
        }

        RunBossWarningMessage();

    }

    public void RunAfterWarningMessage()
    {
        RunGenericBossTasks();
        currentCoroutine = BossInitialize();
        StartCoroutine(currentCoroutine);
        StartCoroutine(MinicannonFiring());
    }

    void InstantiateNodes(int i, GameObject [] dummyNodes, GameObject SourcePrefab, GameObject parentNode)
    {

        GameObject currentBody = Instantiate(SourcePrefab) as GameObject;
        currentBody.transform.parent = dummyNodes[i].transform.parent.transform;
        currentBody.transform.position = dummyNodes[i].transform.position;
        currentBody.transform.rotation = dummyNodes[i].transform.rotation;
        ActiveBodies.Add(currentBody);
        currentBody.name = (""+dummyNodes[0].name+""+i);
        starEnemy currentStarEnemy = currentBody.GetComponentInChildren<starEnemy>();
        currentStarEnemy.parent = currentBody;
        currentStarEnemy.AlertControllerOnDisable = parentNode;
        currentBody.GetComponentInChildren<EnemyHealth>().CollideParentToMessage = parentNode;
        // currentBody.GetComponentInChildren<EnemyHealth>().cantTakeDamage = true;
    }





    IEnumerator BossInitialize()
    {
        yield return new WaitForSeconds(InitialWaitTime);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        currentCoroutine = SmallDoorsPhase();
        StartCoroutine(currentCoroutine);

    }


    IEnumerator MinicannonFiring()
    {
        yield return new WaitForSeconds(MinicannonFireRate);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        if (currentMiniCannonFiring < activeMiniCannons.Count - 1)
        {
            currentMiniCannonFiring++;
        }

        else
        {
            currentMiniCannonFiring = 0;
        }

        if (activeMiniCannons.Count != 0)
        {
            activeMiniCannons[currentMiniCannonFiring].GetComponent<starEnemy>().FireProjectileNoParent();
            StartCoroutine(MinicannonFiring());
        }
    }




    IEnumerator SmallDoorsPhase()
    {

        // step 1


        for (int i = 0; i < ActiveBodies.Count; i++)
        {
             ActiveBodies[i].SetActive(true);
        }


        ChangeInnerMeshRenderStates(true);

        SmallDoorsChangeState(true);

        yield return new WaitForSeconds(NodesExposedDuration);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        SmallDoorsChangeState(false);
        currentCoroutine = Phase1CannonAttackA();
        StartCoroutine(currentCoroutine);
    }



    IEnumerator Phase1CannonAttackA()
    {
        /// step 2A

        starGameManagerRef.PlayAudio(audioSource, DoorsOpenClip);
        MainDoorsChangeState(true);
        BossAnimator.SetBool(CannonExtendTrgr, true);

        StartCoroutine(ChargePipes(PipeChargeGlobalDelay));

        yield return new WaitForSeconds(Phase1CannonAttackDurationSetup);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        currentCoroutine = Phase1CannonAttackB();
        StartCoroutine(currentCoroutine);

    }



    IEnumerator Phase1CannonAttackB()
    {
        /// step 2B
        starGameManagerRef.PlayAudio(audioSource, BulletsFireClip);
        foreach (GameObject picked in bulletSpreadSpawnPoints)
        {
            ProjectileFire(ProjectileA, picked);
        }

        yield return new WaitForSeconds(Phase1CannonAttackDurationShoot);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }
        starGameManagerRef.PlayAudio(audioSource, DoorsCloseClip);
        MainDoorsChangeState(false);
        BossAnimator.SetBool(CannonExtendTrgr, false);

        currentCoroutine = SmallDoorsPhase();
        StartCoroutine(currentCoroutine);
    }
    

    private void Update()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (starGameManagerRef != null)
        {
            if (starGameManagerRef.GamePaused)
            {
                return;
            }
        }

        if (!starGameManagerRef.BossWarningMessagePlaying && !RanGenericBossTasks)
        {
            RunAfterWarningMessage();
        }

        if (ChildCollisionAlert)
        {
            ChildCollisionAlert = false;
            BossDamageEvent.Invoke();
        }


        if (DisabledSubObjects.Count != 0)
        {

            foreach (GameObject picked in DisabledSubObjects)
            {
                if (activeMiniCannons.Contains(picked))
                {
                    activeMiniCannons.Remove(picked);
                }

                if (damageObjectLinks.ContainsKey(picked))
                {

                    damageSwapList currentDamageListItem = damageObjectLinks[picked];

                    GameObject[] objectsHide = currentDamageListItem.ObjectsHide;
                    GameObject[] objectsShow = currentDamageListItem.ObjectsShow;

                    for (int i = 0; i < objectsHide.Length; i++)
                    {
                        objectsHide[i].SetActive(false);
                    }

                    for (int i = 0; i < objectsShow.Length; i++)
                    {
                        objectsShow[i].SetActive(true);
                    }

                }

                ActiveBodiesUpdater(picked);
            }


        }


        if (bossPhase == BossPhase.BossPhase1 && AllActiveBodiesCleared)
        {
            bossPhase = BossPhase.BossPhase2;
            AllActiveBodiesCleared = false;

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            MainDoorsChangeState(true);
            BossAnimator.SetBool(CannonExtendTrgr, true);
            BossAnimator.SetTrigger(CannonSweepTrgr);
            laserActive = true;

            if (UserTargetReticleSpawnPointsSecondary.Length != 0)
            {
                SpawnReticles(UserTargetReticleSpawnPointsSecondary, DelayReticleSecondaryTimer);
            }

        }
    }
    

    private void MainDoorsChangeState (bool state)
    {
        OuterDoorsCollision.SetActive(!state);

        BossAnimator.SetBool(MainDoorsBool, state);

        if (MainHitbox.GetComponent<Collider>())
        {
            MainHitbox.GetComponent<Collider>().enabled = state;
        }
    }

    private void SmallDoorsChangeState (bool state)
    {

        BossAnimator.SetBool(SmallDoorsBool, state);
        
        for (int i = 0; i < activeMiniCannons.Count; i++)
        {
            activeMiniCannons[i].GetComponent<starEnemy>().ProjectileFiringEnabled = state;
        }

        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            ActiveBodies[i].GetComponent<Collider>().enabled = state;
        }

        if (UserTargetReticleSpawnPoints.Length != 0)
        {
            if (state == false)
            {
                AnimateReticles(ReticleShrink);
            }
            else
            {
                AnimateReticles(ReticleActivate);
            }
        }

    }


    public void OverridePhase1Cannon()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        SmallDoorsChangeState(false);
        MainDoorsChangeState(true);
        BossAnimator.SetBool(CannonExtendTrgr, true);
        currentCoroutine = Phase1CannonAttackA();
        StartCoroutine(currentCoroutine);
    }
}
