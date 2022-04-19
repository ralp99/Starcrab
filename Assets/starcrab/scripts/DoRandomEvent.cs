using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoRandomEvent : MonoBehaviour {

    [System.Serializable]
    public class instanceList

    {
        public string Note = "";
        public UnityEvent RandomEvent;
    }

    public List<instanceList> InstanceList;

    public void ExecuteRandomEvent()
    {
        int selectedNumber = Random.Range(0, InstanceList.Count);
        InstanceList[selectedNumber].RandomEvent.Invoke();
    }
}
