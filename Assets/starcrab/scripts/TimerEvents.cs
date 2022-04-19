using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvents : MonoBehaviour
{

    private StarGameManager starGameManagerRef;
    public enum OnFinish {Nothing, Disable, Restart, Destroy}
    public OnFinish onFinish;
    public bool pauseable;
    public float offset;

    private int activeCoroutineCounter;


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
       // [HideInInspector]
       // public int id;
        public UnityEvent thisEvent;
      
    }

    public List<instanceList> InstanceList;

    [HideInInspector]  // probably disable this HiD during testing later
    public List<float> Timeslist = new List<float>();


    private IEnumerator TimedSelect(float duration, UnityEvent usedEvent)

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

        usedEvent.Invoke();

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

                case OnFinish.Destroy:
                    Destroy(gameObject);
                    break;

                default:
                    break;
            }
        }
    }

    void Awake()

    {
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
            }
        }
    }




    public void PlayTimer(float duration, UnityEvent usedEvent)

    {
        StartCoroutine(TimedSelect(duration, usedEvent));
    }
    

    void RunNewCoroutine()
    {

        PlayTimer(Timeslist[activeCoroutineCounter], InstanceList[activeCoroutineCounter].thisEvent);
        activeCoroutineCounter++;
    }


    void OnEnable()
    {
        activeCoroutineCounter = 0;
        RunNewCoroutine();
    }
    
}
