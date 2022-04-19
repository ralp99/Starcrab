using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolListMembership : MonoBehaviour
{

     public enum UsingList { ExplosionsEnemy, BulletsEnemy, BulletsHero, EnemyA, EnemyB, EnemyC, Tile, Special, Shadow }

    // For items like cannons, barriers, etc they need to be set to BOTH, 
    // otherwise they can be deactivated before they are activated (a common occurence)
    // and if not checking for OnEnable, they won't get put into the proper Active or Inactive list
    public enum ListActivity { None, OnEnable, OnDisable, Both}

    public UsingList usingList;
    public ListActivity listActivity;
    public int ItemLimit = 5;
    private List<GameObject> activeList;
    private List<GameObject> inactiveList;

    StarGameManager starGameManagerRef;
    [HideInInspector]
    public List<Transform> disableChildren;
    [HideInInspector]
    public List<Transform> unparentChildren;
    [HideInInspector]
    public GameObject PooledParent;

    // Dictionary<string, int> entityCounts = starGameManagerRef.EntityCounts;
    Dictionary<string, int> entityCounts;



    private void SelectListType()
    {
   
        switch (usingList)
        {

            case UsingList.ExplosionsEnemy:
                activeList = starGameManagerRef.explosionsPoolActive;
                inactiveList = starGameManagerRef.explosionsPoolInactive;
                break;

            case UsingList.BulletsEnemy:
                activeList = starGameManagerRef.EnemyBulletPoolActive;
                inactiveList = starGameManagerRef.EnemyBulletPoolInactive;
                break;

            case UsingList.BulletsHero:
                activeList = starGameManagerRef.HeroBulletPoolActive;
                inactiveList = starGameManagerRef.HeroBulletPoolInactive;
                break;

            case UsingList.EnemyA:
                activeList = starGameManagerRef.EnemyPoolActive;
                inactiveList = starGameManagerRef.EnemyPoolInactive;
                break;

            case UsingList.EnemyB:
                activeList = starGameManagerRef.EnemyPoolActive;
                inactiveList = starGameManagerRef.EnemyPoolInactive;
                break;

            case UsingList.EnemyC:
                activeList = starGameManagerRef.EnemyPoolActive;
                inactiveList = starGameManagerRef.EnemyPoolInactive;
                break;

            case UsingList.Tile:
                activeList = starGameManagerRef.TilePoolActive;
                inactiveList = starGameManagerRef.TilePoolInactive;
                break;

            case UsingList.Special:
                activeList = starGameManagerRef.SpecialPoolActive;
                inactiveList = starGameManagerRef.SpecialPoolInactive;
                break;
            case UsingList.Shadow:
                activeList = starGameManagerRef.ShadowPoolActive;
                inactiveList = starGameManagerRef.ShadowPoolInactive;
                break;
        }
    }

    void StarGameManagerChecker()
    {

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
       
            if (starGameManagerRef != null)
            {
                entityCounts = starGameManagerRef.EntityCounts;
                SelectListType();
            }
        }
    }
    
    private void Awake()
    {
        StarGameManagerChecker();  // this seems ok to do on awake
        // if it doesnt work, try onEnable instead
    }


    private void OnEnable()
    {
     
     //   StarGameManagerChecker();

        if (starGameManagerRef == null)
        {
            return;
        }

        if (!entityCounts.ContainsKey(gameObject.name))
        {
            starGameManagerRef.EntityCounters(gameObject);
        }
        

        List<Transform> itemsToAdd = new List<Transform>();
        List<Transform> itemsToRemove = new List<Transform>();

        foreach (Transform picked in unparentChildren)
        {
            if (picked != null)
            {
                if (picked.gameObject.activeSelf == false)
                {
                    itemsToAdd.Add(picked);
                }
            }
        }

        foreach (Transform picked in itemsToAdd)
        {
            picked.parent = null;
            unparentChildren.Remove(picked);
        }


        foreach (Transform picked in unparentChildren)
        {
            if (!disableChildren.Contains(picked) && unparentChildren.Contains(picked))
            {
               itemsToRemove.Add(picked);
            }
        }


        foreach (Transform picked in itemsToRemove)
        {
            if (unparentChildren.Contains(picked))
            {
                unparentChildren.Remove(picked);
            }
        }

        //maybe unecessary
        itemsToAdd.Clear();
        itemsToRemove.Clear();

        /*
        // moved further up
        if (starGameManagerRef == null)

        {
            starGameManagerRef = StarGameManager.instance;
            SelectListType();
        }
        */

        if (listActivity == ListActivity.OnEnable || listActivity == ListActivity.Both)
        {
            if (inactiveList.Contains(gameObject))
            {
                inactiveList.Remove(gameObject);
            }


            if (!activeList.Contains(gameObject))
            {
                activeList.Add(gameObject);
            }
        }
    }


    public void PooledChildWasDisabled(GameObject childGameObject)
    {
        // A tile's Feature object will tell its former parent to disregard it,
        // if it was previously detached (disabled/destroyed) before
        // the parent tile was. Example, cannons/popups

        if (disableChildren.Contains(childGameObject.transform))
        {
            disableChildren.Remove(childGameObject.transform);
        }
       
        if (unparentChildren.Contains(childGameObject.transform))
        {
            unparentChildren.Remove(childGameObject.transform);
        }

    }
    

     


    private void OnDisable()
    {

        if (starGameManagerRef == null)
        {
            return;
        }

        string objectName = this.name;

        int currentValue = starGameManagerRef.EntityCounts[objectName];
        starGameManagerRef.EntityCounts[objectName] = currentValue - 1;
      
        // This will currently crash if an object isntantiated without going through spawnChecker,
        // as there will be no dictionary entry created for it's Entity Counter/limit watcher



        if (disableChildren.Count > 0)

        {
            foreach (Transform child in disableChildren)

            {
                child.gameObject.SetActive(false);
            }

            disableChildren.Clear();
        }

        if (listActivity == ListActivity.OnDisable || listActivity == ListActivity.Both)

        {
            if (activeList.Contains(gameObject))
            {
                activeList.Remove(gameObject);
            }

            if (!inactiveList.Contains(gameObject))
            {
                inactiveList.Add(gameObject);
             }
        }
    }

    


}
