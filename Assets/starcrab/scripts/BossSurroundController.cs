using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSurroundController : BossControllerGeneric {


    // WORKING VERSION

    public enum SpinMode { Off, Normal, Split}
    public bool TempTestFire;
    public bool FlipAll;
    public bool SidewaysAll;
    public bool spinOn;
    public bool spinReset;
    public bool reverseSpin;

    public bool ToggleAxisTumble;
    bool tumbleWasToggled;
    bool tumbleOn;
    bool reverseTumble;

    bool tumbleToOrigin;
    bool tumbleToHalf;

    bool allAreFlipped;
    bool allAreSideways;

    public bool ToggleVerticalSplit;
    public bool didVerticalSplit;
    bool didDepthToggle;
    public bool didSideways;
    public bool ToggleBodyDepth;
    public bool Activate2ndReversed;
    bool canActivateNextBody = true;
    public float InitialActivateDelay = 2.0f;
    public float ActivateDelayBetweenBodies = 0.25f;
    public GameObject BodySource;
    public int BodyIterations;
    public float DegreeDistance = 20.0f;
    public GameObject SpinObject;
    public GameObject TumbleObject;

    public List<GameObject> body = new List<GameObject>();

    public Animator animator;

    public float SpinupRate = 0.1f;
    public float FlashFireDelay = 1.0f;
    public float SpinGroupSpeedAbove = 0.4f;
    public float SpinGroupSpeedBelow = 0.4f;
    private float usingSpinGroupSpeedBelow;
    public float TumbleGroupSpeed = 0.25f;
    public float incrementFlipDelay = 0.35f;
    private float currentSpinSpeedAbove;
    private float currentSpinSpeedBelow;

    public float TumbleTransferSpeed = 0.18f;
    public float VerticalSplitDistance = 0.5f;
    public float DistanceMoveSpeed = 1.0f;
    public float DistanceDepthMoveAbove = 1.0f;
    public float DistanceDepthMoveBelow = 2.0f;


    public List<GameObject> aboveBodies = new List<GameObject>();
    public List<GameObject> belowBodies = new List<GameObject>();
    public List<GameObject> ActivatingBodyList = new List<GameObject>();

    public int currentActiveBodyInt;
    int currentBodyActivatedCounter = 0;
    bool ranInitialProcesses = false;

    Transform rotateFrom;
    float timeCount = 0.0f;
    public float SpinSmoothingTransitionStarting = 2.0f;
    public float SpinSmoothingTransitionStopping = 4.0f;
    bool startedSlerp;
    bool isStillSpinningAbove;
    bool isStillSpinningBelow;
    bool isMidAxisTumble;
    bool allBodiesLoaded;
    float currentPlacementDegreeDistance;

    public bool FiringEnabled;
    public float FiringRate = 3.0f;
    public float FiringVariation = 1.0f;
    public bool NextFormationChange;
    public bool RandomFormationChange;
    int formationCounter;
    int maxAttackCounter = 10;
    public float DelayBetweenMultiFire = 0.15f;
    int activeBodyFiringCounter;
    bool fireNextNormal;
    bool normalFiringMode;
    public bool killRandombody;
    bool beganFirstSpin;
    public float TimerRevertTumble = 3.5f;
    public float TimerRevertTumbleJitter = 0.35f;
    float timerRevertTumble;
    bool runningTumbleTimer;
    public bool midProcedure;
    IEnumerator lastCoroutine;

    public float SpinOnResumeTimer = 4.0f;

    public GameObject FollowObjectOverride;
    public float FollowSpeed = 1.2f;
    public bool skipIntro;

    public float zLimitSplit = 0.117f;
    public float zLimitSideways = 0.096f;
    public float zLimitSidewaysSplit = 0.179f;
    public float MidProcedureWaitTime = 4.2f;

    public AudioClip BeamInClip;
    public AudioClip SpinFwdClip;
    public AudioClip SpinRevClip;
    public AudioClip EnemyShotClip;
    public AudioClip RotateAClip;
    public AudioClip RotateBClip;
    public AudioClip FlipOverClip;
    public AudioClip VerticalSeparateClip;
    public AudioClip VerticalRejoinClip;

    public AudioClip ReorientClip;
    public AudioClip GlobalReorientClip;
    public AudioClip PreShootClip;


    public Dictionary<GameObject, Quaternion> initRotationQuaternion = new Dictionary<GameObject, Quaternion>();
    public Dictionary<GameObject, float> depthDistanceAbove = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> depthDistanceBelow = new Dictionary<GameObject, float>();

    public Dictionary<GameObject, float> verticalDistanceAbove = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> verticalDistanceBelow = new Dictionary<GameObject, float>();

    public int enemyHealthReduceA = 20;
    public int enemyHealthReduceB = 10;

    public int enemyHealthReduceThresholdA = 4;
    public int enemyHealthReduceThresholdB = 2;

    private bool performedReductionA;
    private bool performedReductionB;

    [Header("NEW DEBUGS")]

    private bool RunF01;
    private bool RunF02, RunF03, RunF04, RunF05, RunF07, RunF08;
    public bool RunF06, RunF09, RunF10;

    /// <summary>
    /// for debugging when spinning went off-axis, calling these quickly helped test the issue
    /// </summary>
    public void EnableR6()
    {
        RunF06 = true;
    }

    public void EnableR9()
    {
        RunF09 = true;
    }

    public void EnableR10()
    {
        RunF10 = true;
    }


    void Start()
    {
        ResetBossPhase(true);
        audioSource = GetComponent<AudioSource>();
        RunBossWarningMessage();
    }


    void RunAfterWarningMessage()
    {
        StartCoroutine(BeginSurroundBossProcess());
        RunGenericBossTasks();
    }


    IEnumerator BeginSurroundBossProcess()
    {
        if (skipIntro)
        {
            InitialActivateDelay = 0;
        }

        yield return new WaitForSeconds(InitialActivateDelay);

        // MoveNearPlayer();
        gameObject.transform.position = FollowObjectOverride.transform.position;

        normalFiringMode = false;
        fireNextNormal = true;

        for (int i = 0; i < BodyIterations; i++)
        {
            GameObject currentBody = Instantiate(BodySource) as GameObject;
            currentBody.transform.parent = SpinObject.transform;
            currentBody.transform.localEulerAngles = new Vector3(-90, 0, currentPlacementDegreeDistance);
            currentBody.transform.localPosition = Vector3.zero;
            currentPlacementDegreeDistance = currentPlacementDegreeDistance + DegreeDistance;
            body.Add(currentBody);
            currentBody.name = "boss_surround_body_" + i;
            starEnemy currentStarEnemy = currentBody.GetComponentInChildren<starEnemy>();
            currentStarEnemy.parent = currentBody;
            currentStarEnemy.AlertControllerOnDisable = gameObject;
            currentBody.GetComponentInChildren<EnemyHealth>().cantTakeDamage = true;
        }


        foreach (GameObject picked in body)
        {
            ActiveBodies.Add(picked);
            initRotationQuaternion.Add(picked, picked.transform.rotation);
        }


        for (int i = 0; i < body.Count; i++)
        {
            if (i % 2 == 0)
            {
                aboveBodies.Add(body[i]);
                depthDistanceAbove.Add(body[i], body[i].transform.localPosition.x);
                verticalDistanceAbove.Add(body[i], body[i].transform.localPosition.y);
            }

            else

            {
                belowBodies.Add(body[i]);
                depthDistanceBelow.Add(body[i], body[i].transform.localPosition.x);
                verticalDistanceBelow.Add(body[i], body[i].transform.localPosition.y);
            }
        }

        for (int i = 0; i < aboveBodies.Count; i++)
        {
            ActivatingBodyList.Add(aboveBodies[i]);
        }


        if (!Activate2ndReversed)
        {
            for (int i = 0; i < belowBodies.Count; i++)
            {
                ActivatingBodyList.Add(belowBodies[i]);
            }
        }

        else

        {
            // activates belowBodies in the reverse order of assignment, nice effect
            for (int i = belowBodies.Count; i-- > 0;)

            {
                ActivatingBodyList.Add(belowBodies[i]);
            }
        }

       // MoveNearPlayer();


        if (skipIntro)
        {
            foreach(GameObject picked in body)
            {
                picked.SetActive(true);
            }
        }
        // check for final and set normalFiringActive = true;

    }


    void FlipAllBodies()
    {
        if (allAreFlipped)

        {
            for (int i = 0; i < ActiveBodies.Count; i++)
            {
                GameObject currentObject = ActiveBodies[i];
                currentObject.GetComponentInChildren<starEnemy>().ResetCurrentProjectileSpawnPointCounter();
            }
        }
        else
        {
            for (int i = 0; i < ActiveBodies.Count; i++)
            {
                GameObject currentObject = ActiveBodies[i];
                currentObject.GetComponentInChildren<starEnemy>().IncrementCurrentProjectileSpawnPointCounter();
            }
        }

        allAreFlipped = !allAreFlipped;
        allAreSideways = false;
        currentActiveBodyInt = 0;
        StartCoroutine(DoFlipAnim(false));
    }

    void SidewaysAllBodies()
    {
        allAreSideways = !allAreSideways;
        didSideways = !didSideways;
        currentActiveBodyInt = 0;
        StartCoroutine(DoFlipAnim(true));
    }



    IEnumerator DoFlipAnim(bool sideways)
    {
        yield return new WaitForSeconds(incrementFlipDelay);
        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }

        GameObject currentActiveBody = ActiveBodies[currentActiveBodyInt];

        Animator currentAnimator = currentActiveBody.GetComponent<Animator>();
        starEnemy currentStarEnemy = currentActiveBody.GetComponentInChildren<starEnemy>();


        // IMPORTANT - do a check to make sure object is active, or it will crash
        // rma0109

        if (ActiveBodies[currentActiveBodyInt].activeSelf)
        {
            if (sideways)
            {

                if (allAreSideways)
                {
                    currentAnimator.SetTrigger("flipSideways");
                }

                else
                {
                    currentAnimator.SetTrigger("flip");
                }
            }

            else
            {
                currentAnimator.SetBool("orientReverse", allAreFlipped);
                currentAnimator.SetTrigger("flip");
            }

              currentActiveBody.GetComponentInChildren<AudioSource>().PlayOneShot(ReorientClip);

        }

        currentActiveBodyInt++;  //should this be in above bracket?
        // should I do a range check and return if it's above ActiveBodies.count?

        if (currentActiveBodyInt < ActiveBodies.Count)
        {
            StartCoroutine(DoFlipAnim(sideways));
        }
    }

    void BeginRandomCannonProcess()
    {
         BeginGeneralCannonProcess(ActiveBodies[Random.Range(0, ActiveBodies.Count)]);
    }

    void BeginGeneralCannonProcess(GameObject currentObject)
    {
        Animator animator = null;

        if (currentObject.GetComponent<Animator>())
        {
            animator = currentObject.GetComponent<Animator>();
        }

        if (animator != null)
        {
            animator.SetTrigger("cannonFire");
            StartCoroutine(DoFire(animator));
        }

        currentObject.GetComponentInChildren<AudioSource>().PlayOneShot(PreShootClip);
    }


    IEnumerator DoFire(Animator animator)
    {
        yield return new WaitForSeconds(FlashFireDelay);
        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }
        animator.gameObject.GetComponentInChildren<starEnemy>().FireProjectileNoParent();
    }


    void SpinResetter()
    {
        
        for (int i = 0; i < body.Count; i++)
        {
            body[i].transform.rotation = Quaternion.Lerp(body[i].transform.rotation, initRotationQuaternion[body[i]],
                Time.deltaTime * SpinSmoothingTransitionStopping);
        }
        
        float angle = Quaternion.Angle(body[0].transform.rotation, initRotationQuaternion[body[0]]);

        
        if (angle < 0.01f) // 0.168
        {
            spinReset = false;
            StartCoroutine(ResumeSpinning());
        }


    }


    IEnumerator ResumeSpinning()
    {
        yield return new WaitForSeconds(SpinOnResumeTimer);
        spinOn = true;
    }

    /*
    void SpinAudioActivate()
    {
        print("RUNNING SPIN AUDIO ACTIVATE");

        if (reverseSpin)
        {
            starGameManagerRef.PlayAudio(audioSource, SpinRevClip);
        }

        else

        {
            starGameManagerRef.PlayAudio(audioSource, SpinFwdClip);
        }

        audioSource.loop = true;

    }
    */

    void SpinAudioActivate()
    {
        starGameManagerRef.PlayAudio(audioSource, SpinFwdClip);
        audioSource.loop = true;
    }





    void SpinUpdate(float spinGoalSpeedAbove, float spinGoalSpeedBelow, float spinSmoothingTransition )

        {
            currentSpinSpeedAbove = Mathf.Lerp(currentSpinSpeedAbove, spinGoalSpeedAbove, Time.deltaTime * spinSmoothingTransition);
            currentSpinSpeedBelow = Mathf.Lerp(currentSpinSpeedBelow, spinGoalSpeedBelow, Time.deltaTime * spinSmoothingTransition);

        // SpinObject.transform.Rotate(0, currentSpinSpeedAbove, 0);


        Vector3 directionVector = Vector3.up;
        if (tumbleWasToggled)
        {
            //   directionVector = Vector3.right;
            //  directionVector = Vector3.left;
            directionVector = Vector3.forward;

        }


        for (int i = 0; i < aboveBodies.Count; i++)
        {
            //    aboveBodies[i].transform.RotateAround(SpinObject.transform.position, directionVector, currentSpinSpeedAbove);
             aboveBodies[i].transform.Rotate(0,0, currentSpinSpeedAbove);


        }


        for (int i = 0; i < belowBodies.Count; i++)

        {
            //  belowBodies[i].transform.RotateAround(SpinObject.transform.position, directionVector, currentSpinSpeedBelow);
            belowBodies[i].transform.Rotate(0, 0, currentSpinSpeedBelow);


        }

    }

    void DepthTumbling()
    {

        float endPosAbove = DistanceDepthMoveAbove;
        float endPosBelow = DistanceDepthMoveBelow;


        if (didDepthToggle)
        {
            // endPosAbove = 0;
            // endPosBelow = 0;

            endPosAbove = endPosAbove * -1;
            endPosBelow = endPosBelow * -1;

        }



        for (int i = 0; i < aboveBodies.Count; i++)

        {
            GameObject currentObject = aboveBodies[i];

            float newPos = Mathf.Lerp(0.0f, endPosAbove, Time.deltaTime * DistanceMoveSpeed);
            
            Vector3 newVector = new Vector3(newPos, verticalDistanceAbove[aboveBodies[i]], 0);
            //  Vector3 combinedVector = new Vector3(0, newPos, 0);
            Vector3 combinedVector = new Vector3(verticalDistanceAbove[aboveBodies[i]], newPos, 0);

            currentObject.transform.Translate(combinedVector, Space.Self);  
            
        }

        for (int i = 0; i < belowBodies.Count; i++)

        {
            GameObject currentObject = belowBodies[i];

            float newPos = Mathf.Lerp(0.0f, endPosBelow, Time.deltaTime * DistanceMoveSpeed);

            Vector3 newVector = new Vector3(newPos, verticalDistanceBelow[belowBodies[i]], 0);
            //  Vector3 combinedVector = new Vector3(0, newPos, 0);
            Vector3 combinedVector = new Vector3(verticalDistanceBelow[belowBodies[i]], newPos, 0);

            currentObject.transform.Translate(combinedVector, Space.Self);

        }


        /*

        // moving in the gemeral proper manner, just need to find a way to multiply it properly
        for (int i = 0; i < aboveBodies.Count; i++)
        {
            GameObject currentObject = aboveBodies[i];
            float newPos = Mathf.Lerp(currentObject.transform.localPosition.x, endPosd, Time.deltaTime * DistanceMoveSpeed);
            //   currentObject.transform.localPosition = new Vector3(newPos, verticalDistanceAbove[aboveBodies[i]], 0);
            Vector3 newVector = new Vector3(newPos, verticalDistanceAbove[aboveBodies[i]], 0);
            //  currentObject.transform.Translate(newVector, Space.Self);
            currentObject.transform.Translate(Vector3.up, Space.Self);
        }

        */




        /*
        for (int i = 0; i < aboveBodies.Count; i++)
        {
            GameObject currentObject = aboveBodies[i];
            float newPos = Mathf.Lerp(currentObject.transform.localPosition.x, endPosd, Time.deltaTime * DistanceMoveSpeed);
            //   currentObject.transform.localPosition = new Vector3(newPos, verticalDistanceAbove[aboveBodies[i]], 0);
            Vector3 newVector = new Vector3(newPos, verticalDistanceAbove[aboveBodies[i]], 0);
            currentObject.transform.Translate (newVector, Space.Self);

        }

        
        for (int i = 0; i < belowBodies.Count; i++)
        {
            GameObject currentObject = belowBodies[i];
            float newPos = Mathf.Lerp(currentObject.transform.localPosition.x, endPosd * -1, Time.deltaTime * DistanceMoveSpeed);
            // currentObject.transform.localPosition = new Vector3(newPos, verticalDistanceBelow[belowBodies[i]], 0);
            Vector3 newVector = new Vector3(newPos, verticalDistanceBelow[belowBodies[i]], 0);
            currentObject.transform.Translate(newVector, Space.Self);
        }
        */





        Vector3 targetPosition = new Vector3(0, endPosAbove, 0);
        Vector3 currentPosition = aboveBodies[0].transform.localPosition;
        //  float rangeCheck = 0.1f;  // last good
        float rangeCheck = 0.05f;

       // Vector3 zeroPosition = new Vector3(0,0,0);


        if (didDepthToggle)
        {
         //   rangeCheck = rangeCheck * -1;
        }


        if (Vector3.Distance(targetPosition, currentPosition) > rangeCheck)


            /*

            if ((!didDepthToggle && Vector3.Distance (targetPosition, currentPosition) > rangeCheck) ||

                (didDepthToggle && Vector3.Distance(targetPosition, currentPosition) > rangeCheck)

                )

        */

            if ((Vector3.Distance(targetPosition, currentPosition) > rangeCheck) ||

                (currentPosition.z <= 0.01f))

                /// grab Z at beginning of move, and then don't go less than that on return?


            {
            RefreshTransformDictionaries(depthDistanceAbove, aboveBodies, false);
            RefreshTransformDictionaries(depthDistanceBelow, belowBodies, false);
            ToggleBodyDepth = false;
            didDepthToggle = !didDepthToggle;
        }
        
    }



    void VerticalSplitting()
    {

        float endPosd = VerticalSplitDistance;
        
        if (didVerticalSplit)
        {
            endPosd = 0;
        }

        for (int i = 0; i < aboveBodies.Count; i++)
        {
            GameObject currentObject = aboveBodies[i];
            float newPos = Mathf.Lerp(currentObject.transform.localPosition.y, endPosd, Time.deltaTime * DistanceMoveSpeed);
            currentObject.transform.localPosition = new Vector3(depthDistanceAbove[aboveBodies[i]], newPos, 0);
        }

        for (int i = 0; i < belowBodies.Count; i++)
        {
            GameObject currentObject = belowBodies[i];
            float newPos = Mathf.Lerp(currentObject.transform.localPosition.y, endPosd * -1, Time.deltaTime * DistanceMoveSpeed);
            currentObject.transform.localPosition = new Vector3(depthDistanceBelow[belowBodies[i]], newPos, 0);
        }


        float rangeCheck = 0.0001f;
        float checkValue = aboveBodies[0].transform.localPosition.y;

        float upperLimit = endPosd - rangeCheck;
        float lowerLimit = endPosd + rangeCheck;



        if (((checkValue >= upperLimit) && (checkValue <= lowerLimit)))

        {
            RefreshTransformDictionaries(verticalDistanceAbove, aboveBodies, true);
            RefreshTransformDictionaries(verticalDistanceBelow, belowBodies, true);
            ToggleVerticalSplit = false;
            didVerticalSplit = !didVerticalSplit;
            midProcedure = false;
        }

    }

    void RefreshTransformDictionaries(Dictionary<GameObject, float> dictionary, List<GameObject> list, bool useVertical)
    {
        dictionary.Clear();
        foreach (GameObject picked in list)
        {
            if (useVertical)
            {
                dictionary.Add(picked, picked.transform.localPosition.y);
            }

            else

            {
                dictionary.Add(picked, picked.transform.localPosition.x);
            }
        }
    }

    void ResetRunBools()
    {
        RunF01 = RunF02 = RunF03 = RunF04 = RunF05 = RunF06 = RunF07 = RunF08 = RunF09 = RunF10 = false;
    }

    void RunDebugBoolChecks()

    {

        if (RunF01)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation01();
            }
            ResetRunBools();
        }

        if (RunF02)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation02();
            }
            ResetRunBools();
        }

        if (RunF03)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation03();
            }
            ResetRunBools();
        }
        if (RunF04)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation04();
            }
            ResetRunBools();
        }

        if (RunF05)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation05();
            }
            ResetRunBools();
        }

        if (RunF06)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation06();
            }
            ResetRunBools();
        }

        if (RunF07)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation07();
            }
            ResetRunBools();
        }


        if (RunF08)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation08();
            }
            ResetRunBools();
        }

        if (RunF09)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation09();
            }
            ResetRunBools();
        }

        if (RunF10)
        {
            if (!midProcedure)
            {
                BeginMidProcedureFailsafe();
                Formation10();
            }
            ResetRunBools();
        }


        // ----------- end runBool testing
    }


    void Update() {

        RunDebugBoolChecks();

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


        if ((bossPhase != BossPhase.BossPhase1) && ActiveBodies.Count == 0)
        {
           // print("boss beaten in surround controller");
            DoBossDefeated(false);
        }


        if (bossPhase == BossPhase.BossPhase2)
        {
            if (spinOn)
            {
                if (!audioSource.isPlaying)
                {
                    SpinAudioActivate();
                }
            }

            else

            {
                audioSource.loop = false;
            }
        }


        if (FollowObjectOverride == null)
        {

            if (starGameManagerRef.HeroShip != null)
            {
                FollowObjectOverride = starGameManagerRef.HeroShip;
            }
            
        }
        
        if (TempTestFire)
        {
            TempTestFire = false;
            if (ActiveBodies.Count > 0)
            {
                BeginRandomCannonProcess();
            }
        }

        if (FlipAll)
        {
            FlipAll = false;
            FlipAllBodies();
        }

        if (SidewaysAll)
        {
            SidewaysAll = false;
            SidewaysAllBodies();
        }


        if (reverseSpin)
        {
            SpinGroupSpeedAbove = SpinGroupSpeedAbove * -1;
            SpinGroupSpeedBelow = SpinGroupSpeedBelow * -1;

            reverseSpin = false;
        }

        if (reverseTumble)
        {
            TumbleGroupSpeed = TumbleGroupSpeed * -1;
            reverseTumble = false;
        }

        if (spinReset)
        {

            if (!tumbleWasToggled)
            {
                spinOn = false;
                SpinResetter();
            }

            else
            {
                spinReset = false;
            }
        }



        if (ToggleVerticalSplit)
        {
            VerticalSplitting();
        }

        if (ToggleBodyDepth)
        {
            DepthTumbling();
        }



        if (ToggleAxisTumble)
        {

            if (!tumbleWasToggled)
            {
                tumbleToHalf = true;
            }

            else

            {
                tumbleToOrigin = true;
            }

            ToggleAxisTumble = false;
            tumbleWasToggled = !tumbleWasToggled;

            audioSource.Stop();
            audioSource.PlayOneShot(GlobalReorientClip);
        }

        if (spinOn && !isMidAxisTumble)
        {

            if (SpinObject != null)
            {
                if (didVerticalSplit)

                {
                    usingSpinGroupSpeedBelow = SpinGroupSpeedBelow;
                }

                else

                {
                    usingSpinGroupSpeedBelow = SpinGroupSpeedAbove;
                }


                isStillSpinningAbove = true;
                SpinUpdate(SpinGroupSpeedAbove, usingSpinGroupSpeedBelow, SpinSmoothingTransitionStarting);
            }
        }

        else

        {

            if (isStillSpinningAbove && !isMidAxisTumble)

                {
                    SpinUpdate(0.0f, 0.0f, SpinSmoothingTransitionStopping);

                    if (currentSpinSpeedAbove == 0)
                    {
                        isStillSpinningAbove = false;
                    }
                }
        }

        
        
        if (!tumbleToOrigin && !tumbleToHalf)
        {
            if (tumbleOn)
            {
                if (TumbleObject != null)
                {
                    TumbleObject.transform.Rotate(TumbleGroupSpeed, 0, 0);
                }
            }
        }

        if (tumbleToOrigin || tumbleToHalf)

        {

            tumbleOn = false;
            isMidAxisTumble = true;

            if (!startedSlerp)
            {
                startedSlerp = true;
                rotateFrom = TumbleObject.transform;
            }

            float goalFloat = 90.0f;

            if (tumbleToOrigin)
            {
                goalFloat = 0.0f;
            }



            Quaternion goalQuaternion = Quaternion.Euler(goalFloat, 0, 0);

            TumbleObject.transform.rotation = Quaternion.Slerp(rotateFrom.rotation, goalQuaternion, timeCount * TumbleTransferSpeed);

            timeCount = timeCount + Time.deltaTime;
            
            float checkingX = TumbleObject.transform.eulerAngles.x;

            if (checkingX == goalFloat)
                
            {
                startedSlerp = false;
                tumbleToHalf = false;
                tumbleToOrigin = false;
                timeCount = 0.0f;
                isMidAxisTumble = false;
            }

        }
        
            if (NextFormationChange)
            {
                IncrementAttackFormationCounter();
                NextFormationChange = false;
                SwitchFormations();
            }


            if (RandomFormationChange)
            {
                RandomAttackFormationSelect();
                RandomFormationChange = false;
                //  NextFormationChange = false;
                SwitchFormations();
            }



        if (currentBodyActivatedCounter < ActivatingBodyList.Count)
        {
            if (canActivateNextBody)
            {
                StartCoroutine(ActivatingBodies(ActivatingBodyList[currentBodyActivatedCounter]));
                currentBodyActivatedCounter++;
            }
        }

        if (currentBodyActivatedCounter == BodyIterations && !ranInitialProcesses)
        {
            ranInitialProcesses = true;
            for (int i = 0; i < ActiveBodies.Count; i++)
            {
                ActiveBodies[i].GetComponentInChildren<EnemyHealth>().cantTakeDamage = false;
            }
            spinOn = true;
        }



        // waits until all surroundBodies have been initialized so that they will all be activated
        // before running deactivated/remove from active list check


        if (!allBodiesLoaded)
        {

            if (ActivatingBodyList.Count == BodyIterations)

                {
                      if (ActivatingBodyList[BodyIterations - 1].activeSelf)
                    {
                        allBodiesLoaded = true;
                        normalFiringMode = true;
                        bossPhase = BossPhase.BossPhase2;
                    // print("enter boss phase 2");  // this happens way too early
                        SpinAudioActivate();
                    }
                }
        }


        // removes from activeList if they have been disabled. If this gives trouble,
        // add into a new temp list and delete list contents after running thru orig list
        // referring to Generic Class, other subClasses should operate like this



        // ------------------------------------------------------------------------


        if (allBodiesLoaded)
        {
            if (ActiveBodies.Count <= enemyHealthReduceThresholdA && !performedReductionA)
            {
                performedReductionA = true;
                foreach (GameObject picked in ActiveBodies)
                {
                    EnemyHealth currentEnemyHealth = picked.GetComponentInChildren<EnemyHealth>();
                    if (currentEnemyHealth.health > enemyHealthReduceA)
                    {
                        currentEnemyHealth.health = enemyHealthReduceA;
                    }
                }
            }





            if (ActiveBodies.Count <= enemyHealthReduceThresholdB && !performedReductionB)
            {
                performedReductionB = true;
                foreach (GameObject picked in ActiveBodies)
                {
                    EnemyHealth currentEnemyHealth = picked.GetComponentInChildren<EnemyHealth>();
                    if (currentEnemyHealth.health > enemyHealthReduceB)
                    {
                        currentEnemyHealth.health = enemyHealthReduceB;
                    }
                }
            }
        }
        
        // ------------------------------------------------------------------------




        

        if (DisabledSubObjects.Count != 0)

            {

                foreach (GameObject picked in DisabledSubObjects)
                {
                    ActiveBodiesUpdater(picked);
                }

                if (ActiveBodyBeingCleared && ActiveBodies.Count != 0)
                    {
                        if (!beganFirstSpin)
                        {
                            beganFirstSpin = true;
                            NextFormationChange = true;
                        }
                        else
                        {
                            RandomFormationChange = true;
                        }
                    }
            }


            if (normalFiringMode && fireNextNormal)
        {
            StartCoroutine(NormalFiringCounter());
        }

        if (killRandombody)
        {
            if (!midProcedure)
            {
                GameObject pickedBody = ActiveBodies[Random.Range(0, ActiveBodies.Count)];
                pickedBody.SetActive(false);
            }

            killRandombody = false;
        }

        // following

        if (FollowObjectOverride != null)
        {

            if (normalFiringMode)
            {
                MoveNearPlayer();
            }

        }
    }
    

    public void MoveNearPlayer()
    {


        /*

    transform.position = Vector3.Lerp(transform.position,
        FollowObjectOverride.transform.position, Time.deltaTime * FollowSpeed);

      */



        //  /*


        Vector3 goalPosition = FollowObjectOverride.transform.position;

/*
        Vector3 newPosition = Vector3.Lerp(transform.position,
            FollowObjectOverride.transform.position, Time.deltaTime * FollowSpeed);
            */


        float newPosX = goalPosition.x;
        float newPosY = goalPosition.y;
        float newPosZ = goalPosition.z;

        float zLimitCheck = 0.0f;

        if (didVerticalSplit & didSideways)
        {
        //   print("zLimitSidewaysSplit");
            zLimitCheck = zLimitSidewaysSplit;
        }

        if (!didVerticalSplit & didSideways)
        {
          //  print("zLimitSideways");

            zLimitCheck = zLimitSideways;
        }

        if (didVerticalSplit & !didSideways)
        {
          //  print("zLimitSplit");

            zLimitCheck = zLimitSplit;
        }


        float checkPosition = transform.localPosition.z;


        if (checkPosition < zLimitCheck)
        {
            //  print("reassign Z");
            //   newPosY = zLimitCheck;
            newPosY = newPosY + zLimitCheck;

        }


        Vector3 calculatedGoalPosition = new Vector3(newPosX, newPosY, newPosZ);


        transform.position = Vector3.Lerp(transform.position,
            calculatedGoalPosition, Time.deltaTime * FollowSpeed);

       // transform.position = new Vector3(newPosX, newPosY, newPosZ);


   // */

    }



    IEnumerator NormalFiringCounter()
    {
        fireNextNormal = false;
        float firingRate = FiringRate + (Random.Range(0, FiringVariation));
        yield return new WaitForSeconds(firingRate);

        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }

        TempTestFire = true;
        fireNextNormal = true;
    }



    IEnumerator ActivatingBodies(GameObject currentObject)
    {
        canActivateNextBody = false;
        yield return new WaitForSeconds(ActivateDelayBetweenBodies);
        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }
        currentObject.SetActive(true);
        canActivateNextBody = true;
        starGameManagerRef.PlayAudio(audioSource, BeamInClip);

    }



    void BeginMidProcedureFailsafe()
    {
        midProcedure = true;
        if (lastCoroutine == null)
        {
            lastCoroutine = MidProcedureFailsafeTimer();
            StartCoroutine(lastCoroutine);
        }

        else
        {
            StopCoroutine(lastCoroutine);
            lastCoroutine = null;
            BeginMidProcedureFailsafe();
        }


    }

    IEnumerator MidProcedureFailsafeTimer()
    {
        yield return new WaitForSeconds(MidProcedureWaitTime);
        midProcedure = false;
        lastCoroutine = null;
    }


    void SwitchFormations()
    {

        if (midProcedure)
        {
            return;
        }

        // midProcedure = true;
        BeginMidProcedureFailsafe();

        switch (formationCounter)
        {
            case 1:
                Formation01();
                break;

            case 2:
                Formation02();
                break;

            case 3:
                Formation03();
                break;

            case 4:
                Formation04();
                break;

            case 5:
                Formation05();
                break;

            case 6:
                Formation06();
                break;

            case 7:
                Formation07();
                break;

            case 8:
                Formation08();
                break;

            case 9:
                Formation09();
                break;

            case 10:
                Formation10();
                break;

            default:
                break;
        }
        
    }


    void IncrementAttackFormationCounter()
    {
        formationCounter++;
        if (formationCounter > maxAttackCounter)
        {
            formationCounter = 0;
        }
    }

    void RandomAttackFormationSelect()
    {
        formationCounter = Random.Range(0, maxAttackCounter); 
    }

    void Formation01()
    {
     //   print("FORM 1");

        if (!spinOn)
        {
            spinOn = true;
        }

        Formation06();
    }

    void Formation02()
    {
        if (!runningTumbleTimer)
        {
      //      print("FORM 2");

            StartCoroutine(RunTumbleTimer());
        }

        else
        {
            RandomFormationChange = true;
        }
    }

    void Formation03()
    {

        if (spinOn)
        {
      //      print("FORM 3");
            reverseSpin = true;
            midProcedure = false;
        }

        else

        {
            RandomFormationChange = true;

        }
    }

   public void Formation04()
    {

        // fire all at once

      //    print("FORM 4");


        normalFiringMode = false;

        for (int i = 0; i < ActiveBodies.Count; i++)
        {
            BeginGeneralCannonProcess(ActiveBodies[i]);
            if (i == ActiveBodies.Count-1)
            {
                normalFiringMode = true;
                midProcedure = false;
            }
        }
    }


    IEnumerator RunTumbleTimer()
    {
        runningTumbleTimer = true;
        ToggleAxisTumble = true;
        timerRevertTumble = TimerRevertTumble + Random.Range(0, TimerRevertTumbleJitter);
        yield return new WaitForSeconds(timerRevertTumble);
        ToggleAxisTumble = true;
        runningTumbleTimer = false;
        midProcedure = false;
    }

 
    public void Formation05()
    {
     //   print("FORM 5");

        // fire all in body order
        StartCoroutine(FireConsecutive(ActiveBodies[activeBodyFiringCounter])); // SOME PROBLEM HERE OUT OF RANGE
    }

    IEnumerator FireConsecutive(GameObject currentObject)
    {
        normalFiringMode = false;

        yield return new WaitForSeconds(DelayBetweenMultiFire);
        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }
        BeginGeneralCannonProcess(currentObject);
        activeBodyFiringCounter++;
        if (activeBodyFiringCounter < ActiveBodies.Count)
        {
            StartCoroutine (FireConsecutive(ActiveBodies[activeBodyFiringCounter]));
        }

        else

        {
            activeBodyFiringCounter = 0;
            normalFiringMode = true;
            midProcedure = false;
        }
    }




   public  void Formation06()
    {
        // vertical split


      //  print("FORM 6");

        if (didVerticalSplit)
        {
            spinReset = true;
            if (usingSpinGroupSpeedBelow < 0)
            {
                SpinGroupSpeedBelow = SpinGroupSpeedBelow * -1;
            }
            audioSource.PlayOneShot(VerticalRejoinClip);
        }

        else
        {
            audioSource.PlayOneShot(VerticalSeparateClip);
        }

        // might need to change this to wait
        ToggleVerticalSplit = true;
     //  midProcedure = false;  // not sure if this catches
    }

    void Formation07()
    {
        // sideways flip

     //     print("FORM 7");

        SidewaysAll = true;
        midProcedure = false;
    }


    void Formation08()
    {
     //   print("FORM 8");

        reverseSpin = true;
        // flip all
        FlipAll = true;
        midProcedure = false;
    }

   public void Formation09()
    {
        // limited burst, some value with some limit
        if (didVerticalSplit)
        {
           // print("FORM 9 - reverse bottom");
            SpinGroupSpeedBelow = SpinGroupSpeedBelow * -1;

        }

        else
        {
            Formation02();
        }

    }

    void Formation10()
    {
        // etc

       // print("FORM 10");


        reverseSpin = true;

        Formation02();
    }

}
