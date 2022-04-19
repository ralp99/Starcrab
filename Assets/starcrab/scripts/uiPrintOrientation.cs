using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiPrintOrientation : MonoBehaviour {

    public enum WatchSelect { Camera, Playfield, Control }
    public WatchSelect watchSelect;
	public GameObject watchObject;
	public bool name, localPosition, worldPosition, rotation, scale, enabled;
	Text txt;
    TextMesh txtmesh;
    private bool useTextMesh;

	string useName, useLpos, useWpos, useRot, useSca, useActive;

	void Start () {

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



        if (watchSelect == WatchSelect.Camera)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("Cam " + watchObject.GetComponent<StarShipController>().cameraOriented);

            }

            else

            {
                txtmesh.text = string.Format("Cam "+watchObject.GetComponent<StarShipController>().cameraOriented);
            }
            
        }


        if (watchSelect == WatchSelect.Playfield)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("Playfield " + watchObject.GetComponent<StarShipController>().playfieldOriented);

            }

            else

            {
                txtmesh.text = string.Format("Playfield " + watchObject.GetComponent<StarShipController>().playfieldOriented);
            }

        }


        if (watchSelect == WatchSelect.Control)
        {

            if (!useTextMesh)
            {
                txt.text = string.Format("Control " + watchObject.GetComponent<StarShipController>().controlOrient);

            }

            else

            {
                txtmesh.text = string.Format("Control " + watchObject.GetComponent<StarShipController>().controlOrient);
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
