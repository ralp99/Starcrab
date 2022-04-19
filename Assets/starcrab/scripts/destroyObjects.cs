using UnityEngine;
using System.Collections;

public class destroyObjects : MonoBehaviour {

	public GameObject[] objectsToDestroy;
    public float wait;

    void OnEnable()
    {

        StartCoroutine(DestroyTimer());


    }

    IEnumerator DestroyTimer()
    {

        yield return new WaitForSeconds(wait);

        DoDestroy();
        
    }


	void DoDestroy () {
	
		foreach (GameObject picked in objectsToDestroy) 
		{
			Destroy (picked);
		}


		gameObject.SetActive(false);

	}




}
