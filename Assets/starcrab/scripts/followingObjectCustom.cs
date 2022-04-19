using UnityEngine;
using System.Collections;

public class followingObjectCustom : MonoBehaviour
{

    public GameObject watchThis;
    public Transform patrolPoint;
    public GameObject movingObject;
    public float moveSpeed;
    float useSpeed;
    public bool overrideSpeed;
    int currentPoint;
    public bool closeOnFinish;
    public Behaviour disableMonoOnFinish;
    public float offsetX, offsetY, offsetZ;
    Vector3 newGoalPos;
    Vector3 newGoalRot;
    public bool useLocalPos;
    public float waitTime;
    public float threshold;

   // public bool position;
   // public bool lookTowards;
    //public bool scale;

    bool canGo;
    bool canCount;
    public bool waitAgain;
    Vector3 followingPosition;
    Vector3 followingRotation;

    Vector3 currentPos;
    Vector3 currentRot;

    ValueStore valueStore;

    void Start()
    {

        //movingObject.transform.position = patrolPoint.position;
        //print (patrolPoint.position);

        if ((overrideSpeed) || (watchThis == null)) useSpeed = moveSpeed;
        else moveSpeed = watchThis.GetComponent<ValueStore>().scrollSpeed;

        if (useLocalPos)
        followingPosition = patrolPoint.transform.localPosition;
        else followingPosition = patrolPoint.transform.position;
        followingRotation = patrolPoint.transform.localEulerAngles;
        canCount = true;
       

    }


    void Update()
    {

    //    print("initial goal "+initialGoal);
    //    print("following Position " + followingPosition);
     //   print("new goal " + newGoalPos);


        if (waitTime > 0)
        {
            if ((!canGo)&&(canCount)) StartCoroutine(TimedSelect());
        }
        else canGo = true;


        if (canCount)
        { 
        if (canGo)
        {


                float newRotX = patrolPoint.localRotation.x;
                float newRotY = patrolPoint.localRotation.y;
                float newRotZ = patrolPoint.localRotation.z;



                float newPosX = patrolPoint.position.x + offsetX;
                float newPosY = patrolPoint.position.y + offsetY;
                  float newPosZ = patrolPoint.position.z + offsetZ;

            if (useLocalPos)
            {
                newPosX = patrolPoint.localPosition.x + offsetX;
                newPosY = patrolPoint.localPosition.y + offsetY;
                newPosZ = patrolPoint.localPosition.z + offsetZ;
            }


            newGoalPos = new Vector3(newPosX, newPosY, newPosZ);
            newGoalRot = new Vector3(newRotX, newRotY, newRotZ);

            if (useLocalPos) currentPos = movingObject.transform.localPosition;
            else currentPos = movingObject.transform.position;


                //   if ((currentPos == newGoalPos)&&(currentRot == newGoalRot))

                //--------------------------------------------------------------

                // if ((movingObject.transform.position == patrolPoint.position) || (Vector3.Distance(movingObject.transform.position, patrolPoint.position) < threshold))


                //     if (currentPos == newGoalPos)

                //   if ((currentPos == newGoalPos)||(currentPos <= threshold))
                if ((currentPos == newGoalPos) || (Vector3.Distance(currentPos, newGoalPos) < threshold))



                {
                    //  print("AT FINISH POS");
                    if (closeOnFinish) gameObject.SetActive(false);

                    canCount = false;
                    StopAllCoroutines();
                    if (disableMonoOnFinish != null)
                    {
                        canCount = true;
                        canGo = false;
                        disableMonoOnFinish.enabled = false;
                    }

                }


                if (canGo)

                {
                    movingObject.transform.localPosition = Vector3.MoveTowards(movingObject.transform.localPosition, newGoalPos, moveSpeed * Time.deltaTime);
           
                    //if (lookTowards)

                    //{
                    //    print("should rotate");
                      
                    //    print("patrolPoint.positon " + patrolPoint.position);
                    //    print(" movingObject.transform.position " + movingObject.transform.position);
                    //    print("diff " + (patrolPoint.position - movingObject.transform.position));



                    //     var q = Quaternion.LookRotation(patrolPoint.position - movingObject.transform.position);
     
                    //      movingObject.transform.rotation = Quaternion.RotateTowards(movingObject.transform.rotation, q, moveSpeed * Time.deltaTime);


                    //}



                }
                //   else print("CANNOT GO");
            } // END canGo


    }

        if (!canCount)
        {
            if (currentPos == newGoalPos)
            {
              //  print("arrived");
            followingPosition = patrolPoint.transform.position;


                if (waitAgain)
                {
                    if (newGoalPos != followingPosition)
                    {

                        StopAllCoroutines();

                        canGo = false;
                        canCount = true;
                        //  print("shouldn't follow");
                    //    print("NEW START POS");

                        followingPosition = newGoalPos;
                    }
                    else canCount = false;
                }

            }

        } // END canCount FALSE

    }


    private IEnumerator TimedSelect()

    {
     //   print("COUNTING NOW");
        yield return new WaitForSeconds(waitTime);
       // print("OK GOOD");
        canCount = canGo = true;
    }



}
