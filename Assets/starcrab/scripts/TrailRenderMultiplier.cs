using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRenderMultiplier : MonoBehaviour {

    private StarGameManager starGameManagerRef;

    public TrailRenderer trailRenderer;

    private float initMultiplier = 0.006f;


    public void AdjustTrailRenderMultiplier()
    {
        // currentMultiplier = initMultiplier * starGameManagerRef.StageSize;

        // trailRenderer.emitting = trailRenderer.emitting * currentMultiplier;


          trailRenderer.widthMultiplier = initMultiplier * starGameManagerRef.StageSize;

    }

    
    void Start () {
      //  starGameManagerRef = StarGameManager.instance;
     //   initMultiplier = trailRenderer.widthMultiplier;  //probably keep

        if (!starGameManagerRef.TrailRendererResize.Contains(gameObject))
        {
            starGameManagerRef.TrailRendererResize.Add(gameObject);
        }

        AdjustTrailRenderMultiplier();

    }

    private void OnEnable()
    {
          // .006
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (starGameManagerRef != null)
        {
            AdjustTrailRenderMultiplier();
        }
    }
}
