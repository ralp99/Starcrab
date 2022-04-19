using UnityEngine;
using System.Collections;

public class playAnimator4 : MonoBehaviour {

	public GameObject[] animObject;
	public string triggera = "";
	public bool spam;
	public float threshold = .5f;
    public bool closeObjectOnFinish = true;
    public bool killComponentOnFinish;
    public Behaviour thisBehavior;

    bool playNext = false;

		void Update () {


		foreach (GameObject picked in animObject) picked.GetComponent<Animator> ().SetTrigger (triggera);

        if (spam)
        {
            if (closeObjectOnFinish) gameObject.SetActive(false);
            if (killComponentOnFinish) thisBehavior.enabled = false;
        }
        else

        {
            StartCoroutine(TimedSelect());
            if (playNext == true)
            {
                foreach (GameObject picked in animObject) picked.GetComponent<Animator>().ResetTrigger(triggera);

                playNext = false;
                if (closeObjectOnFinish) gameObject.SetActive(false);
                if (killComponentOnFinish) thisBehavior.enabled = false;

            }
            else
                playNext = false;
        }

						}


			private IEnumerator TimedSelect ()
						{
							yield return new WaitForSeconds(threshold);
				
							playNext = true;
						}


















		// TO DO - ADD SPAMMING, SPAMMING DELAY TIME

//			StartCoroutine(TimedSelect());
//			if (playNext == true) {
//				animObject.GetComponent<Animator> ().ResetTrigger (triggera);
//				playNext = false;
//				gameObject.SetActive (false);
//			} else
//				playNext = false;
//	
//		}
//
//	
//		private IEnumerator TimedSelect ()
//		{
//			yield return new WaitForSeconds(.5f);
//
//			playNext = true;
//	
//		}


}


