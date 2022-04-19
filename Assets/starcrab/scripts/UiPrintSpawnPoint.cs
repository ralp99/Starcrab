using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPrintSpawnPoint : MonoBehaviour {


    StarGameManager starGameManagerRef;

    Text txt;
    TextMesh txtmesh;
    private bool useTextMesh;

    void Start () {

        starGameManagerRef = StarGameManager.instance;

        if (gameObject.GetComponent<Text>())
        {
            txt = gameObject.GetComponent<Text>();
        }

        else

        {
            txtmesh = gameObject.GetComponent<TextMesh>();
            useTextMesh = true;
        }
    }


    void Update () {
        if (!useTextMesh)
        {
            txt.text = string.Format(" " + starGameManagerRef.currentHeroSpawnPoint);

        }

        else

        {
            txtmesh.text = string.Format(" " + starGameManagerRef.currentHeroSpawnPoint);
        }
    }
}
