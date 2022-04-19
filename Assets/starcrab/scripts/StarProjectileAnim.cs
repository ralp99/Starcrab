using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarProjectileAnim : MonoBehaviour {

    public enum CheckTerminator { None, ActorsParent, Father, Grandfather }
    public enum DestroyType { Destroy, Disable, Passthrough }
   // public enum ShotDirection { NA, Forward, Backward, Left, Right, Above, Below }
    public enum TerminatorComparison { Less, Greater }
    public enum SpeedType { GameSpeed, ProjectileSpeed }

    public int projectileStrength;
    public float MoveSpeed;
    public DestroyType destroyType;
    public ShotDirection shotDirection;
    public SpeedType speedType = SpeedType.ProjectileSpeed;
    public GameObject UsingObject;
    public GameObject deathPrefabHarm, deathPrefabImm;

    StarGameManager starGameManagerRef;
    private float workingSpeed;
    private GameObject parentCheck; 
    public float CurrentDistVector;
    public float TerminatorDistanceOverride = 0.0f;
    private float terminatorDistance;
    public CheckTerminator checkTerminator;
    public bool NotifyOnly;
    public bool DelayFiring;
    [Space]

    public GameObject DetonateSpawnObject;
    public float DetonateTime;
    public float DetonateTimeJitter;
    private float detonateTime;
    private bool beganDetonatorTimer;
    private float DetonateRadius;
    [Space]

    public float XVariance;
    public float XJitter;
    // private float xVariance;
    private float xMoveSpeed;
    private float workingXSpeed;
    public float XSwitchTime;
    public float XSwitchTimeJitter;
    private float xSwitchTime;
    [Space]

    public float YVariance;
    public float YJitter;
    private float yMoveSpeed;
    private float workingYSpeed;
    public float YSwitchTime;
    public float YSwitchTimeJitter;
    private float ySwitchTime;

    private bool reenabled;

    private IEnumerator xMovementCoroutine;
    private IEnumerator yMovementCoroutine;

    public void ManualDetonate()
    {
        PerformDetonate();
    }

    void Start ()

    {

        starGameManagerRef = StarGameManager.instance;

        if (parentCheck == null && checkTerminator != CheckTerminator.None)
        {

            if (checkTerminator == CheckTerminator.Grandfather)
            {
                parentCheck = gameObject.transform.parent.transform.parent.gameObject;
            }

            if (checkTerminator == CheckTerminator.Father)

            {
                parentCheck = gameObject.transform.parent.gameObject;
            }

            if (checkTerminator == CheckTerminator.ActorsParent)

            {
                parentCheck = starGameManagerRef.ActorsParent;
            }

        }

        if (UsingObject == null)
        {
            UsingObject = gameObject;
        }

        OnReenable();
    }







    IEnumerator DetonateTimer()
    {
        yield return new WaitForSeconds(detonateTime);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        PerformDetonate();
        
    }

    void PerformDetonate()
    {
        GameObject deathPrefabUse = null;

        if (DetonateSpawnObject != null)
        {
            deathPrefabUse = starGameManagerRef.SpawnedChecker(DetonateSpawnObject);  // was sending TRUE
        }

        if (deathPrefabUse != null)
        {
            deathPrefabUse.transform.position = transform.position;
            deathPrefabUse.transform.parent = starGameManagerRef.ActorsParent.transform;
            deathPrefabUse.SetActive(true);
        }

        DestroyProjectile();
    }



    private void OnReenable()
    {
        reenabled = true;
        beganDetonatorTimer = false;

        if (xMovementCoroutine != null)
        {
            StopCoroutine(xMovementCoroutine);
            xMovementCoroutine = null;
        }

        if (yMovementCoroutine != null)
        {
            StopCoroutine(yMovementCoroutine);
            yMovementCoroutine = null;
        }
        
        

        if (XVariance != 0 || XJitter !=0)

        {

            if (Random.value > 0.5f)
            {
                XVariance = XVariance * -1;
            }

            if (Random.value > 0.5f)
            {
                XJitter = XJitter * -1;
            }

            xMoveSpeed = XVariance + Random.Range(0, XJitter);
        }
        

        if (YVariance != 0 || YJitter != 0)
        {

            if (Random.value > 0.5f)
            {
                YVariance = YVariance * -1;
            }

            if (Random.value > 0.5f)
            {
                YJitter = YJitter * -1;
            }

            yMoveSpeed = YVariance + Random.Range(0, YJitter);
        }

        if (XSwitchTime != 0 || XSwitchTimeJitter != 0)
        {
            xMovementCoroutine = SwitchMoveTimer(true);
            StartCoroutine(xMovementCoroutine);
        }

        if (YSwitchTime != 0 || YSwitchTimeJitter != 0)
        {
            yMovementCoroutine = SwitchMoveTimer(false);
            StartCoroutine(yMovementCoroutine);
        }
    }


    IEnumerator SwitchMoveTimer(bool usingX)
    {


        if (Random.value > 0.5f)
        {
            YSwitchTimeJitter = YSwitchTimeJitter * -1;
        }

        float duration = YSwitchTime + Random.Range(0, YSwitchTimeJitter);

        if (usingX)
        {
            if (Random.value > 0.5f)
            {
                XSwitchTimeJitter = XSwitchTimeJitter * -1;
            }

            duration = XSwitchTime + Random.Range(0, XSwitchTimeJitter);
        }

        yield return new WaitForSeconds(duration);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        if (usingX)
        {
            xMoveSpeed = xMoveSpeed * -1;
        }
        else
        {
            yMoveSpeed = yMoveSpeed * -1;
        }


        if (usingX)
        {
            xMovementCoroutine = SwitchMoveTimer(true);
            StartCoroutine(xMovementCoroutine);
        }
        else
        {
            yMovementCoroutine = SwitchMoveTimer(false);
            StartCoroutine(yMovementCoroutine);
        }

    }




        private void Update()

    {

        if (starGameManagerRef == null || DelayFiring)
        {
            return;
        }

        if (!reenabled)
        {
            OnReenable();
        }


        if (DetonateSpawnObject != null || (DetonateTime != 0 && DetonateSpawnObject == null))
        {
            if (!beganDetonatorTimer)
            {
                beganDetonatorTimer = true;
                detonateTime = DetonateTime + Random.Range(0, DetonateTimeJitter);
                StartCoroutine(DetonateTimer());
            }
        }

        if (TerminatorDistanceOverride != 0)
        {
            terminatorDistance = TerminatorDistanceOverride;
        }

        else
        {
            terminatorDistance = starGameManagerRef.TerminatorDistance;
        }

        if (parentCheck != null)
        {

            if (checkTerminator == CheckTerminator.ActorsParent)

            {
                CurrentDistVector = Vector3.Distance(parentCheck.transform.position, gameObject.transform.position);
            }

            else
            {
                CurrentDistVector = Vector3.Distance(parentCheck.transform.localPosition, gameObject.transform.localPosition);
            }

            if (terminatorDistance != 0 && CurrentDistVector > terminatorDistance)
            {
                if (NotifyOnly)
                {
                    print ("REACHED BOUNDARY");
                }

                else

                {
                    transform.localPosition = new Vector3(0, 0, 0);
                    UsingObject.SetActive(false);
                }

            }
        }


        if (starGameManagerRef != null)
        {

            float speedMultiplier = starGameManagerRef.GameSpeed;

            if (speedType == SpeedType.ProjectileSpeed)
                {
                    speedMultiplier = starGameManagerRef.ProjectileSpeed;
                }

            workingSpeed = MoveSpeed * speedMultiplier;
            workingXSpeed = xMoveSpeed * speedMultiplier;
            workingYSpeed = yMoveSpeed * speedMultiplier;


        }
        
        if (shotDirection == ShotDirection.Forward)
        {
             transform.Translate(workingXSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime, workingSpeed * Time.deltaTime);
        }

        if (shotDirection == ShotDirection.Backward)
        {
              transform.Translate(workingXSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime, -1 * workingSpeed * Time.deltaTime);
        }

        if (shotDirection == ShotDirection.Left)
        {
            transform.Translate(workingXSpeed * Time.deltaTime, -1 * workingSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime);
        }

        if (shotDirection == ShotDirection.Right)
        {
            transform.Translate(workingXSpeed * Time.deltaTime, workingSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime);
        }

        
        if (shotDirection == ShotDirection.Above)

        {
            transform.Translate(workingSpeed * Time.deltaTime, workingXSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime);
        }

        if (shotDirection == ShotDirection.Below)

        {
            transform.Translate(-1 * workingSpeed * Time.deltaTime, workingXSpeed * Time.deltaTime, workingYSpeed * Time.deltaTime);
        }
    }



    // MIGHT NEED TO RE-ENABLE?
    /*
    void OnEnable()

    {
      //  if (starGameManagerRef == null) starGameManagerRef = StarGameManager.instance;
    }
    */


   // void Update () {
        //   transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // transform.Translate((0,0,1) * Time.deltaTime*speed);
        // ------


      //     transform.Translate(directionVector * Time.deltaTime * speed);
        // use mechanim instead


   // }


    public void DestroyProjectile()
    {

        // if (destroyMeOnImpact) Destroy(gameObject);
        //else gameObject.SetActive(false);

      //  beganDetonatorTimer = false;
        reenabled = false;

        if (destroyType == DestroyType.Destroy)
        {
            Destroy(UsingObject);
        }

        if (destroyType == DestroyType.Disable)
        {
            UsingObject.SetActive(false);
        }

        if (destroyType == DestroyType.Passthrough)
        {

        }



        /*
        if (starGameManagerRef.activeEnemies.Contains(UsingObject))
            
              {
                 starGameManagerRef.activeEnemies.Remove(UsingObject);
              }
              */

        /// SHOULD NOT NEED TO USE THIS - BUT IF BULLETS ARE JUMPING AROUND INCORRECTLY AFTER FIRING, TRY THIS IF NECESSARY
        /*
        if (!starGameManagerRef.inactiveEnemiesPool.Contains(UsingObject))
        {
            starGameManagerRef.inactiveEnemiesPool.Add(UsingObject);
        }
        */

        // INSTEAD OF THE FOLLOWING:

        /*
        starGameManagerRef.activeEnemies.Remove(UsingObject);
        starGameManagerRef.inactiveEnemiesPool.Add(UsingObject);
        */

    }



    void OnDisable()
    {
        /*
        if (starGameManagerRef.HeroBulletList.Contains(UsingObject))
        {
            starGameManagerRef.HeroBulletList.Remove(UsingObject);
        }
        */

        DestroyProjectile();
    }


}
