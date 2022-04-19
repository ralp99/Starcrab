using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LocalSceneManagement : MonoBehaviour {

    public GameObject ShipSpawnPoint;
    public Vector3 SpawnShipPos;

    public GameObject ActorsParent;
    public GameObject TileHolder;
    public GameObject DrawVolume;
    public GameObject DrawVolumeDebug;
    public GameObject BombEffect; // should be prefab
    public GameObject StageSizeObject;
    public Rigidbody[] SleepPhysicsOnPause;

    [Header("Gestures UI")]
    public GameObject GestureScaleObject;
    public GameObject GestureMoveRotObject;
    public TapToPlaceParent FinalBossPlacementObject;
    public Animator FinalBossHolderAnimator;
    public Animator PerimeterBoxOutlineAnim;
    public string FinalBossGoAwayTrigger = "";
    public string FinalBossReturnTrigger = "";
    public float TerminatorDistance;
    public bool ActivateElements;
    public bool BeginManagerGameplaySettingsAtActivate;
    // public bool DisableThisAfterActivate;
    public bool BeginGameplayAtSceneLoad;
    public GameObject BossPlacementObject;


    public GameObject[] ElementsToActivate;
    public SoShipControllerParameters ShipControllerSo;
    public List<SoTileMoverTileGroup> TileGroups;
    private StarGameManager starGameManagerRef;
    public bool LoadHeroShipLateStart = true;
    public bool ForceLoadAltSceneOnRun = false;
    public int AltLoadScene = 0;
    public HeroSpawnPoint heroSpawnPoint;

    // public HeroSpawnPoint heroSpawnPoint;

    private void Start()
    {
        /*
        starGameManagerRef = StarGameManager.instance;
        starGameManagerRef.TerminatorDistance = TerminatorDistance;
        starGameManagerRef.starShipController.ShipSo = ShipControllerSo;
        starGameManagerRef.TileHolderObject = TileHolder;
        starGameManagerRef.DrawVolume = DrawVolume;
        starGameManagerRef.DrawVolumeDebug = DrawVolumeDebug;
        starGameManagerRef.BombEffect = BombEffect;
        starGameManagerRef.StageSizeObject = StageSizeObject;
        starGameManagerRef.ExtraBody = ExtraBody;
        starGameManagerRef.ShipExplosion = ShipExplosion;

        starGameManagerRef.UiManager.GestureScaleObject = GestureScaleObject;
        starGameManagerRef.UiManager.GestureMoveRotObject = GestureMoveRotObject;
        starGameManagerRef.UiManager.PerimeterBoxOutlineAnim = PerimeterBoxOutlineAnim;
        */


        if (StarGameManager.instance)
        {
            starGameManagerRef = StarGameManager.instance;
            starGameManagerRef.TerminatorDistance = TerminatorDistance;
        }

        if (ForceLoadAltSceneOnRun && starGameManagerRef == null)
        {
            SceneManager.LoadScene(AltLoadScene);
            return;
        }

            if (ActorsParent != null)
        {
            starGameManagerRef.ActorsParent = ActorsParent;
        }

        if (ShipControllerSo != null)
        {
            starGameManagerRef.starShipController.ShipSo = ShipControllerSo;
        }

        if (TileHolder != null)
        {
            starGameManagerRef.TileHolderObject = TileHolder;
        }

        if (DrawVolume != null)
        {
            starGameManagerRef.DrawVolume = DrawVolume;
        }

        if (DrawVolumeDebug != null)
        {
            starGameManagerRef.DrawVolumeDebug = DrawVolumeDebug;
        }

        if (BombEffect != null)
        {
            starGameManagerRef.BombEffect = BombEffect;
        }

        if (StageSizeObject != null)
        {
            starGameManagerRef.StageSizeObject = StageSizeObject;
        }

        if (GestureScaleObject != null)
        {
            starGameManagerRef.UiManager.GestureScaleObject = GestureScaleObject;
        }

        if (GestureMoveRotObject != null)
        {
            starGameManagerRef.UiManager.GestureMoveRotObject = GestureMoveRotObject;
        }

        if (PerimeterBoxOutlineAnim != null)
        {
            starGameManagerRef.UiManager.PerimeterBoxOutlineAnim = PerimeterBoxOutlineAnim;
        }

        if (FinalBossPlacementObject != null)
        {
            starGameManagerRef.FinalBossPlacementObject = FinalBossPlacementObject;
        }

        starGameManagerRef.localSceneManagement = gameObject.GetComponent<LocalSceneManagement>();
        

        if (TileGroups.Count != 0)
        {
            starGameManagerRef.TileMoverObject.GetComponent<TileMover>().ReplaceTileMoverGroup(TileGroups);
        }


        starGameManagerRef.ReassignItemsOnSceneLoad();


        if (BeginGameplayAtSceneLoad)
        {
            starGameManagerRef.BeginGameplay();
        }


        /*
         // rma1205
        if (LoadHeroShipLateStart)
        {
            ShipObject.SetActive(true);
        }
        */

        foreach (Rigidbody picked in starGameManagerRef.SleepPhysicsOnPause)
        {
            if (picked == null)
            {
                starGameManagerRef.SleepPhysicsOnPause.Remove(picked);
            }
        }


        if (SleepPhysicsOnPause != null)
        {
            foreach (Rigidbody picked in SleepPhysicsOnPause)
            {
                starGameManagerRef.SleepPhysicsOnPause.Add(picked);
            }
        }
        
    }

    /*
    private void Update()
    {
        if (ActivateElements)
        {
            ActivateElements = false;
            ActivateTheElements();
        }
    }
    */


   public void ActivateTheElements()
    {
        foreach (GameObject picked in ElementsToActivate)
        {
            picked.SetActive(true);
        }

        if (BeginManagerGameplaySettingsAtActivate)
        {
            starGameManagerRef.BeginGameplay(heroSpawnPoint);
        }

        /*
        if (DisableThisAfterActivate)
        {
            gameObject.SetActive(false);
        }
        */
    }





}




