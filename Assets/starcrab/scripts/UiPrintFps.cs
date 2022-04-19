using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPrintFps : MonoBehaviour {


    public int AvgFrameRate;
   // public Text Display_text;
    public TextMesh DisplayMesh;
    

    void Update()
    {

        float current = 0;
        //  current = Time.frameCount / Time.time;
        current = (int)(1f / Time.unscaledDeltaTime);
        AvgFrameRate = (int)current;
       // Display_text.text = AvgFrameRate.ToString();
       DisplayMesh.text = AvgFrameRate.ToString();

    }
}
