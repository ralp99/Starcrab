using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class Sensor_RAM2 : MonoBehaviour {

    public bool UseStandardHighlightColor = true;
    public Color HighlightColor;
    Color useHighlightColor;

    public GameObject HighlightOverride;
    private bool canHighlight;
    public bool HighlightOnGaze;
    public UnityEvent GazeEvent;
    public UnityEvent GazeOffEvent;
    public UnityEvent DwellEvent;
    public float GazeWait;
    public float GazeOffWait;
    public float DwellWait;
    float useDwellWait;
    public bool UseBubbleSelectTimeForDwellWait;
    private IEnumerator currentGazeWaitCoroutine;
    private IEnumerator currentGazeOffWaitCoroutine;
    private IEnumerator currentDwellWaitCoroutine;
   // [HideInInspector]
    public bool selected;

  //  [HideInInspector]
    public Color origColor;  //mkprv
    private Material material;
   // private GameObject detectedParentObject;
    private StarGameManager starGameManagerRef;
    public StarUiManager uiManager;

    public GameObject BubbleSpawnPoint;
    private GameObject bubbleChildHolder;

    private GameObject bubbleChild;
    [HideInInspector]
    public Sensor_RAM2 parentSensor;
    private Renderer bubbleChildRenderer;
    private Collider bubbleChildCollider;

   // [HideInInspector]
    public bool childBubbleIsGazedOn;
    [HideInInspector]
    public IEnumerator LastReleaseGazeCoroutine;
    public raycastTimer raycastTimerThatHitMe;

    bool bubbleChildTrack;

    public bool ForceIgnoreInactiveParent;

    public List<string> InclusiveRayTag = new List<string>();  // will use only these
    public List<string> ExclusiveRayTag = new List<string>();  // will ignore only these. DON'T NEED BOTH

    public bool UseDistanceThreshold;
    public float DistanceThresholdOverride;
    private Transform userTransform;

    public bool WorkInMenuNavMode = true;

    // public void animateNoticed(bool happens)

    public float ActiveDelayOverride;
    public bool UseActiveDelay;
    private bool SensorInitialized;

    AirtapController2 airtapController2 = null;

    IEnumerator DelayingSensorInitialize()
    {
        yield return new WaitForSeconds(ActiveDelayOverride);
        SensorInitialized = true;
    }


    void CreateBubbleSelector()
    {
        bubbleChildHolder = Instantiate(uiManager.BubbleSelectObject) as GameObject;
      //  bubbleChildHolder.transform.parent = BubbleSpawnPoint.transform;
     //   bubbleChildHolder.transform.localPosition = Vector3.zero;
        bubbleChild = bubbleChildHolder.transform.GetChild(0).gameObject;
        Sensor_RAM2 bubbleSensor = bubbleChild.GetComponent<Sensor_RAM2>();

        bubbleSensor.parentSensor = gameObject.GetComponent<Sensor_RAM2>();
        bubbleSensor.DwellEvent = DwellEvent;
        bubbleChildRenderer = bubbleChild.GetComponent<Renderer>();
        bubbleChildCollider = bubbleChild.GetComponent<Collider>();
        bubbleChildTrack = true;
    }

    private bool canHitFalse = true;

    private void Update()
    {
        if (bubbleChildTrack)
        {
            bubbleChildHolder.transform.position = BubbleSpawnPoint.transform.position;
            bubbleChildHolder.transform.rotation = BubbleSpawnPoint.transform.rotation;
        }

        if (SensorInitialized)
        {
            if (raycastTimerThatHitMe != null)
            {
                //   CheckMyStatus();
                BeingHit(true);
                canHitFalse = true;
            }
            else
            {
                if (canHitFalse)
                {
                    BeingHit(false);  // dont do this more than once
                    canHitFalse = false;
                }
            }
            

        }

       


    }

    private void OnDisable()
    {
        if (bubbleChildHolder != null)
        {
            bubbleChildHolder.SetActive(false);
        }
    }





    public void RaycastFilterCheck(raycastTimer raycastTimerCheck)  // 1st line of defense

    {
        if (starGameManagerRef != null)
        {
            if (!WorkInMenuNavMode && starGameManagerRef.controllerInteractionMode != ControllerInteractionMode.Teleport)
            {
                return;
            }
        }
      

        if (UseDistanceThreshold)
        {
            if (userTransform != null)
            {
                if (Vector3.Distance(gameObject.transform.position, userTransform.position) >
                                    DistanceThresholdOverride)
                {
                    return;
                }
            }
        }
        
        bool canRegisterHit = true;

        if (InclusiveRayTag.Count != 0)
        {
            if (InclusiveRayTag.Contains(raycastTimerCheck.tag))
            {
                canRegisterHit = true;
            }
            else
            {
                // not contained in inclusive list
                canRegisterHit = false;
            }
        }
        else
        {
            canRegisterHit = true;
        }

        if (ExclusiveRayTag.Count != 0)
        {
            if (ExclusiveRayTag.Contains(raycastTimerCheck.tag))
            {
                canRegisterHit = false;
            }
            else
            {
                canRegisterHit = true;
            }
        }

        if (canRegisterHit)
        {
            raycastTimerThatHitMe = raycastTimerCheck;
        }
        else
        {
            raycastTimerThatHitMe = null;
        }
    }


    /*

    public void CheckMyStatus()
    {
        // failsafe if the raycast moves too quickly off the bubble
       if (raycastTimerThatHitMe == null)
        {
            BeingHit(false);
        }
       else
        {
            BeingHit(true);  // i think!!
        }

    }
    */

    public void BeingHit(bool happens)

    {

        if (happens)
        {
            selected = true;
            // hitting

            if (parentSensor != null)  // if it is a bubbleSensor
            {
                parentSensor.childBubbleIsGazedOn = true;
                parentSensor.BeingHit(true);
            }

            if (currentDwellWaitCoroutine != null)
            {
                StopCoroutine(currentDwellWaitCoroutine);
            }

            currentGazeWaitCoroutine = NewEvent(GazeWait, GazeEvent);

            if (BubbleSpawnPoint != null)
          //  if a bubbleSensor is present, enable it

            {
                if (bubbleChildHolder == null)
                {
                    CreateBubbleSelector();
                }
                else
                {
                    bubbleChildRenderer.enabled = true;
                    bubbleChildCollider.enabled = true;
                }
            }
            
            StartCoroutine(currentGazeWaitCoroutine);

            if (!BubbleSpawnPoint)
            {
                currentDwellWaitCoroutine = NewEvent(useDwellWait, DwellEvent);
                StartCoroutine(currentDwellWaitCoroutine);
            }

            if (currentGazeOffWaitCoroutine != null)
            {
                StopCoroutine(currentGazeOffWaitCoroutine);
            }

        }

        else

        {
            // released
            selected = false;

            raycastTimerThatHitMe = null;

            if (parentSensor != null)
            {
                parentSensor.childBubbleIsGazedOn = false;
                //  parentSensor.CheckMyStatus();  // need to do BeingHit instead!  //rma2020_02_20
                parentSensor.BeingHit(false); // I THINK! need to test this  //rma2020_02_20
            }

            if (bubbleChild != null)
            {
                bubbleChildRenderer.enabled = false;
                bubbleChildCollider.enabled = false;
            }

            currentGazeOffWaitCoroutine = NewEvent(GazeOffWait, GazeOffEvent);

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(currentGazeOffWaitCoroutine);
            }
            
            // can delete these?

            if (currentGazeWaitCoroutine != null)
            {
                StopCoroutine(currentGazeWaitCoroutine);
            }

            if (currentDwellWaitCoroutine != null)
            {
                StopCoroutine(currentDwellWaitCoroutine);
            }



        }
    }

    

    private IEnumerator NewEvent(float duration, UnityEvent thisEvent)
    {
        yield return new WaitForSeconds(duration);

        if (thisEvent != null)
        {
            thisEvent.Invoke();
        }


        if (canHighlight && HighlightOnGaze)
        {

            if (thisEvent == GazeEvent)
            {
                HighlightMaterial();
            }

            if (thisEvent == GazeOffEvent && !selected)
            {
                // might need to extend those conditions to invoking gazeOffEvent, unsure
                RevertMaterial();
            }
        }
    }

    private void HighlightMaterial()
    {
        material.color = useHighlightColor;
    }

    private void RevertMaterial()
    {
        material.color = origColor;
    }

    private void OnEnable()
    {

        if (bubbleChildHolder != null)
        {
            bubbleChildHolder.SetActive(true);
        }

        if (HighlightOverride == null)
        {
            HighlightOverride = this.gameObject;
        }

        if (canHighlight)
        {
            RevertMaterial();
        }
    }


    private void Start()
    {
        starGameManagerRef = StarGameManager.instance;
        uiManager = starGameManagerRef.UiManager;
        userTransform = starGameManagerRef.CameraHolder.transform;


        if (GetComponent<AirtapController2>())
        {
            airtapController2 = GetComponent<AirtapController2>();
        }
        
        if (UseStandardHighlightColor)
        {
            useHighlightColor = uiManager.StandardButtonHighlightColor;
        }
        else
        {
            useHighlightColor = HighlightColor;
        }

        if (HighlightOverride.GetComponent<Renderer>().material.HasProperty("_Color"))
        {
            material = HighlightOverride.GetComponent<Renderer>().material;
            origColor = material.color;
            canHighlight = true;
        }

      //  if (WatchInactiveParent != null)
         if (!ForceIgnoreInactiveParent)
        {
      //      detectedParentObject = gameObject.transform.parent.transform.gameObject;
        }

        if (UseBubbleSelectTimeForDwellWait)
        {
            useDwellWait = uiManager.BubbleSelectLoadTime;
        }
        else
        {
            useDwellWait = DwellWait;
        }

        if (DistanceThresholdOverride == 0)
        {
            DistanceThresholdOverride = starGameManagerRef.DistanceGrabThreshold;
        }

        if (UseActiveDelay)
        {
            if (ActiveDelayOverride == 0)
            {
                ActiveDelayOverride = uiManager.SensorActiveDelay;
            }

            StartCoroutine(DelayingSensorInitialize());
        }

        else
        {
            SensorInitialized = true;
        }

    }

}

