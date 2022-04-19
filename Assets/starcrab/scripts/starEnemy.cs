using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class starEnemy : MonoBehaviour {
    public enum CheckTerminator { None, ActorsParent, Father, GrandfatherLocal, GrandfatherWorld }
    public enum PowerupType { None, LevelUp, OneUp, Bomb, Starman, Random }

    public float ShadowBaseSize;
    public float MinShadowSize = 0.01f;
    public float MaxShadowSize = 2f;


    //[HideInInspector]
    public string Extra = "";
    public string description = "";
    public GameObject parent;
    public GameObject AlertControllerOnDisable;
    public GameObject projectile;
	public GameObject[] projectileSpawnPoint;
    public GameObject currentProjectileSpawnPoint;  // make private


  //  [HideInInspector]
    public int currentProjectileSpawnPointCounter;
    public bool SimultaneousSpawns;
    public bool AutoIterateSpawnPoints;

    bool usingSingleProjSpawnPoint;
    public bool ProjectileFiringEnabled = true;
    public ShotDirection ShotDirection;
    public Vector3 SpawnOrientation;
    public string[] SpawnAnimTriggers;
    public UnityEvent ProjectileEvent;
    public string animPattern = "";
    public Animator animator;
  //  public bool KeepAnimatorStateOnDisable;  // not implemented in this version of unity
    // public int collisionDamage;
    public int pointValue;
    public int projectileStrength;
    public float fireRate;
    public bool leader;
    public bool onlyLeaderShoots;
    public int followers;
    public List<GameObject> followersList;
    public float followerDistanceX, followerDistanceY, followerDistanceZ;
    public float delayFollowersSpawn;
    public GameObject leaderObject;
    public GameObject deathPrefab;
    //public Vector3 DeathPrefabOffset;
    public GameObject DeathPrefabSpawnpoint;
    public bool canDieOnContact = true;
    public GameObject DropOnDeath;
    public float DropScale = 0.025f;
    public PowerupType powerupType;
    public bool IsObstacle;
    private GameObject parentCheck;
    public float CurrentDistVector;
    public float TerminatorDistance = 0.52f;
    public CheckTerminator checkTerminator;
    public bool NotifyOnly;
    private bool localPauseFlag;
    private bool disableSpecialEffectOnDeath;
    private Vector3 leaderBeginPos;
    public bool ZeroPosOnDeath;
    
    StarGameManager starGameManagerRef;
    bool deactivated;

    float useFollowDistanceX, useFollowDistanceY, useFollowDistanceZ;
    Transform newParent;

    [HideInInspector] public GameObject grandParent;

    public float MoveSpeed = 1.0f;
    private float workingSpeed;

    public enum Homing { Off, WithRaycast, NoRaycast}
    public Homing homing;
    public string[] IgnoreColliderTag;
    public string ResetKineTag = "";
    public string BoundaryTag = "";

    public GameObject UpArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;
    public GameObject RightArrow;

    private AudioSource audioSource;
    public AudioClip ProjectileAudio;
    public AudioClip AudioOnStart;
    // public AudioClip CollisionAudio;


    public bool isBeingDrawn;
    public bool hasActivatedRenderers;
    public bool IgnoreRenderList;
    public bool SubscribeToAnimPauseList = true;
    [HideInInspector]
    public bool forceShadowActiveAtStart;

    [HideInInspector]
    public List<Renderer> meshRenderers = new List<Renderer>();

    List<Animator> pauseAnimList;

    void SetHomingModeNoRaycast()
    {
        homing = Homing.NoRaycast;
    }

    void SetHomingModeWithRaycast()
    {
        homing = Homing.WithRaycast;
    }

    void SetHomingModeOff()
    {
        homing = Homing.Off;
    }

    void ToggleOffscreenSettings(bool setOnscreen)
    {
       
        foreach (Renderer picked in meshRenderers)
        {
            picked.enabled = setOnscreen;
        }

        if (shadowObject != null)
        {
            shadowObject.SetActive(setOnscreen);
        }

      if (!setOnscreen)
        {
            if (NotifyOnly)
            {
                print(gameObject.name + " is out of draw area");
            }
            else
            {
                transform.localPosition = new Vector3(0, 0, 0);
                disableSpecialEffectOnDeath = true;
                parent.SetActive(false);
            }
        }
      else
        {
            BeginShootingCounter();
        }
    }


    void DefineRenderList()
    {
        if (IgnoreRenderList)
        {
            return;
        }

        if (meshRenderers.Count == 0)
        {
            if (gameObject.GetComponent<Renderer>())
            {
                if (gameObject.GetComponent<Renderer>().enabled)
                {
                    meshRenderers.Add(gameObject.GetComponent<Renderer>());
                }
            }

            foreach (Transform child in gameObject.transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        meshRenderers.Add(child.GetComponent<Renderer>());
                    }
                }
            }
        }
    }



    void Start()

    {

        StarGameManagerRefChecker();

        /*
        if (checkTerminator == CheckTerminator.None)
        {
            IgnoreRenderList = true;
        }
        */

        rbody = GetComponent<Rigidbody>();

        workingToVel = toVel;
        workingMaxVel = maxVel;
        /*
        if (KeepAnimatorStateOnDisable)
        {
            animator.GetComponent<Animator>().keepAnimatorControllerStateOnDisable;
        }
        */
        sensorSet = new GameObject[] {AboveSensor, BelowSensor, LeftSensor, RightSensor};

        if (parentCheck == null && checkTerminator != CheckTerminator.None)
        {

            if (checkTerminator == CheckTerminator.GrandfatherLocal ||
                checkTerminator == CheckTerminator.GrandfatherWorld)
            {
                // parentCheck = gameObject.transform.parent.transform.parent.gameObject;//   adjust this rma916
                parentCheck = parent.transform.parent.gameObject;
               // print(gameObject.name + " checking against " + parentCheck);
            }

            if (checkTerminator == CheckTerminator.Father)

            {
                //  parentCheck = gameObject.transform.parent.gameObject;  // adjust this rma916
                parentCheck = parent;
            }

            if (checkTerminator == CheckTerminator.ActorsParent)

            {
                parentCheck = starGameManagerRef.ActorsParent;
            }



        }

        if (audioSource == null)
        {
            if (gameObject.GetComponent<AudioSource>())
            {
                audioSource = (gameObject.GetComponent<AudioSource>());
            }
        }

        DefineRenderList();

        DoOnRestart();

        if (projectileSpawnPoint.Length == 0 ||
            (projectileSpawnPoint.Length == 1 && projectileSpawnPoint[0] == null))
        {
            usingSingleProjSpawnPoint = true;
            currentProjectileSpawnPoint = this.gameObject;
        }

        if (projectileSpawnPoint.Length == 1 && projectileSpawnPoint[0] != null)
        {
            usingSingleProjSpawnPoint = true;
            currentProjectileSpawnPoint = projectileSpawnPoint[0];
        }

        if (currentProjectileSpawnPoint == null)
        {
            currentProjectileSpawnPoint = projectileSpawnPoint[currentProjectileSpawnPointCounter];  // not sure - PROBABLY GOOD
        }
       
    }

    public float toVel = 2.5f;
    public float maxVel = 15.0f;
    public float maxForce = 40.0f;
    public float gain = 5f;
    Rigidbody rbody;
    // Transform targetPos;

 //   public RaycastDirectionChecker RaycastSensor;
 // blah

    
    public float homingTargetOffsetX;
    public float homingTargetOffsetY;
    public float homingTargetOffsetZ;
    public float OffsetIncrements;
    public bool collidingWGeo;  // mkPrivate
    public bool aboveBlocked;
    public bool straightBlocked;
    public bool belowBlocked;

    public bool leftBlocked;
    public bool rightBlocked;

    public bool FlipBlockedSides;

    public float workingToVel;
    public float workingMaxVel;

    public GameObject StraightSensor, AboveSensor, BelowSensor, LeftSensor, RightSensor;
    GameObject[] sensorSet = new GameObject[4];
    public string[] CheckTag;
    public float RaycastDistanceH, RaycastDistanceV, RaycastDistanceS;
    public bool ContinuousRaycast;

    public bool PowerUp;
    public UnityEvent PowerUpFx;
    public bool LifeUp;
    public UnityEvent LifeUpFx;
    private GameObject shadowObject;


    private void OnCollisionEnter(Collision collision)
    {
     
        foreach (string picked in IgnoreColliderTag)
        {
            if (collision.gameObject.tag == picked)
            {
              //  print("hit "+picked);
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            }
        }

        if (collision.gameObject.tag == ResetKineTag || collision.gameObject.tag == BoundaryTag)
        {
            rbody.Sleep();
        }
        
    }

    private void OnCollisionStay (Collision collision)
    {
        if (collision.gameObject.tag == ResetKineTag)

        {
            collidingWGeo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collidingWGeo == true)
        {
            collidingWGeo = false;
        }
    }
    
    public void IncrementCurrentProjectileSpawnPointCounter()
    {
        currentProjectileSpawnPointCounter++;
        if (currentProjectileSpawnPointCounter >= projectileSpawnPoint.Length)
        {
            currentProjectileSpawnPointCounter = 0;
        }
          currentProjectileSpawnPoint = projectileSpawnPoint[currentProjectileSpawnPointCounter];
    }

    public void ResetCurrentProjectileSpawnPointCounter()
    {
        currentProjectileSpawnPointCounter = 0;
        currentProjectileSpawnPoint = projectileSpawnPoint[currentProjectileSpawnPointCounter];
    }



    void FixedUpdate()
    {

      if (homing != Homing.WithRaycast)
        {
            return;
        }



        if (starGameManagerRef.GamePaused)
        {
            if (!localPauseFlag)
            {
                localPauseFlag = true;
                rbody.Sleep();
            }

            return;
        }

        else

        {
            if (localPauseFlag)
            {
                localPauseFlag = false;
            }

        }





        Vector3 targetPosition = starGameManagerRef.HeroShip.transform.position + new Vector3 (homingTargetOffsetX, homingTargetOffsetY, homingTargetOffsetZ);



       // Vector3 dist = starGameManagerRef.HeroShip.transform.position - transform.position;
       //  dist.y = 0; // ignore height differences


         Vector3 dist = targetPosition - transform.position;


        // calc a target vel proportional to distance (clamped to maxVel)
        Vector3 tgtVel = Vector3.ClampMagnitude(toVel * dist, maxVel);

        // calculate the velocity error
        Vector3 error = tgtVel - rbody.velocity;

        // calc a force proportional to the error (clamped to maxForce)
        Vector3 force = Vector3.ClampMagnitude(gain * error, maxForce);
        rbody.AddForce(force);
    }


    void ReducePhysicsVelocity()
    {
        workingToVel = 0.2f;
        workingMaxVel = 0.5f;
    }


    void ResumePhysicsVelocity()
    {
        workingToVel = toVel;
        workingMaxVel = maxVel;
    }


    void FireRaycast (GameObject sensor)
    {
        RaycastHit hit;
        float distance = RaycastDistanceS;
        bool angleBlocked = false;
        
        if (sensor == AboveSensor || sensor == BelowSensor)
        {
            distance = RaycastDistanceV;
        }
        
        if (sensor == LeftSensor || sensor == RightSensor)
        {
            distance = RaycastDistanceH;
        }

        Physics.Raycast(sensor.transform.position, sensor.transform.forward, out hit, distance);

        if (hit.collider != null)
        {
            foreach (string picked in CheckTag)
            {
                if (hit.collider.tag == picked)
                {
                    angleBlocked = true;
                }
            }
        }
        
        else

        {
            angleBlocked = false;
        }
       
       if (sensor == StraightSensor)
        {
            straightBlocked = angleBlocked;
        }

        if (sensor == AboveSensor)
        {
            aboveBlocked = angleBlocked;
        }

        if (sensor == BelowSensor)
        {
            belowBlocked = angleBlocked;
        }

        if (sensor == LeftSensor)
        {
            leftBlocked = angleBlocked;
        }

        if (sensor == RightSensor)
        {
            rightBlocked = angleBlocked;
        }
    }


    void UnblockAllSensors()
    {
        straightBlocked = false;
        aboveBlocked = false;
        belowBlocked = false;
        leftBlocked = false;
        rightBlocked = false;
    }

    public bool outOfRange;

    void BoundaryChecking()
    {

        if (checkTerminator != CheckTerminator.None)
        {
            if (checkTerminator == CheckTerminator.ActorsParent)
            {
                TerminatorDistance = starGameManagerRef.TerminatorDistance;
            }

            if (checkTerminator == CheckTerminator.GrandfatherWorld || checkTerminator == CheckTerminator.ActorsParent)
            {
                CurrentDistVector = Vector3.Distance(parentCheck.transform.position, gameObject.transform.position);
            }
            else
            {
                CurrentDistVector = Vector3.Distance(parentCheck.transform.localPosition, gameObject.transform.localPosition);
            }

            isBeingDrawn = !(TerminatorDistance != 0 && CurrentDistVector > TerminatorDistance);
        }
        

        if (isBeingDrawn && !hasActivatedRenderers)
        {
            ToggleOffscreenSettings(true);
            hasActivatedRenderers = true;
        }


        if (!isBeingDrawn && hasActivatedRenderers)
        {
            ToggleOffscreenSettings(false);
            hasActivatedRenderers = false;
        }
            

    }  // END BOUNDARY CHECKING


    

    void Update()
    {
        if (!IgnoreRenderList)
        {
            BoundaryChecking();
        }

        /*
        if (UpArrow != null)
        {
            GameObject[] ArrowArray = { UpArrow, DownArrow, LeftArrow, RightArrow };

            foreach (GameObject picked in ArrowArray)
            {
                picked.SetActive(false);
            }
        }
        */

        /*
         // probably fine, but untested

        if (IsObstacle)
        {
            return;
        }
        */

        if (PowerUp)
        {
            PowerUp = false;
            PowerUpFx.Invoke();
        }

        if (LifeUp)
        {
            LifeUp = false;
            LifeUpFx.Invoke();
        }



      
        if (homing == Homing.WithRaycast)
        {

            if (!collidingWGeo)
            {
                FireRaycast(StraightSensor);
            }

            else

            {
                straightBlocked = true;
            }
            
            if (straightBlocked)

            {
                foreach (GameObject picked in sensorSet)
                {
                    FireRaycast(picked);
                }
            }

            else

            {
                UnblockAllSensors();
            }
            


            if (ContinuousRaycast)
            {
                {
                    foreach (GameObject picked in sensorSet)
                    {
                        FireRaycast(picked);
                    }
                }
            }


            
                if (!straightBlocked)

                {

                  //  if (!aboveBlocked && !belowBlocked && !leftBlocked && !rightBlocked)
                  //  {
                        homingTargetOffsetX = 0;
                        homingTargetOffsetY = 0;
                        homingTargetOffsetZ = 0;
                    //  }

                    ResumePhysicsVelocity();

                    // these were factory settings
                  //  toVel = 2.0f;
                 //   maxVel = 2.5f;

                }

                else

              {

                int pickedVerticalDirRandom = 0;

                if (!belowBlocked && !aboveBlocked)
                {
                    pickedVerticalDirRandom = Random.Range(1, 2);
                }


                    if ((aboveBlocked && !belowBlocked) || pickedVerticalDirRandom == 1)

                    {
                     //   print("should be moving down");
                        homingTargetOffsetY -= OffsetIncrements;
                        ReducePhysicsVelocity();
                    if (DownArrow != null)
                        {
                            DownArrow.SetActive(true);
                        }
                    }

                    else

                    {
                        if ((belowBlocked && !aboveBlocked) || pickedVerticalDirRandom == 2)

                        {
                         //   print("should be moving up");
                            homingTargetOffsetY += OffsetIncrements;
                            ReducePhysicsVelocity();
                        if (UpArrow != null)
                            {
                                UpArrow.SetActive(true);
                            }
                        }
                    }
                    

                // check sides blocked


                int pickedHorizDirRandom = 0;

                if (!leftBlocked && !rightBlocked)
                {
                    pickedHorizDirRandom = Random.Range(1, 2);
                }


                if ((leftBlocked && !rightBlocked) || pickedHorizDirRandom == 1)

                    {
                        //  print("LEFT BLOCKED");
                        // homingTargetOffsetY -= OffsetIncrements;
                        ReducePhysicsVelocity();

                        if (!FlipBlockedSides)
                        {
                            homingTargetOffsetX -= OffsetIncrements;
                            if (RightArrow != null)
                            {
                                RightArrow.SetActive(true);
                            }
                        }

                        else

                        {
                            homingTargetOffsetX += OffsetIncrements;
                            if (RightArrow != null)
                            {
                                RightArrow.SetActive(false);
                            }
                        }
                    }

                    else

                    {
                        if ((rightBlocked && !leftBlocked) || pickedHorizDirRandom == 2)

                        {
                            //   print("RIGHT BLOCKED");

                            //     homingTargetOffsetY += OffsetIncrements;
                            ReducePhysicsVelocity();

                            if (!FlipBlockedSides)
                            {
                                homingTargetOffsetX += OffsetIncrements;
                                if (LeftArrow != null)
                                {
                                    LeftArrow.SetActive(true);
                                }
                            }

                            else
                            {
                                homingTargetOffsetX -= OffsetIncrements;
                                if (LeftArrow != null)
                                {
                                    LeftArrow.SetActive(false);
                                }
                            }
                        }
                    }
            }
        }



       

        if (starGameManagerRef != null)
        {
            workingSpeed = MoveSpeed * starGameManagerRef.GameSpeed;
        }



       
        if (homing == Homing.NoRaycast)
        {
            float step = workingSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, starGameManagerRef.HeroShip.transform.position, step);
        }




        /*
        if (homingToHero)
        {
          //  gameObject.GetComponent<Rigidbody>.useGravity = false;

            //  Rigidbody.velocity = Vector3.zero;
            //  Rigidbody.angularvelocity = 0;

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

            rigidbody.velocity = Vector3.zero;
           // rigidbody.angularVelocity = 0.0f;



            Vector3 Force = Vector3.zero;
            Vector3 TmpForce = Vector3.MoveTowards(this.transform.position, starGameManagerRef.HeroShip.transform.position, workingSpeed);
            Force = new Vector3(TmpForce.x - this.transform.position.x, TmpForce.y - this.transform.position.y,
                TmpForce.z - this.transform.position.z);

            // Rigidbody.Addforce(Force);
            // Rigidbody.Addforce(transform.forward);

           //  rigidbody.AddForce(Force);

              rigidbody.AddForce(transform.forward);


        }

        */
        
    }  // END UPDATE



    void BeginShootingCounter()
    {
        if (!ProjectileFiringEnabled || !isBeingDrawn)
        {
            return;
        }

        if (parent != null)
        {
            if (!parent.activeSelf)
            {
                return;
            }
        }


        if (fireRate > 0 && starGameManagerRef.EnemiesCanShoot && !starGameManagerRef.GamePaused)

        {

            if (onlyLeaderShoots && leader)
            {
                StartCoroutine(Shooting());
            }

            else

            {
                if (!onlyLeaderShoots)
                {
                    StartCoroutine(Shooting());
                }
            }
        }
    }



    void TriggerAnimPattern(string thisAnimPattern, Animator thisAnimator)
    {
        if (thisAnimPattern != "" && thisAnimator != null)
        {
            thisAnimator.SetTrigger(thisAnimPattern);
        }
    }

    void DoOnRestart()
    {

        if (AudioOnStart != null)
        {
            PlayAudio(AudioOnStart);
        }

        TriggerAnimPattern(animPattern, animator);

        if (grandParent != null)
        {
            newParent = grandParent.transform;
        }


        if (leader)

        {
            StartCoroutine(SpawnNextFollower());
        }

        else
        {
            followersList.Clear(); // shouldn't need to do this
        }



        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        if (!IgnoreRenderList)
        {
            isBeingDrawn = false;
            hasActivatedRenderers = false;
        }

        if (meshRenderers.Count != 0)
        {
            foreach (Renderer picked in meshRenderers)
            {
                picked.enabled = false;
            }
        }

    }


    void StarGameManagerRefChecker()
    {
       if (starGameManagerRef == null)
           {
            starGameManagerRef = StarGameManager.instance;
            pauseAnimList = starGameManagerRef.PauseAnimList;
           }
    }

    void OnEnable()

    {

        StarGameManagerRefChecker();

        if (animator != null)

        {
            animator.SetFloat("multiplier", MoveSpeed);

            if (SubscribeToAnimPauseList)
            {
                pauseAnimList.Add(animator);

                if (starGameManagerRef.GamePaused)
                {
                    animator.speed = 0;
                }
            }
        }


            if (checkTerminator == CheckTerminator.None)
        {
            IgnoreRenderList = true;
            isBeingDrawn = true;
            hasActivatedRenderers = true;
        }

        if (IgnoreRenderList)
        {
            BeginShootingCounter();
        }

        if (gameObject.transform.parent != null)
        {
            leaderBeginPos = gameObject.transform.parent.localPosition;  // problem here from PowerupShell
        }

        collidingWGeo = false;

        if (rbody != null)
        {
            rbody.Sleep();
        }

        if (ShadowBaseSize > 0)
        {
            shadowObject = null;
            shadowObject = starGameManagerRef.SpawnedChecker(starGameManagerRef.GenericShadowObject);

            if (shadowObject == null)
            {
                return;
            }

            // useObject.transform.SetParent(prefabParent.transform, false);
            shadowObject.transform.SetParent(starGameManagerRef.ActorsParent.transform, false);
            
            GenericShadow genericShadow = shadowObject.GetComponent<GenericShadow>();
            genericShadow.followObjectXZ = this.gameObject;  // make sure this is occuring // rma0909
            genericShadow.followObjectY = starGameManagerRef.ShadowYObject;
            genericShadow.SizeMultiplier = ShadowBaseSize;
            genericShadow.MinimumSize = MinShadowSize;
            genericShadow.MaximumSize = MaxShadowSize;

            // force shadowObject.SetActive(true) if object is rendering

            if (forceShadowActiveAtStart)
                {
                    shadowObject.SetActive(true);
                }
        }

        //  print("I GOT ENABLED");  // yes

        if (deactivated)
        {

            if (leader)
            {
                DoOnRestart();
            }
            else
            {
                TriggerAnimPattern(animPattern, animator);// does this belong here?
            }

            deactivated = false;
        }

    }  // END ENABLE


    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(fireRate);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        fireProjectile(starGameManagerRef.ActorsParent);
        BeginShootingCounter();
    }


    public void FireProjectileNoParent()
    {
        fireProjectile(null);
    }


    void PlayAudio(AudioClip clip)  // should reference this in SGM instead!
    {
        audioSource.clip = clip;
          audioSource.Play();
    }
    

    public void fireProjectile(GameObject prefabParent)
    {
        if (!ProjectileFiringEnabled || !isBeingDrawn)
        {
            return;
        }

        if (prefabParent == null)
        {
            prefabParent = starGameManagerRef.ActorsParent;
        }

        if (SimultaneousSpawns)
        {
            for (int i = 0; i < projectileSpawnPoint.Length; i++)
            {
                DoProjectileFiring(prefabParent, projectileSpawnPoint[i]);
            }
        }
        else
        {
            DoProjectileFiring(prefabParent, currentProjectileSpawnPoint);
        }
    }




    void DoProjectileFiring(GameObject prefabParent, GameObject currentSpawnPoint)
    {
        // this section is used to tell enemies to be projectiles, as in final level doors spitting out ships
      //  print("doing proj fire from "+gameObject);
        GameObject useObject = null;
        useObject = starGameManagerRef.SpawnedChecker(projectile);

        if (ProjectileAudio != null && audioSource != null)
        {
            PlayAudio(ProjectileAudio);
        }

        if (useObject == null)
        {
            return;
        }

        useObject.transform.SetParent(prefabParent.transform, false);

        useObject.transform.position = currentSpawnPoint.transform.position;
        useObject.transform.rotation = currentSpawnPoint.transform.rotation;
        ProjectileEvent.Invoke();

        if (!SimultaneousSpawns && projectileSpawnPoint.Length > 1  && AutoIterateSpawnPoints)
        {
            IncrementCurrentProjectileSpawnPointCounter();
        }


        if (ShotDirection != ShotDirection.NA)
        {
            if (useObject.GetComponent<StarProjectileAnim>())
            {
                useObject.GetComponent<StarProjectileAnim>().shotDirection = ShotDirection;
            }
        }

        if (SpawnOrientation != Vector3.zero)
        {
            useObject.transform.localEulerAngles = (SpawnOrientation);
        }

        useObject.SetActive(true);

        // this one works!!! -----------------------------------------------------------------------------------

        starEnemy starEnemy = null;
        if (useObject.GetComponentInChildren<starEnemy>())
        {
             starEnemy = useObject.GetComponentInChildren<starEnemy>();
        }

        if (starEnemy != null)
        {
            starEnemy.leader = true;
            starEnemy.forceShadowActiveAtStart = true;
        }

        if (SpawnAnimTriggers.Length != 0)
        {

            if (useObject.GetComponentInChildren<Animator>())
            {
                Animator spawnAnimator = useObject.GetComponentInChildren<Animator>();
                foreach (string trigger in SpawnAnimTriggers)
                {
                    spawnAnimator.SetTrigger(trigger);
                    if (starEnemy != null)
                    {
                        starEnemy.animPattern = trigger;
                    }
                    // hopefully won't cause issues //rma2019_09_25
                    //   useObject.GetComponentInChildren<starEnemy>().SpawnAnimTriggers = SpawnAnimTriggers;  // "safer" but effect not as good
                }
            }
        }
    }


    

    private IEnumerator SpawnNextFollower()
    {

        for (int i = 0; i < (followers); i++)
        {

            yield return new WaitForSeconds(delayFollowersSpawn);

            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }


            useFollowDistanceX = useFollowDistanceX + followerDistanceX;
            useFollowDistanceY = useFollowDistanceY + followerDistanceY;
            useFollowDistanceZ = useFollowDistanceZ + followerDistanceZ;
            
           if (parent != null)
            {

                GameObject useObject = null;

                useObject = starGameManagerRef.SpawnedChecker(parent);

                if (useObject == null)
                {
                    break;
                }

                bool useGrandParent = false;

                if (parent.transform.parent)
                {
                    useObject.transform.SetParent(parent.transform.parent, false);  //problem here rma920
                    useGrandParent = true;
                }

                starEnemy useObjectStarEnemy = useObject.GetComponentInChildren<starEnemy>();
                useObjectStarEnemy.leader = false;
                useObjectStarEnemy.leaderObject = this.gameObject;

                if (!useGrandParent)
                {
                    useObject.transform.SetParent(newParent, false);
                }

                useObject.transform.localPosition = leaderBeginPos;  // have to test w offset
                useObject.transform.localPosition = new Vector3(useObject.transform.localPosition.x + useFollowDistanceX,
                useObject.transform.localPosition.y + useFollowDistanceY, useObject.transform.localPosition.z + useFollowDistanceZ);

                followersList.Add(useObject);

                useObject.SetActive(true);

                string useAnimPattern = useObjectStarEnemy.animPattern;

                if (SpawnAnimTriggers.Length !=0)
                {
                    useAnimPattern = SpawnAnimTriggers[0];  
                    // very dirty don't like this
                    // actually not currently using this technique
                }

                // TriggerAnimPattern(useObjectStarEnemy.animPattern, useObjectStarEnemy.animator);

                TriggerAnimPattern(useAnimPattern, useObjectStarEnemy.animator);


            } // END IF PARENT NULL
        }
        
    }


    public void CharacterDestroyed(bool killed)
    {

        if (killed)
            // so killed = false if don't want character to explode on death, i.e.
            // if this is being called to run when out-of-bounds/end of anim

         //   RemoveFromAnimPauseList();

        {
            if (deathPrefab != null)
            { 

                // check if instantiated

                GameObject deathPrefabUse = null;

                // deathPrefabUse = PoolCheck(starGameManagerRef.explosionsPoolInactive, deathPrefab.name);

                // this may give an error if starEnemy is initialized at play
                deathPrefabUse = starGameManagerRef.SpawnedChecker(deathPrefab);  // was sending TRUE


                /*
                if (deathPrefabUse == null)

                {
                    deathPrefabUse = Instantiate(deathPrefab) as GameObject;
                    deathPrefabUse.name = deathPrefab.name;
                    deathPrefabUse.transform.SetParent(parent.transform.parent, false);
                }
                */

                // if (parent != null)
                if (!IsObstacle && deathPrefabUse != null)
                {
                    deathPrefabUse.transform.SetParent(parent.transform.parent, false);
                }

                if (DeathPrefabSpawnpoint == null)
                {
                    DeathPrefabSpawnpoint = gameObject;
                }

                if (deathPrefabUse != null)
                {
                    deathPrefabUse.transform.position = DeathPrefabSpawnpoint.transform.position;
                    deathPrefabUse.SetActive(true);
                }

            }
            
            /*
            if (homing == Homing.WithRaycast || ZeroPosOnDeath)
            {
                transform.localPosition = Vector3.zero;
            }
            */

        }  // end killed


        if (homing == Homing.WithRaycast || ZeroPosOnDeath)
        {
            transform.localPosition = Vector3.zero;
        }
        // moved this out of "killed check" so that enemy will reset to 0 no matter what..



        // if (parent != null)
        if (!IsObstacle)

        {
            parent.SetActive(false);
        }

        if (leader)
        {

            if (followersList.Count != 0)
            {
                GameObject newLeader = followersList[0];

                starEnemy newStarEnemy = newLeader.GetComponentInChildren<starEnemy>();

                newStarEnemy.leader = true;
                newStarEnemy.leaderObject = null;
                newStarEnemy.BeginShootingCounter();

                starGameManagerRef.leaderList.Add(newLeader);


                for (int i = 0; i < (followersList.Count); i++)

                {
                    if (newLeader != followersList[i])
                    {
                        newStarEnemy.followersList.Add(followersList[i]);
                    }
                }

                followersList.Clear();

            } // end followersList

            StopCoroutine(SpawnNextFollower());
            starGameManagerRef.leaderList.Remove(parent);

        } // END LEADER

         ClearEnemyProperties();
        
    }



    public void ForceDropOnDeath()
    {
        GameObject useObject = null;
        useObject = starGameManagerRef.SpawnedChecker(DropOnDeath);

        if (useObject == null)
        {
            return;
        }

        useObject.transform.parent = gameObject.transform.parent.transform;
        useObject.transform.localPosition = Vector3.zero;
        useObject.transform.localEulerAngles = Vector3.zero;
        useObject.transform.localScale = Vector3.one;
        useObject.SetActive(true);
    }


    void RemoveFromAnimPauseList()
    {
        if (SubscribeToAnimPauseList)
        {
            if (pauseAnimList.Contains(animator))
            {
                pauseAnimList.Remove(animator);
            }
        }
    }


    void OnDisable()
    {
        if (starGameManagerRef == null)
        {
            return;
        }

        if (shadowObject != null)
        {
            shadowObject.SetActive(false);
            shadowObject = null;
        }

        //   if (DropOnDeath != null)
        if (DropOnDeath != null && Extra != "")

        {

            if (!disableSpecialEffectOnDeath)
            { 
                GameObject useObject = null;
                useObject = starGameManagerRef.SpawnedChecker(DropOnDeath);

                if (useObject == null)
                {
                    return;
                }
                
                // useObject.transform.SetParent(prefabParent.transform, false);

                useObject.transform.position = currentProjectileSpawnPoint.transform.position;
                useObject.transform.rotation = currentProjectileSpawnPoint.transform.rotation;
               // useObject.transform.localScale = new Vector3(3, 3, 3);

                if (starGameManagerRef.Caboose != null)
                {
                       useObject.transform.parent = starGameManagerRef.Caboose.transform;
                }

                // passes POW type to animator and value for gameManager

                starEnemy useObjectStarEnemy = useObject.GetComponentInChildren<starEnemy>();
                useObjectStarEnemy.animPattern = Extra;
                useObjectStarEnemy.Extra = Extra;
                useObjectStarEnemy.forceShadowActiveAtStart = true;

                useObject.SetActive(true);
                useObject.transform.localScale = new Vector3(DropScale, DropScale, DropScale);

                if (starGameManagerRef.gameplayMode == StarGameManager.GameplayMode.Showcase)
                {
                    useObjectStarEnemy.SetHomingModeNoRaycast();
                }
            }
        }

        CharacterDestroyed(false);
        UnblockAllSensors();


        if (gameObject.transform.parent != null)
        {
            if (!gameObject.transform.parent.gameObject.activeSelf)
            {
                disableSpecialEffectOnDeath = true;
            }
        }


        // this is where pickup effect occurs

        if (!disableSpecialEffectOnDeath)
        {
            /*
            
            // gets disabled before it can actually play, and plays on next appear
            if (CollisionAudio != null && audioSource != null)
            {
                print("SHOULD PLAY PICKUP SOUND");
                PlayAudio(CollisionAudio);
            }
            */


            if (Extra == PowerupType.LevelUp.ToString())
            {
                {
                   // increasing POW level
                    starGameManagerRef.IncreasePowLevel();
                }
            }

            if (Extra == PowerupType.OneUp.ToString())
            {
                {
                    // increasing life by one
                    starGameManagerRef.currentLives++;
                }
            }

            if (Extra == PowerupType.Bomb.ToString())
            {
                {
                    // deploying bomb
                    starGameManagerRef.BombEnemies();
                }
            }

            if (Extra == PowerupType.Starman.ToString())
            {
                {
                    // starman invincibile
                    starGameManagerRef.SetHeroStarmanMode();
                }
            }
            
        }

        disableSpecialEffectOnDeath = false;

        if (AlertControllerOnDisable != null && parent != null)
        {
            AlertControllerOnDisable.GetComponent<BossControllerGeneric>().AlertedSubObjectDisabled(parent);
        }

    }  // end disable


    void ClearEnemyProperties()
    {
        RemoveFromAnimPauseList();

        leader = false;
        deactivated = true;

        if (followersList.Count > 0) followersList.Clear();

        if (leaderObject != null)
        {
            leaderObject.GetComponent<starEnemy>().followersList.Remove(parent);
            leaderObject = null;
        }
        
    }
    
}
