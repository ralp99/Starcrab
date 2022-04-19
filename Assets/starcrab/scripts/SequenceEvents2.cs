using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SequenceEvents2 : MonoBehaviour {

    public bool StopAtBeginning;
    public bool StopAtEnd;

    private int sequenceEvent;
    private bool currentlyGoingFwd;
    private bool currentlyGoingReverse;
    private bool reachedEnd;

    [System.Serializable]

    public class instanceList

    {
        [HideInInspector] public int SequenceId;

        public string note = "";
        public UnityEvent thisEvent;
    }

    public List<instanceList> InstanceList;



    private void Start()
    {
        int currentID = 0;

        foreach (instanceList currentList in InstanceList)
        {
            currentList.SequenceId = currentID;
            currentID++;
        }

    }


    public void GoToEntry(int entry)

    {

        if (entry < 0)
        {
            entry = 0;
        }

        if (entry > InstanceList.Count - 1)
        {
            entry = InstanceList.Count - 1;
        }

        sequenceEvent = entry;
        InstanceList[sequenceEvent].thisEvent.Invoke();
    }

    public void GoToLastEntry()

    {
        sequenceEvent = InstanceList.Count-1;
        InstanceList[sequenceEvent].thisEvent.Invoke();
    }


public void PrevSequence()

    {

        if (currentlyGoingFwd)

        {
            currentlyGoingFwd = false;
            sequenceEvent--;
        }

        sequenceEvent--;

        if (sequenceEvent < 0)
        {

            if (!StopAtBeginning)

            {
                sequenceEvent = InstanceList.Count - 1;
            }

            else

            {
                sequenceEvent = 0;
            }
        }

     
        InstanceList[sequenceEvent].thisEvent.Invoke();

        currentlyGoingReverse = true;

    }

    public void NextSequence()
    {

        if (currentlyGoingReverse)
        {
            currentlyGoingReverse = false;
            sequenceEvent++;

            if (sequenceEvent > InstanceList.Count - 1)

            {
                if (!StopAtEnd)

                {
                    sequenceEvent = 0;
                }

                else

                {
                    sequenceEvent = InstanceList.Count - 1;
                  
                }
            }
        }


        InstanceList[sequenceEvent].thisEvent.Invoke();

        sequenceEvent++;

        if (sequenceEvent > InstanceList.Count-1 )
        {

            if (!StopAtEnd)

            {
                sequenceEvent = 0;
            }

            else

            {
                sequenceEvent = InstanceList.Count - 1;
                reachedEnd = true;
            }
        }


        if (!reachedEnd)
        {
            currentlyGoingFwd = true;
        }

        else

        {
            currentlyGoingFwd = false;
            reachedEnd = false;
        }

    }



}
