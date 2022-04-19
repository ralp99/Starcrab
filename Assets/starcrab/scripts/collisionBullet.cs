using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class collisionBullet : MonoBehaviour {

    // public GameObject watchThis;
    StarGameManager starGameManagerRef;



    [System.Serializable]

    public class instanceList

    {
        public string messageCheck = "";
        public GameObject[] objectEnable;
        public GameObject[] objectDisable;
        public Behaviour[] behaviorEnable;
        public Behaviour[] behaviorDisable;
        public int pointValue;
        public bool mute;
       [HideInInspector] public bool canGo;
    }


    public List<instanceList> InstanceList;

    public void collideNoticed(string receivedMessage)

    {
        foreach (instanceList inList in InstanceList)
        {
            if (receivedMessage == inList.messageCheck)
            { inList.canGo = true; }
        }
      
    }



    void Start()
    {
        starGameManagerRef = StarGameManager.instance;

    }



    void Update()
    {
        

        foreach (instanceList inList in InstanceList)
        {
            if (inList.canGo)
            {

                if (!inList.mute)
                {
                    if (inList.pointValue != 0)

                    {
                      //  watchThis.GetComponent<ValueStore>().currentScore = watchThis.GetComponent<ValueStore>().currentScore + inList.pointValue;
                        starGameManagerRef.currentScore = starGameManagerRef.currentScore + inList.pointValue;
                    }


                    if (inList.objectEnable != null)
                        foreach (GameObject picked in inList.objectEnable) picked.SetActive(true);

                    if (inList.objectDisable != null)
                        foreach (GameObject picked in inList.objectDisable) picked.SetActive(false);

                    if (inList.behaviorEnable != null)
                        foreach (Behaviour picked in inList.behaviorEnable) picked.enabled = true;

                    if (inList.behaviorDisable != null)
                        foreach (Behaviour picked in inList.behaviorDisable) picked.enabled = false;

                }

        }
            inList.canGo = false;

        }
        
    }
    
}
