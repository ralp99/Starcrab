using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinalController : BossControllerGeneric
{

    enum ExtendDoorPosition { Closed, Open, Cannon1, Cannon2, Cannon3, Cannon4 };

    public GameObject[] DummyDoorsA;
    public GameObject[] DummyDoorsB;
    public GameObject[] DummyDoorsC;
    public GameObject[] DummyExteriorGuns;

    public GameObject SourceDoorsA;
    public GameObject SourceDoorsB;
    public GameObject SourceDoorsC;
    public GameObject SourceExteriorGuns;
    public Transform DoorScaleTransformSource;

    //public GameObject ArmExtensionCannons;

    public Animator Animator;

    public float ArmDoorRetractWait = 0.35f;
    public float DelayBetweenDoorsOpen = 1.0f;
    public float DelayBetweenDoorsOpenJitter = 1.0f;
    private float DelayBetweenDoorsOpenUse;

    public float doorOpenTimeArm = 7.0f;
    public float doorOpenTimeJitterArm = 7.0f;
    public float doorOpenTimeGreen = 3.0f;
    public float doorOpenTimeJitterGreen = 3.0f;
    public float doorOpenTimePurple = 5.0f;
    public float doorOpenTimeJitterPurple = 5.0f;
    private float doorOpenTimeArmUse;
    private float doorOpenTimeGreenUse;
    private float doorOpenTimePurpleUse;
  

    List<GameObject> AllDoors = new List<GameObject>();
    public  List<GameObject> ArmExtendDoors = new List<GameObject>();
    public  List<GameObject> PurpleSpawnDoors = new List<GameObject>();
    public List<GameObject> GreenSpawnDoors = new List<GameObject>();


    Dictionary<GameObject, ExtendDoorPosition> doorOpenBoolDict =
        new Dictionary<GameObject, ExtendDoorPosition>();


    /// <summary>
    ///  debug random door controller
    /// </summary>
    public bool DebugForceArmDoorRandom;
    public bool DebugForcePurpleDoorRandom;
    public bool DebugForceGreenDoorRandom;


    public void setBool ()
        {
        // DebugForceDoorRandom = true;
       // RandomDoorSelect();
        }

    public void ShuffleDoorTimes()
    {
        doorOpenTimeArmUse = doorOpenTimeArm + Random.Range(0, doorOpenTimeJitterArm);
        doorOpenTimeGreenUse = doorOpenTimeGreen + Random.Range(0, doorOpenTimeJitterGreen);
        doorOpenTimePurpleUse = doorOpenTimePurple + Random.Range(0, doorOpenTimeJitterPurple);
        DelayBetweenDoorsOpenUse = DelayBetweenDoorsOpen + Random.Range(0, DelayBetweenDoorsOpenJitter);
    }



    IEnumerator RandomDoorController()
    {
        ShuffleDoorTimes();
        yield return new WaitForSeconds(DelayBetweenDoorsOpenUse);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

       List<GameObject> doorTypeList = GreenSpawnDoors;
       int selectDoorType = Random.Range(0, 3);

        if (selectDoorType == 0)
        {
            ArmDoorControl();
        }

        else
        {

            if (selectDoorType == 1)
            {
                doorTypeList = PurpleSpawnDoors;
            }

            GameObject randomDoor = (doorTypeList[Random.Range(0, doorTypeList.Count)]);
            StartCoroutine(DoorOpening(randomDoor, ""));

            // tell it to spawn as well, either here or in it's sub-controller
            starEnemy currentStarEnemy = randomDoor.GetComponentInChildren<starEnemy>();
            currentStarEnemy.ProjectileFiringEnabled = true;
            currentStarEnemy.fireProjectile(null);

            StartCoroutine(CloseDoorTimer(randomDoor));
        }

        StartCoroutine(RandomDoorController());
    }

    IEnumerator CloseDoorTimer (GameObject selectedDoor)
    {
        ShuffleDoorTimes();
        float delayTime = 0.0f;

        if (ArmExtendDoors.Contains (selectedDoor))
        {
            delayTime = doorOpenTimeArmUse;
        }

        if (GreenSpawnDoors.Contains(selectedDoor))
        {
            delayTime = doorOpenTimeGreenUse;
        }

        if (PurpleSpawnDoors.Contains(selectedDoor))
        {
            delayTime = doorOpenTimePurpleUse;
        }

        yield return new WaitForSeconds(delayTime);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        Animator doorAnimator = selectedDoor.GetComponent<Animator>();
        doorAnimator.SetBool("closed",true);
        doorAnimator.SetTrigger("door");
    }


   
    public void ArmDoorControl()
      {
           GameObject selectedDoor = ArmExtendDoors[Random.Range(0, ArmExtendDoors.Count)];

          int randomEnumValue = Random.Range(0, System.Enum.GetValues(typeof(ExtendDoorPosition)).Length);

          ExtendDoorPosition randomExtendDoorPosition = (ExtendDoorPosition)randomEnumValue;

          Animator animator = selectedDoor.GetComponent<Animator>();
          string doTrigger = "";

          switch (randomExtendDoorPosition)
          {
              case ExtendDoorPosition.Closed:
                  if (doorOpenBoolDict[selectedDoor] != ExtendDoorPosition.Closed)
                  {
                    StartCoroutine(ArmDoorPreClose(selectedDoor));
                  }
                break;
              case ExtendDoorPosition.Open:
                  break;
              case ExtendDoorPosition.Cannon1:
                  doTrigger = "cannon1";
                  break;
              case ExtendDoorPosition.Cannon2:
                  doTrigger = "cannon2";
                  break;
              case ExtendDoorPosition.Cannon3:
                  doTrigger = "cannon3";
                  break;
              case ExtendDoorPosition.Cannon4:
                  doTrigger = "cannon4";
                  break;
              default:
                  break;
          }


          if (randomExtendDoorPosition != ExtendDoorPosition.Open)
          {
              if (randomExtendDoorPosition == ExtendDoorPosition.Closed || doorOpenBoolDict[selectedDoor] == randomExtendDoorPosition)
              {
                  return;
              }

              if (doorOpenBoolDict[selectedDoor] == ExtendDoorPosition.Closed)  // if door was previously closed
              {
                  doorOpenBoolDict[selectedDoor] = randomExtendDoorPosition;
                  StartCoroutine(DoorOpening(selectedDoor, doTrigger));
              }
              else
              {
                  doorOpenBoolDict[selectedDoor] = randomExtendDoorPosition;
                  animator.SetTrigger(doTrigger);
              }

            StartCoroutine(ArmDoorPreClose(selectedDoor));

          }
          else
          {
              ArmDoorControl(); // reshuffle since there's no simple "open state" for Arm doors
          }
      }

    

    IEnumerator DoorOpening(GameObject currentDoor, string armTrigger)
    {
        float openDelay = 0.0f;
        if (armTrigger == "")
        {
            openDelay = ArmDoorRetractWait;
        }
        Animator animator = currentDoor.GetComponent<Animator>();
        animator.SetBool("closed", false);
        animator.SetTrigger("door");

        yield return new WaitForSeconds(openDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        if (armTrigger != "")
        {
            animator.SetTrigger(armTrigger);
        }
    }


    IEnumerator ArmDoorPreClose(GameObject currentDoor)
    {

        yield return new WaitForSeconds(doorOpenTimeArmUse);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        StartCoroutine(DoorClosing(currentDoor, ArmDoorRetractWait));

    }

    IEnumerator DoorClosing (GameObject currentDoor, float delayTimer)
    {
        doorOpenBoolDict[currentDoor] = ExtendDoorPosition.Closed;

        Animator animator = currentDoor.GetComponent<Animator>();

        animator.SetBool("closed", true);
        animator.SetTrigger("cannon0");

        yield return new WaitForSeconds(delayTimer);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }
        animator.SetTrigger("door");
        
    }
    
    void Start () {

        //   PopulateDummies(DummyDoorsA, SourceDoorsA, gameObject, false, true, ArmExtendDoors);
        PopulateDummies(DummyDoorsA, SourceDoorsA, gameObject, false, true, DoorScaleTransformSource, ArmExtendDoors);
        PopulateDummies(DummyDoorsB, SourceDoorsB, gameObject, false, true, DoorScaleTransformSource, PurpleSpawnDoors);
        PopulateDummies(DummyDoorsC, SourceDoorsC, gameObject, false, true, DoorScaleTransformSource, GreenSpawnDoors);
        //  PopulateDummies(DummyExteriorGuns, SourceExteriorGuns, gameObject, false);

        foreach(GameObject picked in ActiveBodies)
        {
            AllDoors.Add(picked);
            picked.SetActive(true);
        }

        foreach (GameObject pickedDoor in AllDoors)
        {
            doorOpenBoolDict.Add(pickedDoor, ExtendDoorPosition.Closed);
        }

        StartCoroutine(RandomDoorController());

    }


    void RemoveDoorFromDoorList(List<GameObject> doorList, GameObject removingObject)
    {
        if (doorList.Contains(removingObject))
        {
            doorList.Remove(removingObject);
        }
    }

    

    void Update () {

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



        if (DisabledSubObjects.Count != 0)
        {

            List<GameObject> removeObjectList = new List<GameObject>();
            foreach (GameObject picked in DisabledSubObjects)
            {
                removeObjectList.Add(picked);
                GameObject currentSubObject = picked;
                ActiveBodiesUpdater(picked);
            }

            foreach (GameObject picked in removeObjectList)
            {
                DisabledSubObjects.Remove(picked);
            }

            foreach (GameObject picked in removeObjectList)
            {
                RemoveDoorFromDoorList(ArmExtendDoors, picked);
                RemoveDoorFromDoorList(PurpleSpawnDoors, picked);
                RemoveDoorFromDoorList(GreenSpawnDoors, picked);
            }
        }


        // ----------------------------------


        if (DebugForceArmDoorRandom)
        {
            DebugForceArmDoorRandom = false;
         //   DoorControl(ArmExtendDoors);
        }

        if (DebugForceGreenDoorRandom)
        {
            DebugForceGreenDoorRandom = false;
        //    DoorControl(GreenSpawnDoors);
        }


        if (DebugForcePurpleDoorRandom)
        {
            DebugForcePurpleDoorRandom = false;
        //    DoorControl(PurpleSpawnDoors);
        }

      //  if (DisabledSubObjects.Count == ActiveBodies.Count)
      //  {
           // print("phase 1 boss defeated - final boss");
      //  }

	}


  


}
