using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spriteCycle: MonoBehaviour {

	public GameObject objectUsing;
	public float duration = 1.21f;
	public bool countPerFrame;
	public bool disableOnFinish;
	public bool loop;
//	public bool pingPong;
	public Sprite[] spriteNew;
	float counter;
	Image imageUse;
	//Texture origtex;


	private IEnumerator TimedSelect ()
	{
		for(int i=0; i<spriteNew.Length; i++)
		{
			if(!imageUse) yield return null;

            if (objectUsing.GetComponent<SpriteRenderer>() != null)
            {
                objectUsing.GetComponent<SpriteRenderer>().sprite = spriteNew[i];

            }
               else imageUse.sprite = spriteNew[i];

			yield return new WaitForSeconds(counter);

			if (i >= (spriteNew.Length-1))
			{
				if (disableOnFinish == true)
					gameObject.SetActive(false);
				if (loop)
					i = 0;
			}

		}

	}

	public void PlayTimer ()
	{
		StartCoroutine(TimedSelect());

	}


	void Start () {

        if (objectUsing == null)
        {
            objectUsing = gameObject;
        }

		imageUse = objectUsing.GetComponent<Image> ();
		if (!countPerFrame)
			counter = duration / spriteNew.Length;
		else
			counter = duration;

	}



	void OnEnable()
	{
		PlayTimer ();

	}

}
