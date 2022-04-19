using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StarShipController : MonoBehaviour {


    public enum ObjectOriented { Null, North, South, East, West }
    public enum ControlOrient { Null, Ahead, Behind, WallSide, GroundSide }

    public ObjectOriented cameraOriented;
    public ObjectOriented playfieldOriented;
    public ControlOrient controlOrient;

    private Vector3 cameraVector;
    private Vector3 shipVector;

    [HideInInspector]
    public SoShipControllerParameters ShipSo;

    /*
    public float lAltitudeMin;
    public float lAltitudeMax;
    public float lDepthMin;
    public float lDepthMax;
    public float lLateralMin;
    public float lLateralMax;
    */

        /*
    public float Boss_LateralMin = -0.6f;
    public float Boss_LateralMax = 0.6f;
    public float Boss_AltitudeMax = -1.185f;
    public float Boss_DepthMin = -1;
    */

    // if need to test in realtime, make these public
    /*
    private float lateralMin;
    private float lateralMax;
    private float altitudeMax;
    private float depthMin;
    */

/*
    public float lateralMin;
    public float lateralMax;
    public float altitudeMax;
    public float depthMin;
    */

    public bool toggleLimits;
    private bool usingBossLimits;

    public float MoveSpeed = 1.0f;
    public GameObject ShipObject;
    public GameObject Camera;
    public Animator ShipAnimator;

    private string FireJetsAnimString = "";
    private string LeftRollAnimString = "";
    private string RightRollAnimString = "";
    private string FwdJetsAnimString = "";
    private string RevJetsAnimString = "";
    private string AboveJetsAnimString = "";
    private string BelowJetsAnimString = "";

    StarGameManager starGameManagerRef;
    private float workingSpeed;
    private Vector3 movementVector;
    private Vector3 previousShipVector;
    private float rangeCheck = 0.0001f;
  //  public Vector3 HeroBasePos;
    public Vector3 HeroBasePosBeingChased;
  //  [HideInInspector]
    public Vector3 usingBasePos;
    //public Vector3 HeroRebirthPos;
    public float RespawnSpeedMultiplier = 0.1f;
    public float ChasedRespawnSpeedMultiplier = 0.7f;
    [HideInInspector]
    public float usingRespawnSpeedMultiplier;

    private float northWestTerminator = 325f;  // was 314


    // public float watchingVector; // this is just temp


    /*
    public bool TestJetsBelow;
    public bool TestJetsAbove;
    public bool TestJetsForward;
    public bool TestJetsReverse;
    public bool TestJetsLeft;
    public bool TestJetsRight;
    */


    private bool fireJets;
    public UnityEvent ButtonAEvent;
    public UnityEvent ButtonBEvent;
    public UnityEvent ButtonXEvent;
    public UnityEvent ButtonYEvent;

    private string XbuttonString;
    private string YbuttonString;
    private string AbuttonString;
    private string BbuttonString;

    public bool canReceiveInput;
    [HideInInspector]
    public bool returnShipToPlayfield;
    public bool KeyboardInput;
    public float KeyboardMultiplier = 0.5f;



    public string[] KeyInputUp;
    public string[] KeyInputDown;
    public string[] KeyInputLeft;
    public string[] KeyInputRight;
    public string[] KeyInputAdvance;
    public string[] KeyInputRetreat;
    public string[] KeyInputFire;
    public string[] KeyInputPause;

    private bool autofireAlreadyTurnedOn;
    LocalSceneManagement localSceneManagementRef;

    void Start()
    {

        starGameManagerRef = StarGameManager.instance;

        if (starGameManagerRef == null)
        {
            workingSpeed = MoveSpeed;
        }

        FireJetsAnimString = "fireJets";
        LeftRollAnimString = "leftRoll";
        RightRollAnimString = "rightRoll";
        FwdJetsAnimString = "fwdJets";
        RevJetsAnimString = "revJets";
        AboveJetsAnimString = "aboveJets";
        BelowJetsAnimString = "belowJets";

        XbuttonString = "360_X";
        YbuttonString = "360_Y";
        AbuttonString = "360_A";
        BbuttonString = "360_B";

       // AssignStageLimits();
       // 
        usingRespawnSpeedMultiplier = RespawnSpeedMultiplier;

        if (localSceneManagementRef != null)
        {
            StartCoroutine(LoadFromSo());
        }
    }


    IEnumerator LoadFromSo()
        
    {
        yield return new WaitForSeconds(0.01f);
    //    usingBasePos = ShipSo.HeroBasePos;
    }

    IEnumerator Autofiring()
    {
        yield return new WaitForSeconds(0.3f);
        ButtonAEvent.Invoke();
        starGameManagerRef.ExtraBodiesFireProjectile();

        StartCoroutine(Autofiring());
    }
    
    public void ResetShipPosition(bool shouldBlink)
    {
        // ShipObject.GetComponent<Collider>().enabled = false;
        ShipAnimator.SetBool(RightRollAnimString, false);
        ShipAnimator.SetBool(LeftRollAnimString, false);
       // returnShipToPlayfield = true;

        if (shouldBlink)
        {
            // not using warp effect
            ShipObject.transform.localPosition = ShipSo.HeroRebirthPos;
            ShipAnimator.SetTrigger("blinkShip");
            returnShipToPlayfield = true;
        }
    }






    public void ShipCanReceiveInput(bool state)
    {
        canReceiveInput = state;
    }

    void CheckAPosition(float checkingVector, bool cameraCheck)
    {

        /*
        if (cameraCheck)
        {
            watchingVector = checkingVector;
        }
        */

        if ((checkingVector > northWestTerminator && checkingVector < 359) || (checkingVector > 0 && checkingVector < 45))

        {

            if (cameraCheck)
            {
                cameraOriented = ObjectOriented.North;
            }

            if (!cameraCheck)
            {
                playfieldOriented = ObjectOriented.West;
            }

        }

        if (checkingVector > 45 && checkingVector < 130)
        {

            if (cameraCheck)
            {
                cameraOriented = ObjectOriented.East;
            }

            if (!cameraCheck)
            {
                playfieldOriented = ObjectOriented.North;
            }

        }

        if (checkingVector > 130 && checkingVector < 220)
        {

            if (cameraCheck)
            {
                cameraOriented = ObjectOriented.South;
            }

            if (!cameraCheck)
            {
                playfieldOriented = ObjectOriented.East;
            }

        }

        if (checkingVector > 220 && checkingVector < northWestTerminator)
        {

            if (cameraCheck)
            {
                cameraOriented = ObjectOriented.West;
            }

            if (!cameraCheck)
            {
                playfieldOriented = ObjectOriented.South;
            }

        }
    }

    void CheckControlOrient()
    {
        if (cameraOriented == ObjectOriented.North && playfieldOriented == ObjectOriented.North)
            controlOrient = ControlOrient.Behind;

        if (cameraOriented == ObjectOriented.South && playfieldOriented == ObjectOriented.South)
            controlOrient = ControlOrient.Behind;

        if (cameraOriented == ObjectOriented.East && playfieldOriented == ObjectOriented.East)
            controlOrient = ControlOrient.Behind;

        if (cameraOriented == ObjectOriented.West && playfieldOriented == ObjectOriented.West)
            controlOrient = ControlOrient.Behind;

        //

        if (cameraOriented == ObjectOriented.North && playfieldOriented == ObjectOriented.South)
            controlOrient = ControlOrient.Ahead;

        if (cameraOriented == ObjectOriented.South && playfieldOriented == ObjectOriented.North)
            controlOrient = ControlOrient.Ahead;

        if (cameraOriented == ObjectOriented.East && playfieldOriented == ObjectOriented.West)
            controlOrient = ControlOrient.Ahead;

        if (cameraOriented == ObjectOriented.West && playfieldOriented == ObjectOriented.East)
            controlOrient = ControlOrient.Ahead;

        //

        if (cameraOriented == ObjectOriented.North && playfieldOriented == ObjectOriented.East)
            controlOrient = ControlOrient.GroundSide;

        if (cameraOriented == ObjectOriented.South && playfieldOriented == ObjectOriented.West)
            controlOrient = ControlOrient.GroundSide;

        if (cameraOriented == ObjectOriented.East && playfieldOriented == ObjectOriented.North)
            controlOrient = ControlOrient.WallSide;

        if (cameraOriented == ObjectOriented.West && playfieldOriented == ObjectOriented.South)
            controlOrient = ControlOrient.WallSide;

        //

        if (cameraOriented == ObjectOriented.North && playfieldOriented == ObjectOriented.West)
            controlOrient = ControlOrient.WallSide;

        if (cameraOriented == ObjectOriented.South && playfieldOriented == ObjectOriented.East)
            controlOrient = ControlOrient.WallSide;

        if (cameraOriented == ObjectOriented.East && playfieldOriented == ObjectOriented.South)
            controlOrient = ControlOrient.GroundSide;

        if (cameraOriented == ObjectOriented.West && playfieldOriented == ObjectOriented.North)
            controlOrient = ControlOrient.GroundSide;
    }


    IEnumerator ReturnShipVisbility(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShipAnimator.SetTrigger("visibleShip");
        ShipObject.GetComponent<Collider>().enabled = true;
    }
    
    void LocalSceneManagementChecker()
    {
        if (localSceneManagementRef == null)
        {
            starGameManagerRef.LocalSceneManagementChecker();
            if (starGameManagerRef.localSceneManager != null)
            {
                localSceneManagementRef = starGameManagerRef.localSceneManager.GetComponent<LocalSceneManagement>();
            }
        }
    }






    void StickChecks(bool useVectorx, float multiplierA, float multiplierB, float multiplierC)
    {
        if (useVectorx)
        {
            movementVector.x = Input.GetAxis("360_LeftJoystickX") * workingSpeed * multiplierA;  // lateral
        }
        else
        {
            movementVector.y = Input.GetAxis("360_LeftJoystickX") * workingSpeed * multiplierA;  // lateral
        }


        movementVector.z = Input.GetAxis("360_LeftJoystickY") * workingSpeed;  // altitude


        float newMovementVector = 0;
        bool sendZeroData = false;
        bool cancelRemainingChecks = false;


        if (Input.GetAxis("360_LB") != 0 || Input.GetAxis("360_TriggerLeft") != 0 ||
            Input.GetAxis("360_RightJoystickY") != 0)

        {
            if (Input.GetAxis("360_LB") != 0)
            {
                newMovementVector = Input.GetAxis("360_LB") * workingSpeed * multiplierB;  // DEPTH RETREAT
                cancelRemainingChecks = true;
            }

            if (Input.GetAxis("360_TriggerLeft") != 0 && !cancelRemainingChecks)
            {
                newMovementVector = Input.GetAxis("360_TriggerLeft") * workingSpeed * multiplierB;  // DEPTH RETREAT
                cancelRemainingChecks = true;
            }

            if (Input.GetAxis("360_RightJoystickY") != 0 && !cancelRemainingChecks)
            {
                newMovementVector = Input.GetAxis("360_RightJoystickY") * workingSpeed * multiplierB;  // DEPTH RETREAT
            }

        }

        else
        {
            if (Input.GetAxis("360_RB") != 0 || Input.GetAxis("360_TriggerRight") != 0)
            {
                if (Input.GetAxis("360_RB") != 0)
                {
                    newMovementVector = Input.GetAxis("360_RB") * workingSpeed * multiplierC; // DEPTH ADVANCE
                    cancelRemainingChecks = true;

                }

                if (Input.GetAxis("360_TriggerRight") != 0 && !cancelRemainingChecks)
                {
                    newMovementVector = Input.GetAxis("360_TriggerRight") * workingSpeed * multiplierC; // DEPTH ADVANCE
                }
            }
            else
            {
                sendZeroData = true;
                newMovementVector = 0;  // DEPTH STILL
            }
        }

        if (newMovementVector != 0 || sendZeroData == true)
        {
            if (useVectorx)
            {
                movementVector.y = newMovementVector;
            }
            else
            {
                movementVector.x = newMovementVector;
            }
        }

    }
    

    void Update()
    {

        if (localSceneManagementRef == null)
        {
            LocalSceneManagementChecker();

            if (localSceneManagementRef == null)
            {
                return;
            }
        }


        if (ShipObject == null)
        {
            LocalSceneManagementChecker();
            ShipObject = starGameManagerRef.HeroShip;
        }

        if (ShipAnimator == null)
        {
            LocalSceneManagementChecker();
            ShipAnimator = starGameManagerRef.heroshipAnimator;
        }

        if (Input.GetButtonDown(BbuttonString))
        {
            //used for pause currently
            ButtonBEvent.Invoke();
        }

        /*
        if (KeyboardInput)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))

            {
                ButtonBEvent.Invoke();
            }
        }

        */

        if (KeyboardInput)
        {
            for (int i = 0; i < KeyInputPause.Length; i++)
            {
                if (Input.GetKeyDown(KeyInputPause[i]))
                {
                    ButtonBEvent.Invoke();
                }
            }
        }

        if (starGameManagerRef.GamePaused)
        {
            return;
        }

        if (starGameManagerRef.DebugAutofire)
        {
            if (!autofireAlreadyTurnedOn)
            {
                autofireAlreadyTurnedOn = true;
                StartCoroutine(Autofiring());
            }
        }


        if (returnShipToPlayfield)
        {

            float shipPlayfieldThreshold = 0.01f;
            float resumeVisibilityDelay = 0.5f;

            if (starGameManagerRef.WarpOnEveryRespawn)
            {

                if (starGameManagerRef.currentHeroSpawnPoint == null)
                {
                 //   print("not returning until spawnPointPresent");
                    return;
                }

                ShipObject.transform.localPosition = starGameManagerRef.currentHeroSpawnPoint.transform.localPosition; 
                // errors out on finalBoss load, wait to do this until after gameplay begins  //rma200104
                resumeVisibilityDelay = 0.0f;
            }

            else
            {
                ShipObject.transform.localPosition = Vector3.MoveTowards(ShipObject.transform.localPosition, usingBasePos, usingRespawnSpeedMultiplier * Time.deltaTime);
            }

            
            if (Vector3.Distance(ShipObject.transform.localPosition, usingBasePos) < shipPlayfieldThreshold || starGameManagerRef.WarpOnEveryRespawn)
            {
                returnShipToPlayfield = false;
                ShipCanReceiveInput(true);
                StartCoroutine(ReturnShipVisbility(resumeVisibilityDelay));
                starGameManagerRef.ShadowYObject.SetActive(true);
            }
               return;
           }

        
               CheckAPosition(cameraVector.y, true);
               CheckAPosition(shipVector.y, false);
               CheckControlOrient();

               if (starGameManagerRef != null)
               {
                   workingSpeed = MoveSpeed * starGameManagerRef.GameSpeed;
               }

               cameraVector = Camera.transform.rotation.eulerAngles;

               if (ShipObject != null)
               {
                   shipVector = ShipObject.transform.rotation.eulerAngles;
               }

               if (Input.GetButtonDown(AbuttonString) && canReceiveInput)
               {
                   ButtonAEvent.Invoke();
                   starGameManagerRef.ExtraBodiesFireProjectile();

               }


               if (Input.GetButtonDown(XbuttonString))
               {
                   ButtonXEvent.Invoke();
               }

               if (Input.GetButtonDown(YbuttonString))
               {
                   ButtonYEvent.Invoke();
               }



               switch (controlOrient)
               {
                   case ControlOrient.Behind:
                       StickChecks(true, 1, -1, 1);
                       break;

                   case ControlOrient.Ahead:
                       StickChecks(true, -1, 1, -1);
                       break;

                   case ControlOrient.GroundSide:
                       StickChecks(false, 1, 1, -1);
                       break;

                   case ControlOrient.WallSide:
                       StickChecks(false, -1, -1, 1);
                       break;
               }


               if (KeyboardInput)
               {

                   for (int i = 0; i < KeyInputUp.Length; i++)
                   {
                       if (Input.GetKey(KeyInputUp[i]))
                       {
                           movementVector.z = movementVector.z - KeyboardMultiplier;
                       }
                   }

                   for (int i = 0; i < KeyInputDown.Length; i++)
                   {
                       if (Input.GetKey(KeyInputDown[i]))
                       {
                           movementVector.z = movementVector.z + KeyboardMultiplier;
                       }
                   }

                   for (int i = 0; i < KeyInputLeft.Length; i++)
                   {
                       if (Input.GetKey(KeyInputLeft[i]))
                       {
                           movementVector.x = movementVector.x - KeyboardMultiplier;
                       }
                   }

                   for (int i = 0; i < KeyInputRight.Length; i++)
                   {
                       if (Input.GetKey(KeyInputRight[i]))
                       {
                           movementVector.x = movementVector.x + KeyboardMultiplier;
                       }
                   }


                   for (int i = 0; i < KeyInputAdvance.Length; i++)
                   {
                       if (Input.GetKey(KeyInputAdvance[i]))
                       {
                           movementVector.y = movementVector.y + KeyboardMultiplier;
                       }
                   }


                   for (int i = 0; i < KeyInputRetreat.Length; i++)
                   {
                       if (Input.GetKey(KeyInputRetreat[i]))
                       {
                           movementVector.y = movementVector.y - KeyboardMultiplier;
                       }
                   }

                   for (int i = 0; i < KeyInputFire.Length; i++)
                   {
                       // (Input.GetKeyDown(KeyCode.Space))
                       if (KeyInputFire[i] == "")
                       {
                          if  (Input.GetKeyDown(KeyCode.Space))
                               {
                               ButtonAEvent.Invoke();
                           }
                       }
                       else
                       {
                           if (Input.GetKeyDown(KeyInputFire[i]))
                           {
                               ButtonAEvent.Invoke();
                           }
                       }
                   }





                   // ----------------------------------------
                   /*
                   if (Input.GetKey(KeyCode.A))

                   {
                       movementVector.x = movementVector.x - KeyboardMultiplier;
                   }


                   if (Input.GetKey(KeyCode.D))

                   {
                       movementVector.x = movementVector.x + KeyboardMultiplier;
                   }


                   if (Input.GetKey(KeyCode.Q))

                   {
                       movementVector.y = movementVector.y - KeyboardMultiplier;
                   }


                   if (Input.GetKey(KeyCode.E))

                   {
                       movementVector.y = movementVector.y + KeyboardMultiplier;
                   }

                   if (Input.GetKey(KeyCode.W))

                   {
                       movementVector.z = movementVector.z - KeyboardMultiplier;
                   }


                   if (Input.GetKey(KeyCode.S))

                   {
                       movementVector.z = movementVector.z + KeyboardMultiplier;
                   }

                   if (Input.GetKeyDown(KeyCode.Space))

                   {
                       ButtonAEvent.Invoke();
                   }

                   */
        }


        if (!canReceiveInput || ShipObject == null)
        {
            return;
        }

        previousShipVector = ShipObject.transform.localPosition;
        ShipObject.transform.localPosition += transform.localPosition = movementVector * Time.deltaTime;


        // check boundary limits

        if (ShipObject.transform.localPosition.x < ShipSo.LateralMin)
        {
            ShipObject.transform.localPosition = new Vector3(ShipSo.LateralMin, ShipObject.transform.localPosition.y, ShipObject.transform.localPosition.z);
        }

        if (ShipObject.transform.localPosition.x > ShipSo.LateralMax)
        {
            ShipObject.transform.localPosition = new Vector3(ShipSo.LateralMax, ShipObject.transform.localPosition.y, ShipObject.transform.localPosition.z);
        }

        if (ShipObject.transform.localPosition.y < ShipSo.DepthMin)
        {
            ShipObject.transform.localPosition = new Vector3(ShipObject.transform.localPosition.x, ShipSo.DepthMin, ShipObject.transform.localPosition.z);
        }

        if (ShipObject.transform.localPosition.y > ShipSo.DepthMax)
        {
            ShipObject.transform.localPosition = new Vector3(ShipObject.transform.localPosition.x, ShipSo.DepthMax, ShipObject.transform.localPosition.z);
        }

        if (ShipObject.transform.localPosition.z > ShipSo.AltitudeMin)
        {
            ShipObject.transform.localPosition = new Vector3(ShipObject.transform.localPosition.x, ShipObject.transform.localPosition.y, ShipSo.AltitudeMin);
        }

        if (ShipObject.transform.localPosition.z < ShipSo.AltitudeMax)
        {
            ShipObject.transform.localPosition = new Vector3(ShipObject.transform.localPosition.x, ShipObject.transform.localPosition.y, ShipSo.AltitudeMax);
        }


        // send bools to animator

        
        /*
        
        OLD
        x depth
        y altitude
        z lateral



        NOW
         x lateral
         y depth
         z altitude


         */


        // lateral

        if (previousShipVector.x < ShipObject.transform.localPosition.x - rangeCheck)

        {
            ShipAnimator.SetBool(RightRollAnimString, true);
            ShipAnimator.SetBool(LeftRollAnimString, false);
        }

        if (previousShipVector.x > ShipObject.transform.localPosition.x + rangeCheck)

        {
            ShipAnimator.SetBool(RightRollAnimString, false);
            ShipAnimator.SetBool(LeftRollAnimString, true);

        }

        // multiply by 100 to push up the decimal spaces, otherwise it's too imprecise and will register arbitrarily

        if (previousShipVector.x * 100 == ShipObject.transform.localPosition.x * 100)

        {
            ShipAnimator.SetBool(RightRollAnimString, false);
            ShipAnimator.SetBool(LeftRollAnimString, false);

        }


        // ---------------------------------------------------------------

        // depth

        if (previousShipVector.y < ShipObject.transform.localPosition.y - rangeCheck)

        {
            ShipAnimator.SetBool(RevJetsAnimString, false);
            ShipAnimator.SetBool(FwdJetsAnimString, true);

        }

        if (previousShipVector.y > ShipObject.transform.localPosition.y + rangeCheck)

        {
            ShipAnimator.SetBool(RevJetsAnimString, true);
            ShipAnimator.SetBool(FwdJetsAnimString, false);

        }

        // multiply by 100 to push up the decimal spaces, otherwise it's too imprecise and will register arbitrarily

        if (previousShipVector.y * 100 == ShipObject.transform.localPosition.y * 100)

        {
            ShipAnimator.SetBool(RevJetsAnimString, false);
            ShipAnimator.SetBool(FwdJetsAnimString, false);

        }

        // ---------------------------------------------------------------

        // altitude


        if (previousShipVector.z < ShipObject.transform.localPosition.z - rangeCheck)

        {
            ShipAnimator.SetBool(BelowJetsAnimString, false);
            ShipAnimator.SetBool(AboveJetsAnimString, true);

        }

        if (previousShipVector.z > ShipObject.transform.localPosition.z + rangeCheck)

        {
            ShipAnimator.SetBool(BelowJetsAnimString, true);
            ShipAnimator.SetBool(AboveJetsAnimString, false);

        }

        // multiply by 100 to push up the decimal spaces, otherwise it's too imprecise and will register arbitrarily

        if (previousShipVector.z * 100 == ShipObject.transform.localPosition.z * 100)

        {
            ShipAnimator.SetBool(BelowJetsAnimString, false);
            ShipAnimator.SetBool(AboveJetsAnimString, false);
        }

        if ((ShipAnimator.GetBool(LeftRollAnimString) == false) && (ShipAnimator.GetBool(RightRollAnimString) == false) &&
            (ShipAnimator.GetBool(FwdJetsAnimString) == false) && (ShipAnimator.GetBool(RevJetsAnimString) == false) &&
            (ShipAnimator.GetBool(AboveJetsAnimString) == false) && (ShipAnimator.GetBool(BelowJetsAnimString) == false))
        {
            ShipAnimator.SetBool(FireJetsAnimString, false);
        }

        else

        {
            ShipAnimator.SetInteger("enableIndex", Random.Range(0, 1));
            ShipAnimator.SetBool(FireJetsAnimString, true);
        }






















        // ABOVE STUFF IS NECESSARY - JUST COMMENTED OUT FOR TESTING

        // -------------------------------------------------------

        /*

        ShipAnimator.SetBool(BelowJetsAnimString, TestJetsBelow);
        ShipAnimator.SetBool(AboveJetsAnimString, TestJetsAbove);
        ShipAnimator.SetBool("fwdJets", TestJetsForward);
        ShipAnimator.SetBool("revJets", TestJetsReverse);
        ShipAnimator.SetBool("leftRoll", TestJetsLeft);
        ShipAnimator.SetBool("rightRoll", TestJetsRight);



        if (TestJetsAbove || TestJetsBelow || TestJetsForward || TestJetsReverse || TestJetsLeft || TestJetsRight)
        {
            ShipAnimator.SetBool("fireJets", true);
        }

        if (!TestJetsAbove && !TestJetsBelow && !TestJetsForward && !TestJetsLeft && !TestJetsRight & !TestJetsReverse)
        {
            ShipAnimator.SetBool("fireJets", false);
        }


    */

        /*
        if (toggleLimits)
        {
      
            if (!usingBossLimits)
            {
                SwapBossLimits();
            }

            else

            {
                AssignStageLimits();
            }

            usingBossLimits = !usingBossLimits;
            toggleLimits = false;


        }
        */
    } // END UPDATE

    /*
    void SwapBossLimits()
    {
        lateralMin = Boss_LateralMin;
        lateralMax = Boss_LateralMax;
        altitudeMax = Boss_AltitudeMax;
        depthMin = Boss_DepthMin;
    }
    */

    /*
    void AssignStageLimits()
    {
        lateralMin = ShipSo.LateralMin;
        lateralMax = ShipSo.LateralMax;
        altitudeMax = ShipSo.AltitudeMax;
        depthMin = ShipSo.DepthMin;
    }
    */

}
