using UnityEngine;
using System.Collections;

public class distanceComparenew : MonoBehaviour {

	public GameObject objectA;
	public GameObject objectB;
	public bool printDistance;
	public float distanceReadout;
    public bool AGreaterReadout;
    public bool BGreaterReadout;
    //public string taggedB = "";
    GameObject[] taggedObject;
	public GameObject[] enableInRange;
	public MonoBehaviour[] enableMonoInRange;
	public GameObject[] disableInRange;
    public MonoBehaviour[] disableMonoInRange;
	public GameObject[] enableOutOfRange;
	public MonoBehaviour[] enableMonoOutOfRange;
	public GameObject[] disableOutOfRange;
	public float range;
	float distanceDifferenceObjects;
	float distanceDifferenceTagged;
	bool withinRange;
    public bool greaterA;
    public bool greaterB;
    public bool useWorldSpace;

    float dax, day, daz, dbx, dby, dbz;


     bool PrintCheck()

    {
        if (withinRange) return true;
        else return false;
    }

    
    void Update () {

		Transform distanceA = objectA.transform;
		Transform distanceB = objectB.transform;

        //    print("distanceA "+distanceA.position);
        //    print("distanceB " + distanceB.position);

        

      

        if (useWorldSpace)
        {
            dax = distanceA.position.x;
            day = distanceA.position.y;
            daz = distanceA.position.z;
            dbx = distanceB.position.x;
            dby = distanceB.position.y;
            dbz = distanceB.position.z;
        }

        else
        {
            dax = distanceA.localPosition.x;
            day = distanceA.localPosition.y;
            daz = distanceA.localPosition.z;
            dbx = distanceB.localPosition.x;
            dby = distanceB.localPosition.y;
            dbz = distanceB.localPosition.z;
        }



        if (useWorldSpace) distanceDifferenceObjects = Vector3.Distance(distanceA.position, distanceB.position);
        else distanceDifferenceObjects = Vector3.Distance(distanceA.localPosition, distanceB.localPosition);



        if (printDistance)
		{
            distanceReadout = distanceDifferenceObjects;
		}

        if ((greaterA == false) && (greaterB == false))
        {
            if (distanceDifferenceObjects > range)
                withinRange = false;
            if (distanceDifferenceObjects <= range)
                withinRange = true;
        }

        else

        {


         //   print("dax " + dax + " ||||||| dbx " + dbx);
         //   print("day " + day + " ||||||| dby " + dby);
           // print("daz " + daz + " ||||||| dbz " + dbz);

            withinRange = false;

            if (greaterA)
            {

                if ((dax >= dbx) && (day >= dby) && (daz >= dbz))

                {
                    if ((dax > dbx) || (day > dby) || (daz > dbz)) withinRange = true;

                }

                if (printDistance)

                {
                    if ((dax == dbx) && (day == dby) && (daz == dbz)) AGreaterReadout = BGreaterReadout = false;
                    else
                    {
                        AGreaterReadout = PrintCheck();
                        BGreaterReadout = !(PrintCheck());
                    }

                }

            }
            

            else if (greaterB)
            {
                if ((dbx >= dax) && (dby >= day) && (dbz >= daz))
                {
                    if ((dbx > dax) || (dby > day) || (dbz > daz)) withinRange = true;
                }

                if (printDistance)

                {
                  
                    if ((dax == dbx) && (day == dby) && (daz == dbz)) AGreaterReadout = BGreaterReadout = false;
                    else
                    {
                        BGreaterReadout = PrintCheck();
                        AGreaterReadout = !(PrintCheck());
                    }

                }

            }

        }


        if (withinRange) {
            

            if (enableInRange != null)
            {

                foreach (GameObject picked in enableInRange)
                {
                    picked.SetActive(true);
                }


                foreach (MonoBehaviour picked in enableMonoInRange)
                {
                    picked.enabled = true;
                }
            }

            if (disableInRange != null)
            {
                foreach (GameObject picked in disableInRange)
                {
                    picked.SetActive(false);
                }
            }

            if (disableMonoInRange != null)
            {
                foreach (MonoBehaviour picked in disableMonoInRange)
                {
                    picked.enabled = false;
                }
            }
            

            // END WITHIN RANGE
        }

        else

        {

            if (enableOutOfRange != null)
            { 

            foreach (GameObject picked in enableOutOfRange)
            {
                picked.SetActive(true);
            }

            foreach (MonoBehaviour picked in enableMonoOutOfRange)
            {
                picked.enabled = true;
            }
        }


            if (disableOutOfRange != null)
            {
                foreach (GameObject picked in disableOutOfRange)
                {
                    picked.SetActive(false);
                }
            }
            
        }
    }

}
