using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCannon : MonoBehaviour
{

    public starEnemy StarEnemy;
    public GameObject RotateObjectPitch;
    public GameObject RotateObjectYaw;
    public GameObject YawOffsetObject;
    public Vector3 YawOffsetPos;
    public Vector3 YawOffsetRot;
    public bool InvertPitch;
    public bool FireProjectile;
   // public bool CanFireAtMaxPitchAngle = true;

    private StarGameManager starGameManagerRef;
    private Transform target;

    public float rotSpeed = 90f;
    public float correctionAngle;
   // public float maxright;
    //public float maxleft;
    private float workingSpeed = 99999.0f;
    public float PitchClampMin;
    public float PitchClampMax;

    // NOTE - finalBoss cannons don't use any of the FireProjectile calls in here to actually fire

    private void Start()
    {
        starGameManagerRef = StarGameManager.instance;

        if (starGameManagerRef.HeroShip != null)
        {
            target = starGameManagerRef.HeroShip.transform;
        }
        else
        {
            target = gameObject.transform;
        }


        if (YawOffsetObject != null)
        {
            YawOffsetObject.transform.localPosition = YawOffsetPos;
            YawOffsetObject.transform.localRotation = Quaternion.Euler(YawOffsetRot);
        }
    }


    public void TempFireProjectile()
    {
        FireProjectile = true;
    }
    
    private void Update()
    {

        if (starGameManagerRef.GamePaused)
        {
            return;
        }
        
        if (RotateObjectYaw != null)
        {
            Vector3 aim = target.transform.position;
            Vector3 dir = aim - RotateObjectYaw.transform.position;
            dir.Normalize();
            float yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + correctionAngle;
            //Debug.Log(yAngle);
            // Quaternion desiredRot = Quaternion.Euler(0, yAngle, 0);
            Quaternion desiredRot = Quaternion.Euler(0, yAngle, 0);
            //    RotateObjectYaw.transform.rotation = Quaternion.RotateTowards(RotateObjectYaw.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
            RotateObjectYaw.transform.rotation = Quaternion.RotateTowards(RotateObjectYaw.transform.rotation, desiredRot, workingSpeed * Time.deltaTime);

            workingSpeed = rotSpeed;
        }
        // -----------------------------------------------------

        if (RotateObjectPitch != null)
        {
            Vector3 relativePosPitch = target.position - RotateObjectPitch.transform.position;
            Quaternion rotationPitch = Quaternion.LookRotation(relativePosPitch);
            
            Vector3 useRotation = rotationPitch.eulerAngles;

            float NewAngle = rotationPitch.eulerAngles.x;
            NewAngle = (NewAngle > 180) ? NewAngle - 360 : NewAngle;

            if (PitchClampMax != 0 && PitchClampMin != 0)
            {
                NewAngle = Mathf.Clamp(NewAngle, PitchClampMin, PitchClampMax);
            }

            if (InvertPitch)
            {
                NewAngle = -NewAngle;
            }

            // RotateObjectPitch.transform.localRotation = Quaternion.Euler(NewAngle, 0,0); 
            // this snapped back after player death

            RotateObjectPitch.transform.localRotation = Quaternion.Slerp(RotateObjectPitch.transform.localRotation,
                Quaternion.Euler(NewAngle, 0, 0), Time.deltaTime * (rotSpeed / 8));
        }

        if (FireProjectile)
        {
            FireProjectile = false;
            if (StarEnemy != null)
            {
                StarEnemy.fireProjectile(starGameManagerRef.ActorsParent);
            }
        }
    }  // end update


}
