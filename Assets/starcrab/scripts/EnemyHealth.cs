using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour {

    StarGameManager starGameManagerRef;

    public enum CollideEffect { None, Damage, Immune }
    public enum CollideEventOn { OnlyDamage, OnlyImmune, Both }

    public Animator animator;
    public GameObject mainObject;
    public bool UseGameManagerAsMainHealth;
    public string[] vulnerableToTag;
    private int beginHealth; // mk prv
    public int BeginHealthEasy;
    public int BeginHealthHard;

    public int health;
    

    public bool cantDie;
    public bool cantTakeDamage;

    GameObject usingObject;
    EnemyHealth componentRef;

    public GameObject CollideParentToMessage;
    public GameObject doOnCollide;
    public bool UseAutoDamageFlash;
    public CollideEffect collideEffect;
    public CollideEventOn collideEventOn;
    public UnityEvent CollideEvent;
    public UnityEvent DeathEvent;
    private Material origMaterial;
    private Renderer materialRenderer;
    public bool CheckTriggerCollision;

    [System.Serializable]

    public class instanceList

    {
        public string Note = "";
        public int HealthLevel;
        [HideInInspector]
        public bool didEffect;
        public UnityEvent HealthEvent;
        public Animator AnimatorOverride;
        public string BoolName;
        public bool SetState;
    }

    public List<instanceList> InstanceList;


    public void OnEnable()
    {

        StargameManagerCheck();

        if (starGameManagerRef != null)
        {
            if (starGameManagerRef.gameplayMode == StarGameManager.GameplayMode.Showcase)
            {
                beginHealth = BeginHealthEasy;
            }
            else
            {
                beginHealth = BeginHealthHard;
            }

            health = beginHealth;
        }
    }


    void StargameManagerCheck()
    {
        if (starGameManagerRef == null)
        {
            if (StarGameManager.instance)
            {
                starGameManagerRef = StarGameManager.instance;
            }
        }
    }


    void Start() {

        StargameManagerCheck();

        if (UseGameManagerAsMainHealth)
        {
            mainObject = starGameManagerRef.gameObject;
        }

        materialRenderer = gameObject.GetComponent<Renderer>();

        if (mainObject == null)
            usingObject = gameObject;
        else usingObject = mainObject;

        componentRef = usingObject.GetComponent<EnemyHealth>();

        //        componentRef = EnemyHealth; // CAN I DO THIS?


        //  health = beginHealth;


        foreach (instanceList inList in InstanceList)
        {
            if (inList.AnimatorOverride == null)
            {
                inList.AnimatorOverride = animator;
            }
        }

        }
    

    void PerformCollideEvent()
    {
        CollideEvent.Invoke();

        if (UseAutoDamageFlash)
        {
            if (origMaterial == null)
            {
                origMaterial = gameObject.GetComponent<Renderer>().material;
            }
            StartCoroutine(AutoDamageFlash());
        }
    }
    
    IEnumerator AutoDamageFlash()
    {
        materialRenderer.material = starGameManagerRef.DamageFlashMaterial;
        yield return new WaitForSeconds(0.175f);
        
        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        materialRenderer.material = origMaterial;
    }



    void Update()
    {

        //   if (health <= 0)

        if (!cantDie)
        { 
            if (componentRef.health <= 0)
            {


                if (GetComponent<starEnemy>())
                {
                    GetComponent<starEnemy>().CharacterDestroyed(true);
                    starGameManagerRef.currentScore = starGameManagerRef.currentScore + (usingObject.GetComponent<starEnemy>().pointValue * starGameManagerRef.CurrentMultiplier);

                }


                /*

               if (GetComponent<starPlayer>())
               {
                   GetComponent<starPlayer>().CharacterDestroyed();
               }
               */

                // only will play this if Main Object is the thing that is dying (not an extension)
                DeathEvent.Invoke();



                if (GetComponent<StarGameManager>())

                {
                    componentRef.health = beginHealth;
                }


            }
        }
    }

    void CheckHealthEvents()
    {
        foreach (instanceList inList in InstanceList)
        {
            if (health < inList.HealthLevel)
            {
                if (!inList.didEffect)
                {
                    inList.didEffect = true;
                    inList.HealthEvent.Invoke();
                    inList.AnimatorOverride.SetBool(inList.BoolName, inList.SetState);
                }
            }
        }
    }


    /*
    GameObject PoolCheck(List<GameObject> activeList, string checkName)
    {
        GameObject useThisObject = null;

        if (activeList.Count > 0)
        {
            for (int i = 0; i < activeList.Count; i++)

            {

                if (checkName == activeList[i].name)

                {
                    useThisObject = activeList[i];
                    break;
                }
            }

        }
        return useThisObject;
    }
    */



      public void RegenerateHealth()  // may not use this, ever
    {
        health = beginHealth;
    }



    

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTriggerCollision)
        {
            CollisionInteraction(other.gameObject);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        CollisionInteraction(col.gameObject);
    }



  void CollisionInteraction (GameObject col)
    {

        if (gameObject.GetComponent<starEnemy>())
        {
            //  if (gameObject.GetComponent<starEnemy>().offscreen)
            if (!gameObject.GetComponent<starEnemy>().isBeingDrawn)

            {
                return;
            }
        }

        foreach (string picked in vulnerableToTag)
        {
            if (col.tag == picked)

            {
                // only will play this if actual hit sensor is collided. So if Main Boss has this, won't necessarily be called

                // CollideEvent.Invoke();

                if (col.GetComponent<StarProjectileAnim>())
                {
                    if (collideEffect == CollideEffect.Damage)
                    {

                        if (col.GetComponent<StarProjectileAnim>().deathPrefabHarm != null)
                        {
                            GameObject useObject = null;

                            //  useObject = PoolCheck(starGameManagerRef.explosionsPoolInactive, col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabHarm.name);
                            // GameObject PoolCheck(List<GameObject> activeList, string checkName)

                            useObject = starGameManagerRef.SpawnedChecker(col.GetComponent<StarProjectileAnim>().deathPrefabHarm);  // was sending FALSE


                            /*
                            if (useObject == null)  // HOPEFULLY THIS IS UNNECESSARY
                            {
                                useObject = Instantiate(col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabHarm) as GameObject;
                                useObject.name = col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabHarm.name;
                                //  useObject.transform.SetParent(grandParent.transform, false);
                                useObject.transform.SetParent(gameObject.transform.parent.transform.parent.transform, false);
                            }
                            */



                            // do i need this? //rma820
                            //  useObject.transform.SetParent(gameObject.transform.parent.transform.parent.transform, false);  // not really sure what this mess does

                            if (useObject != null)
                            {
                                useObject.SetActive(true);
                                useObject.transform.position = col.transform.position;
                            }

                        }

                        if (collideEventOn == CollideEventOn.Both || collideEventOn == CollideEventOn.OnlyDamage)
                        {
                            PerformCollideEvent();
                        }

                        if (CollideParentToMessage != null)
                        {
                            CollideParentToMessage.GetComponent<BossControllerGeneric>().
                                ChildCollisionAlert = true;
                        }
                    }


                    if (collideEffect == CollideEffect.Immune)
                    {

                        if (col.GetComponent<StarProjectileAnim>().deathPrefabImm != null)
                        {
                            GameObject useObject = null;

                            //  useObject = PoolCheck(starGameManagerRef.explosionsPoolInactive, col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabImm.name);

                            useObject = starGameManagerRef.SpawnedChecker(col.GetComponent<StarProjectileAnim>().deathPrefabImm);  // was sending FALSE




                            /*
                             // LIKELY CAN ALSO DELETE AFTER TEST
                            if (useObject == null)
                            {
                                useObject = Instantiate(col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabImm) as GameObject;
                                useObject.name = col.gameObject.GetComponent<StarProjectileAnim>().deathPrefabImm.name;
                                //  useObject.transform.SetParent(grandParent.transform, false);
                                useObject.transform.SetParent(gameObject.transform.parent.transform.parent.transform, false);
                            }
                            */
                            if (useObject != null)

                            {
                                useObject.transform.SetParent(gameObject.transform.parent.transform.parent.transform, false);  // this is awful, awful!!
                                useObject.SetActive(true);
                                useObject.transform.position = col.transform.position;
                            }
                        }

                        if (collideEventOn == CollideEventOn.Both || collideEventOn == CollideEventOn.OnlyImmune)
                        {
                            PerformCollideEvent();
                        }

                    }

                    if (!cantTakeDamage)
                    {
                        componentRef.health -= col.GetComponent<StarProjectileAnim>().projectileStrength;
                        CheckHealthEvents();
                    }
                    //  col.gameObject.SetActive(false);  // switched this rma916
                    col.GetComponent<StarProjectileAnim>().DestroyProjectile();
                }

                else if (col.GetComponent<starEnemy>())
                {

                    if (collideEventOn == CollideEventOn.Both || collideEventOn == CollideEventOn.OnlyDamage)
                    {
                        PerformCollideEvent();
                    }

                    if (!cantTakeDamage)
                    {
                        componentRef.health -= col.GetComponent<starEnemy>().projectileStrength;
                    }

                    if (col.GetComponent<starEnemy>().canDieOnContact)
                    {
                        col.GetComponent<EnemyHealth>().DeathEvent.Invoke();
                        col.GetComponent<starEnemy>().CharacterDestroyed(true);
                    }
                }
            }
        }

    }

    }
