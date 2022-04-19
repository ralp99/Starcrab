using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDirectionChecker : MonoBehaviour {

    public enum SensorDirection { UpAngle, DownAngle, Straight, Left, Right}
    public starEnemy StarEnemy;
    public string CheckTag = "";
    public float distance = 0.0f;
    public bool AngleBlocked;
    [Space]
	public SensorDirection CurrentSensorDir;
    [Space]
    public bool incrementSensorPositionManual;
    public int currentIncrement;  //makeprivate
    public float IncrementWaitTime = 0.02f;
    public float ReleaseBlocksWaitTime = 0.4f;
    private IEnumerator lastCoroutine;
    private IEnumerator lastUnblockCoroutine;

    //[HideInInspector]
    public bool BlockedStraight, BlockedAboveAngle, BlockedBelowAngle, BlockedLeft, BlockedRight;

    public GameObject StraightSensor, AboveSensor, BelowSensor, LeftSensor, RightSensor;






    /*
    __________________________________________________________________________________


   [System.Serializable]

   public class instanceList

   {
      public SensorDirection sensorDirection;
      public Vector3 pos;
      public Vector3 rot;
      public float DistanceCheck;
   }


   public List<instanceList> InstanceList;


   private void OnEnable()
   {
       UnblockAllAngles(true);
       currentIncrement = 1;
       AngleBlocked = false;
       CurrentSensorDir = SensorDirection.Straight;

       if (lastUnblockCoroutine != null)
       {
           StopCoroutine(lastUnblockCoroutine);
           lastUnblockCoroutine = null;
       }

       if (lastCoroutine != null)
       {
           StopCoroutine(lastCoroutine);
           lastCoroutine = null;
       }

   }

   private void IncrementThroughRaycastPositions()
   {

       gameObject.transform.localPosition = InstanceList[currentIncrement].pos;
       gameObject.transform.localEulerAngles = InstanceList[currentIncrement].rot;
       distance = InstanceList[currentIncrement].DistanceCheck;

       CurrentSensorDir = InstanceList [currentIncrement].sensorDirection;
       currentIncrement++;

       if (currentIncrement == InstanceList.Count)
       {
           currentIncrement = 0;
       }
   }


   private void BlockedCheck(SensorDirection sensorDirection, bool state)

   {
       switch (sensorDirection)
       {
           case SensorDirection.UpAngle:
               BlockedAboveAngle = state;
               break;

           case SensorDirection.DownAngle:
               BlockedBelowAngle = state;
               break;

           case SensorDirection.Straight:
               BlockedStraight = state;
               break;

           case SensorDirection.Left:
               BlockedLeft = state;
               break;

           case SensorDirection.Right:
               BlockedRight = state;
               break;

           default:
               break;
       }
   }


   private IEnumerator SwitchingSensorDirection()
{
       yield return new WaitForSeconds(IncrementWaitTime);
       IncrementThroughRaycastPositions();
       lastCoroutine = null;
}
*/
    /*
    private IEnumerator UnblockAllAnglesTimer()
    {
        yield return new WaitForSeconds(ReleaseBlocksWaitTime);  // 0.2
        UnblockAllAngles();
        lastUnblockCoroutine = null;
    }
    */
    /*
    private void UnblockAllAngles(bool unblockStraight)
    {
        BlockedAboveAngle = false;
        BlockedBelowAngle = false;
        BlockedLeft = false;
        BlockedRight = false;

        if (unblockStraight)
        {
            BlockedStraight = false;
        }
    }
    

    void ShootRaycast()
    {
        print("RAY CASTING");
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, distance);

        if (hit.collider != null && hit.collider.tag == CheckTag)

        {
            AngleBlocked = true;
        }

        else

        {
            AngleBlocked = false;
        }


        foreach (instanceList inList in InstanceList)
        {

            if (inList.sensorDirection == CurrentSensorDir)
            {
                BlockedCheck(inList.sensorDirection, AngleBlocked);
            }
        }
    }

    private void Update()
    {

        if (lastCoroutine == null)
        {
            if (BlockedStraight || CurrentSensorDir != SensorDirection.Straight)

            {
                lastCoroutine = SwitchingSensorDirection();
                StartCoroutine(lastCoroutine);
            }

            ShootRaycast();  // do this much less often
        }
        
      
        if (incrementSensorPositionManual)
        {
            incrementSensorPositionManual = false;
            IncrementThroughRaycastPositions();
        }
        */
    /*
    if (!BlockedStraight)
    {
        if (lastUnblockCoroutine == null)
        {
            lastUnblockCoroutine = UnblockAllAnglesTimer();
            StartCoroutine(lastUnblockCoroutine);
        }
    }

    else

    {
        if (lastUnblockCoroutine != null)
        {
            StopCoroutine(lastUnblockCoroutine);
        }

        lastUnblockCoroutine = null;
    }
    */

    // }
}
