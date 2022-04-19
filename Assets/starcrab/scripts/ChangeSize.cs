using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour {


    public Vector3 BeginSize;
    public Vector3 EndSize;
    public float Speed;
    [HideInInspector] public StarGameManager starGameManagerRef;
    public bool DisableAtShrink;

	void Update () {
		
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (starGameManagerRef.GamePaused)
        {
            return;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, EndSize, Speed * Time.deltaTime);

        if (transform.localScale.x <= EndSize.x + 0.03f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        transform.localScale = BeginSize;
    }
    
}
