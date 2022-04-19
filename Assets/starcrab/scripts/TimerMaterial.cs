using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimerMaterial : MonoBehaviour
{

    private StarGameManager starGameManagerRef;
    public enum OnFinish {Nothing, Disable, Restart}
    public OnFinish onFinish;
    public bool pauseable;
    public float offset;
    private int activeCoroutineCounter;

    public GameObject thisGameObject;
    public int UseSubID;
    private Material thisMaterial;
    public bool performedAssignCheck;

   [System.Serializable]

    public class instanceList

    {
        public string note = "";
        public bool mute;
        public float duration;
        public float randomOffset;
        [HideInInspector]
        public float useDuration;
        [HideInInspector]
        public float previousDuration;
        //  public UnityEvent thisEvent;
        public Color currentColor = new Vector4 (1,1,1,1);
        public Color currentEmissive = new Vector4 (0, 0, 0, 1);
        public float changeDuration = 1.0f;
        public float changeVariance = 0.7f;
        [HideInInspector]
        public Color initColor, initEmissive;
      
    }

    public List<instanceList> InstanceList;

    [HideInInspector]  // probably disable this HiD during testing later
    public List<float> Timeslist = new List<float>();

    /*
    private void Start()
    {

        if (thisGameObject == null)
        {
            thisGameObject = gameObject;
        }

        Material[] materials;
        materials = thisGameObject.GetComponent<Renderer>().materials;
        thisMaterial = materials[UseSubID];
    }
    */

    //  private IEnumerator TimedSelect(float duration, UnityEvent usedEvent)
    private IEnumerator TimedSelect(float duration, float changeDuration, Color currentColor, Color currentEmissive)


    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

            yield return new WaitForSeconds(duration);

        if (pauseable && starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }

        // usedEvent.Invoke();


        Color initColor = thisMaterial.color;
        Color initEmissive = thisMaterial.GetColor("_EmissionColor");

        for (float i = 0.01f; i < changeDuration; i += 0.1f)
        {
            thisMaterial.SetColor("_Color", Color.Lerp(initColor, currentColor, i / changeDuration));
            thisMaterial.SetColor("_EmissionColor", Color.Lerp(initEmissive, currentEmissive, i / changeDuration));
            yield return null;

        }
        


        if (activeCoroutineCounter < Timeslist.Count)
        {
            RunNewCoroutine();
        }


        else

        {
            switch (onFinish)
            {
                case OnFinish.Disable:
                    gameObject.SetActive(false);
                    break;

                case OnFinish.Restart:
                    activeCoroutineCounter = 0;
                    RunNewCoroutine();
                    break;

                case OnFinish.Nothing:
                    break;

                default:
                    break;
            }
        }
    }



    // --------------------------------------------------







void AssignedCheck()

    {
        if (thisGameObject == null)
        {
            thisGameObject = gameObject;
        }

        Material[] materials;
        materials = thisGameObject.GetComponent<Renderer>().materials;
        thisMaterial = materials[UseSubID];
        performedAssignCheck = true;
    }













    void Awake()

    {

      //  AssignedCheck();

      


        float cumulativeDuration = 0.0f;


        // TO DO - probably sort everything into a different list or array first, by initial duration
        // and then use that list below and in RunNewCoroutine rather than InstanceList - 
        // Call it "OrderedList" or something. In case durations are placed out of order for no good reason

         /*

        

            foreach (instanceList inList in InstanceList)
        {
         //   if (!inList.mute)

        //    {
                //  InstanceList.Sort(inList.duration)
                //  InstanceList = InstanceList.OrderBy(x => x.GetComponent<>().initiative).ToList();
                InstanceList = InstanceList.OrderBy(x => x.duration).ToList();
         //   }
        }

        print("instanceList "+ InstanceList);

        */


                foreach (instanceList inList in InstanceList)
        {
            if (!inList.mute)

            {
                inList.randomOffset = Random.Range(0, inList.randomOffset);
                inList.useDuration = inList.duration + offset + inList.randomOffset;
                inList.useDuration = inList.useDuration - cumulativeDuration;
                cumulativeDuration = cumulativeDuration + inList.useDuration;
                Timeslist.Add(inList.useDuration);

                inList.changeDuration = inList.changeDuration + Random.Range(0, inList.changeVariance);
            }
        }
    }

  


  //  public void PlayTimer(float duration, UnityEvent usedEvent)
      public void PlayTimer(float duration, float changeDuration, Color currentColor, Color currentEmissive)

    {
        //   StartCoroutine(TimedSelect(duration, usedEvent));
        StartCoroutine(TimedSelect(duration, changeDuration, currentColor, currentEmissive));
        
    }


    void RunNewCoroutine()
    {
        PlayTimer(Timeslist[activeCoroutineCounter], InstanceList[activeCoroutineCounter].changeDuration,
              InstanceList[activeCoroutineCounter].currentColor, InstanceList[activeCoroutineCounter].currentEmissive);

        activeCoroutineCounter++;
    }


    void OnEnable()
    {
        AssignedCheck();
        activeCoroutineCounter = 0;
        RunNewCoroutine();
    }
    
}
