using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBulletType : MonoBehaviour
{
    StarGameManager starGameManagerRef;
    public GameObject[] weapons;
    int useID;

    //void OnEnable()
    //{
    //    if (starGameManagerRef == null)
    //        starGameManagerRef = StarGameManager.instance;

    //    foreach (GameObject weapon in weapons)
    //    {
    //        weapon.SetActive(false);
    //    }

    //    for (int i = 0; i < starGameManagerRef.weaponInventoryList.Count; i++)
    //    {
    //        if (starGameManagerRef.weaponInventoryList[i] == starGameManagerRef.currentWeapon)
    //        {
    //            print("i is "+i);
    //            print("master list is  "+starGameManagerRef.weaponInventoryList[i]);
    //            print("bullet is "+weapons[i]);
    //            weapons[i].SetActive(true);
    //            break;
    //        }

    //    }

    //}

    void OnEnable()
    {

           if (starGameManagerRef == null)
          starGameManagerRef = StarGameManager.instance;
           
        switch (starGameManagerRef.currentWeapon)
        {

            case "01_normal":
                useID = 0;
                break;

            case "02_shotgun":
                useID = 1;
                break;

            case "03_swirl":
                useID = 2;
                break;

            case "04_box":
                useID = 3;
                break;

            case "05_spread":
                useID = 4;
                break;

            case "06_side":
                useID = 5;
                break;

            case "07_vertical":
                useID = 6;
                break;

            case "08_ball":
                useID = 7;
                break;

            case "09_drill":
                useID = 8;
                break;

            case "10_scatter":
                useID = 9;
                break;

            case "11_lockon":
                useID = 10;
                break;

            case "12_charge":
                useID = 11;
                break;

            case "13_heatseeker":
                useID = 12;
                break;
                
        }



        weapons[useID].SetActive(true);
             //    print("bullet is "+weapons[useID]);


    }


    void OnDisable()
    {

           foreach (GameObject weapon in weapons)
           {
                weapon.SetActive(false);
          }

    }





}
