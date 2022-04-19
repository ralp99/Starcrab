using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiPrintScore : MonoBehaviour {

    public enum WatchSelect { Player, High, Lives }
    public WatchSelect watchSelect;
	public GameObject watchObject;
	public bool name, localPosition, worldPosition, rotation, scale, enabled;
	Text txt;
    TextMesh txtmesh;
    private bool useTextMesh;

	string useName, useLpos, useWpos, useRot, useSca, useActive;
    StarGameManager starGameManagerRef;

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



        if (watchSelect == WatchSelect.Player)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("Score " + starGameManagerRef.currentScore);

            }

            else

            {
                txtmesh.text = string.Format("Score " + starGameManagerRef.currentScore);
            }
            
        }


        if (watchSelect == WatchSelect.High)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("High " + starGameManagerRef.HighScore);

            }

            else

            {
                txtmesh.text = string.Format("High " + starGameManagerRef.HighScore);
            }

        }

        if (watchSelect == WatchSelect.Lives)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("Lives " + starGameManagerRef.currentLives);

            }

            else

            {
                txtmesh.text = string.Format("Lives " + starGameManagerRef.currentLives);
            }

        }




        /*
		if (name) useName = (watchObject.name+"\n"); 
		if (worldPosition) useWpos = ("Wpos "+watchObject.transform.position+"\n");
		if (localPosition) useLpos = ("Lpos "+watchObject.transform.localPosition+"\n");
		if (rotation) useRot = ("rot "+watchObject.transform.eulerAngles+"\n");
		if (scale) useSca = ("sca "+watchObject.transform.localScale+"\n");
		if (enabled) 
		{
			if (watchObject.activeSelf)
				useActive = "On";
			else
				useActive = "Off";
			}

        //	txt.text = " "+useName.ToString()+""+usePos.ToString(); // ALSO 2





        if (!useTextMesh)
        {
            txt.text = string.Format("" + useName + useLpos + useWpos + useRot + useSca + useActive);
        }

        else

        {
            txtmesh.text = string.Format("" + useName + useLpos + useWpos + useRot + useSca + useActive);
        }
       */



    }
}
