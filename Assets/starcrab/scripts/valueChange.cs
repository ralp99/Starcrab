using UnityEngine;
using System.Collections;

public class valueChange : MonoBehaviour {

 //   public GameObject watchThis;
    public string field;
    public int value;
    StarGameManager starGameManagerRef;

    void Start () {
        starGameManagerRef = StarGameManager.instance;

    }

    void Update () {
        // watchThis.GetComponent<ValueStore>().field = watchThis.GetComponent<ValueStore>().field + value;
        //   watchThis.GetComponent<ValueStore>().currentHealth = watchThis.GetComponent<ValueStore>().currentHealth + value;
        starGameManagerRef.currentHealth = starGameManagerRef.currentHealth + value;
        gameObject.SetActive(false);
    }
}
